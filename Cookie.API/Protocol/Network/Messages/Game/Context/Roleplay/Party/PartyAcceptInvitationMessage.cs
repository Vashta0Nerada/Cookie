﻿using Cookie.API.Utils.IO;

namespace Cookie.API.Protocol.Network.Messages.Game.Context.Roleplay.Party
{
    public class PartyAcceptInvitationMessage : AbstractPartyMessage
    {
        public new const ushort ProtocolId = 5580;

        public override ushort MessageID => ProtocolId;

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
    }
}