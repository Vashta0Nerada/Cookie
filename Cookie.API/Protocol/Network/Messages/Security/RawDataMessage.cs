﻿using Cookie.API.Utils.IO;
using System;

namespace Cookie.API.Protocol.Network.Messages.Security
{
    public class RawDataMessage : NetworkMessage
    {
        public const ushort ProtocolId = 6253;

        public RawDataMessage()
        {
            Console.WriteLine("Raw data crated");
        }

        public RawDataMessage(byte[] content)
        {
            Content = content;
            Console.WriteLine("Raw data crated");
        }

        public override ushort MessageID => ProtocolId;
        public byte[] Content { get; set; }

        public override void Serialize(IDataWriter writer)
        {
            var contentLength = Content.Length;
            writer.WriteVarInt(contentLength);
            for (var i = 0; i < contentLength; i++)
                writer.WriteByte(Content[i]);
        }

        public override void Deserialize(IDataReader reader)
        {
            var contentLength = reader.ReadVarInt();
            reader.ReadBytes(contentLength);
        }
    }
}