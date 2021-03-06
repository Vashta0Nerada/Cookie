﻿using Cookie.API.Protocol.Network.Types.Game.Mount;
using Cookie.API.Utils.IO;

namespace Cookie.API.Protocol.Network.Messages.Game.Context.Mount
{
    public class MountDataMessage : NetworkMessage
    {
        public const ushort ProtocolId = 5973;

        public MountDataMessage(MountClientData mountData)
        {
            MountData = mountData;
        }

        public MountDataMessage()
        {
        }

        public override ushort MessageID => ProtocolId;
        public MountClientData MountData { get; set; }

        public override void Serialize(IDataWriter writer)
        {
            MountData.Serialize(writer);
        }

        public override void Deserialize(IDataReader reader)
        {
            MountData = new MountClientData();
            MountData.Deserialize(reader);
        }
    }
}