﻿using Cookie.API.Protocol.Network.Messages.Game.Social;
using Cookie.API.Utils.IO;

namespace Cookie.API.Protocol.Network.Messages.Game.Guild
{
    public class GuildBulletinSetErrorMessage : SocialNoticeSetErrorMessage
    {
        public new const ushort ProtocolId = 6691;

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