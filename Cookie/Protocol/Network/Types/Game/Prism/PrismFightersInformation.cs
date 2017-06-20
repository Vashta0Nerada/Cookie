//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cookie.Protocol.Network.Types.Game.Prism
{
    using Cookie.IO;
    using Cookie.Network;
    using Cookie.Protocol.Network.Types.Game.Character;
    using Cookie.Protocol.Network.Types.Game.Fight;
    using System.Collections.Generic;

    public class PrismFightersInformation : NetworkType
    {
        
        public const short ProtocolId = 443;
        
        public override short TypeID
        {
            get
            {
                return ProtocolId;
            }
        }
        
        private ProtectedEntityWaitingForHelpInfo m_waitingForHelpInfo;
        
        public virtual ProtectedEntityWaitingForHelpInfo WaitingForHelpInfo
        {
            get
            {
                return m_waitingForHelpInfo;
            }
            set
            {
                m_waitingForHelpInfo = value;
            }
        }
        
        private List<CharacterMinimalPlusLookInformations> m_allyCharactersInformations;
        
        public virtual List<CharacterMinimalPlusLookInformations> AllyCharactersInformations
        {
            get
            {
                return m_allyCharactersInformations;
            }
            set
            {
                m_allyCharactersInformations = value;
            }
        }
        
        private List<CharacterMinimalPlusLookInformations> m_enemyCharactersInformations;
        
        public virtual List<CharacterMinimalPlusLookInformations> EnemyCharactersInformations
        {
            get
            {
                return m_enemyCharactersInformations;
            }
            set
            {
                m_enemyCharactersInformations = value;
            }
        }
        
        private ushort m_subAreaId;
        
        public virtual ushort SubAreaId
        {
            get
            {
                return m_subAreaId;
            }
            set
            {
                m_subAreaId = value;
            }
        }
        
        public PrismFightersInformation(ProtectedEntityWaitingForHelpInfo waitingForHelpInfo, List<CharacterMinimalPlusLookInformations> allyCharactersInformations, List<CharacterMinimalPlusLookInformations> enemyCharactersInformations, ushort subAreaId)
        {
            m_waitingForHelpInfo = waitingForHelpInfo;
            m_allyCharactersInformations = allyCharactersInformations;
            m_enemyCharactersInformations = enemyCharactersInformations;
            m_subAreaId = subAreaId;
        }
        
        public PrismFightersInformation()
        {
        }
        
        public override void Serialize(ICustomDataOutput writer)
        {
            m_waitingForHelpInfo.Serialize(writer);
            writer.WriteShort(((short)(m_allyCharactersInformations.Count)));
            int allyCharactersInformationsIndex;
            for (allyCharactersInformationsIndex = 0; (allyCharactersInformationsIndex < m_allyCharactersInformations.Count); allyCharactersInformationsIndex = (allyCharactersInformationsIndex + 1))
            {
                CharacterMinimalPlusLookInformations objectToSend = m_allyCharactersInformations[allyCharactersInformationsIndex];
                writer.WriteUShort(((ushort)(objectToSend.TypeID)));
                objectToSend.Serialize(writer);
            }
            writer.WriteShort(((short)(m_enemyCharactersInformations.Count)));
            int enemyCharactersInformationsIndex;
            for (enemyCharactersInformationsIndex = 0; (enemyCharactersInformationsIndex < m_enemyCharactersInformations.Count); enemyCharactersInformationsIndex = (enemyCharactersInformationsIndex + 1))
            {
                CharacterMinimalPlusLookInformations objectToSend = m_enemyCharactersInformations[enemyCharactersInformationsIndex];
                writer.WriteUShort(((ushort)(objectToSend.TypeID)));
                objectToSend.Serialize(writer);
            }
            writer.WriteVarUhShort(m_subAreaId);
        }
        
        public override void Deserialize(ICustomDataInput reader)
        {
            m_waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
            m_waitingForHelpInfo.Deserialize(reader);
            int allyCharactersInformationsCount = reader.ReadUShort();
            int allyCharactersInformationsIndex;
            m_allyCharactersInformations = new System.Collections.Generic.List<CharacterMinimalPlusLookInformations>();
            for (allyCharactersInformationsIndex = 0; (allyCharactersInformationsIndex < allyCharactersInformationsCount); allyCharactersInformationsIndex = (allyCharactersInformationsIndex + 1))
            {
                CharacterMinimalPlusLookInformations objectToAdd = ProtocolTypeManager.GetInstance<CharacterMinimalPlusLookInformations>((short)reader.ReadUShort());
                objectToAdd.Deserialize(reader);
                m_allyCharactersInformations.Add(objectToAdd);
            }
            int enemyCharactersInformationsCount = reader.ReadUShort();
            int enemyCharactersInformationsIndex;
            m_enemyCharactersInformations = new System.Collections.Generic.List<CharacterMinimalPlusLookInformations>();
            for (enemyCharactersInformationsIndex = 0; (enemyCharactersInformationsIndex < enemyCharactersInformationsCount); enemyCharactersInformationsIndex = (enemyCharactersInformationsIndex + 1))
            {
                CharacterMinimalPlusLookInformations objectToAdd = ProtocolTypeManager.GetInstance<CharacterMinimalPlusLookInformations>((short)reader.ReadUShort());
                objectToAdd.Deserialize(reader);
                m_enemyCharactersInformations.Add(objectToAdd);
            }
            m_subAreaId = reader.ReadVarUhShort();
        }
    }
}
