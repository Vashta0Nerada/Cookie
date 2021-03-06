﻿using Cookie.API.Utils.IO;

namespace Cookie.API.Protocol.Network.Messages.Game.Context
{
    public class GameEntityDispositionErrorMessage : NetworkMessage
    {
        public const ushort ProtocolId = 5695;

        public override ushort MessageID => ProtocolId;

        public override void Serialize(IDataWriter writer)
        {
        }

        public override void Deserialize(IDataReader reader)
        {
        }
    }
}