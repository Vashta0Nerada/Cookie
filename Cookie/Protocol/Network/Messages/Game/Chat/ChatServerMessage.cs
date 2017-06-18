//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cookie.Protocol.Network.Messages.Game.Chat
{
    using System.Collections.Generic;
    using Cookie.Protocol.Network.Messages;
    using Cookie.Protocol.Network.Types;
    using Cookie.IO;
    
    
    public class ChatServerMessage : ChatAbstractServerMessage
    {
        
        public new const uint ProtocolId = 881;
        
        public override uint MessageID
        {
            get
            {
                return ProtocolId;
            }
        }
        
        private double m_senderId;
        
        public virtual double SenderId
        {
            get
            {
                return m_senderId;
            }
            set
            {
                m_senderId = value;
            }
        }
        
        private string m_senderName;
        
        public virtual string SenderName
        {
            get
            {
                return m_senderName;
            }
            set
            {
                m_senderName = value;
            }
        }
        
        private int m_senderAccountId;
        
        public virtual int SenderAccountId
        {
            get
            {
                return m_senderAccountId;
            }
            set
            {
                m_senderAccountId = value;
            }
        }
        
        public ChatServerMessage(double senderId, string senderName, int senderAccountId)
        {
            m_senderId = senderId;
            m_senderName = senderName;
            m_senderAccountId = senderAccountId;
        }
        
        public ChatServerMessage()
        {
        }
        
        public override void Serialize(ICustomDataOutput writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(m_senderId);
            writer.WriteUTF(m_senderName);
            writer.WriteInt(m_senderAccountId);
        }
        
        public override void Deserialize(ICustomDataInput reader)
        {
            base.Deserialize(reader);
            m_senderId = reader.ReadDouble();
            m_senderName = reader.ReadUTF();
            m_senderAccountId = reader.ReadInt();
        }
    }
}