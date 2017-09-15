using Cookie.API.Messages;
using Cookie.API.Protocol.Network.Messages.Game.Context.Roleplay.Npc;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using Cookie.API.Core;
using Cookie.API.Protocol.Network.Messages.Game.Dialog;
using Cookie.LUA.Objects;
using Cookie.API.Utils;
using System.Threading;
using Cookie.API.Gamedata.D2i;
using Cookie.API.Gamedata.D2o;
using Cookie.API.Datacenter;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class NpcLUA : BaseLUA
    {
        private AutoResetEvent _messageReceived;

        public bool InDialog;
        public uint LastMessageID;
        public List<NpcReplyLUA> PossibleReplies;
        public string Name => FastD2IReader.Instance.GetText(_currentNpc?.NameId);
        public string Dialog
        {
            get
            {
                if (LastMessageID == 0)
                    return "";
                return FastD2IReader.Instance.GetText(ObjectDataManager.Instance.Get<API.Datacenter.NpcMessage>(LastMessageID).MessageId);
            }
        }

        private Npc _currentNpc;

        public NpcLUA(DataLUA data) : base(data)
        {
            LastMessageID = 0;
            InDialog = false;
            PossibleReplies = new List<NpcReplyLUA>();

            _messageReceived = new AutoResetEvent(false);

            _data.Account.Network.RegisterPacket<NpcDialogCreationMessage>(
                OnNpcDialogCreationMessage, MessagePriority.Normal);
            _data.Account.Network.RegisterPacket<NpcDialogQuestionMessage>(
                OnNpcDialogQuestionMessage, MessagePriority.Normal);
            _data.Account.Network.RegisterPacket<LeaveDialogMessage>(
                OnLeaveDialogMessage, MessagePriority.Normal);
            _data.Account.Network.RegisterPacket<NpcGenericActionFailureMessage>(
                OnNpcGenericActionFailureMessage, MessagePriority.Normal);
        }

        private void OnNpcGenericActionFailureMessage(IAccount arg1, NpcGenericActionFailureMessage arg2)
        {
            InDialog = false;
            PossibleReplies = new List<NpcReplyLUA>();
            _messageReceived.Set();
        }

        private void OnNpcDialogQuestionMessage(IAccount arg1, NpcDialogQuestionMessage dialogQuestion)
        {
            PossibleReplies = new List<NpcReplyLUA>();
            foreach (var replyid in dialogQuestion.VisibleReplies)
            {
                PossibleReplies.Add(new NpcReplyLUA((int)replyid, _currentNpc));
            }
            
            LastMessageID = dialogQuestion.MessageId;
            Console.WriteLine(LastMessageID.ToString());
            InDialog = true;
            _messageReceived.Set();
        }

        private void OnLeaveDialogMessage(IAccount arg1, LeaveDialogMessage arg2)
        {
            InDialog = false;
            PossibleReplies = new List<NpcReplyLUA>();
            Logger.Default.Log("Personnage a terminé la discussion avec le NPC " + Name + ".", API.Utils.Enums.LogMessageType.Info);
            _messageReceived.Set();
        }

        private void OnNpcDialogCreationMessage(IAccount arg1, NpcDialogCreationMessage arg2)
        {
            Logger.Default.Log("Le personnage est en discussion avec le NPC " + Name + ".", API.Utils.Enums.LogMessageType.Info);
        }

        /// <summary>
        /// Starts the dialog with an NPC
        /// </summary>
        /// <param name="npcid"></param>
        /// <param name="action"></param>
        /// <returns>True if a reply is expected, False otherwise</returns>
        public bool Talk(double npcid, int action = 3)
        {
            if (InDialog)
            {
                Logger.Default.Log("Impossible de lancer un dialogue, vous êtes déjà en conversation avec " + Name + " .", API.Utils.Enums.LogMessageType.Error);
                return false;
            }

            InDialog = false;

            var Npc = _data.Account.Character.Map.Npcs.Find(npc => npc.Id == npcid);
            if (Npc == null)
            {
                Logger.Default.Log("Aucun NPC avec l'id " + npcid.ToString() + " sur cette carte.", API.Utils.Enums.LogMessageType.Error);
                return false;
            }

            _currentNpc = ObjectDataManager.Instance.Get<Npc>(Npc.NpcId);

            if (!_currentNpc.Actions.Contains((uint)action))
            {
                Logger.Default.Log("Impossible d'effectuer l'action " + FastD2IReader.Instance.GetText(ObjectDataManager.Instance.Get<NpcAction>(action).NameId) + " avec ce NPC.", API.Utils.Enums.LogMessageType.Error);
                return false;
            }

            var message = new NpcGenericActionRequestMessage
            {
                NpcId = (int)Npc.Id,
                NpcActionId = (byte)action,
                NpcMapId = _data.Account.Character.MapId
            };

            _data.Account.Network.SendToServer(message);

            _messageReceived.WaitOne(5000);
            return InDialog;
        }

        public bool TalkWithCellId(int cellid, int action = 3)
        {
            var Npc = _data.Account.Character.Map.Npcs.Find(npc => npc.CellId == cellid);
            if (Npc == null)
            {
                Logger.Default.Log("Aucun NPC sur la CellId " + cellid.ToString() + " sur cette carte.", API.Utils.Enums.LogMessageType.Error);
                return false;
            }

            return Talk(Npc.Id, action);
        }

        public bool Reply(int replyid)
        {
            if(replyid < 0)
            {
                if (PossibleReplies.Count < Math.Abs(replyid))
                {
                    Logger.Default.Log("La réponse " + replyid.ToString() + " n'existe pas.", API.Utils.Enums.LogMessageType.Error);
                    return false;
                }
                replyid = PossibleReplies[Math.Abs(replyid) - 1].Id;
            }
            var message = new NpcDialogReplyMessage((uint)replyid);

            _data.Account.Network.SendToServer(message);

            _messageReceived.WaitOne(5000);
            return InDialog;
        }

        public bool LeaveDialog()
        {
            var message = new LeaveDialogRequestMessage();

            _data.Account.Network.SendToServer(message);

            _messageReceived.WaitOne(5000);
            return !InDialog;
        }
    }
}
