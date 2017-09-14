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

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class NpcLUA : BaseLUA
    {
        private AutoResetEvent _messageReceived;

        public bool InDialog;
        public List<NpcReplyLUA> PossibleReplies;

        public NpcLUA(DataLUA data) : base(data)
        {
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
            List<NpcReplyLUA> list = new List<NpcReplyLUA>();
            foreach (var replyid in dialogQuestion.VisibleReplies)
            {
                list.Add(new NpcReplyLUA((int)replyid));
            }

            InDialog = true;
            _messageReceived.Set();
        }

        private void OnLeaveDialogMessage(IAccount arg1, LeaveDialogMessage arg2)
        {
            InDialog = false;
            PossibleReplies = new List<NpcReplyLUA>();
            _messageReceived.Set();
        }

        private void OnNpcDialogCreationMessage(IAccount arg1, NpcDialogCreationMessage arg2)
        {
            InDialog = true;
            Logger.Default.Log("Personnage en discussion avec le NPC " + arg2.NpcId.ToString() + ".", API.Utils.Enums.LogMessageType.Info);
        }

        /// <summary>
        /// Starts the dialog with an NPC
        /// </summary>
        /// <param name="npcid"></param>
        /// <param name="action"></param>
        /// <returns>True if a reply is expected, False otherwise</returns>
        public bool Talk(double npcid, int action = 3)
        {
            InDialog = false;

            var Npc = _data.Account.Character.Map.Npcs.Find(npc => npc.Id == npcid);
            if (Npc == null)
            {
                Logger.Default.Log("Aucun NPC avec l'id " + npcid.ToString() + " sur cette carte.", API.Utils.Enums.LogMessageType.Error);
                return false;
            }

            var npcUId = Convert.ToInt32(Npc.NpcId);
            var npcMapId = Convert.ToInt32(Npc.Id);
            var npcActionId = (byte)action;

            var message = new NpcGenericActionRequestMessage
            {
                NpcId = npcMapId,
                NpcActionId = npcActionId,
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
    }
}
