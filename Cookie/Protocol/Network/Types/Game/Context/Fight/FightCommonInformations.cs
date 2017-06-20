//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Cookie.Network;

namespace Cookie.Protocol.Network.Types.Game.Context.Fight
{
    using Cookie.IO;
    using System.Collections.Generic;


    public class FightCommonInformations : NetworkType
    {
        
        public const short ProtocolId = 43;
        
        public override short TypeID
        {
            get
            {
                return ProtocolId;
            }
        }
        
        private List<FightTeamInformations> m_fightTeams;
        
        public virtual List<FightTeamInformations> FightTeams
        {
            get
            {
                return m_fightTeams;
            }
            set
            {
                m_fightTeams = value;
            }
        }
        
        private List<System.UInt16> m_fightTeamsPositions;
        
        public virtual List<System.UInt16> FightTeamsPositions
        {
            get
            {
                return m_fightTeamsPositions;
            }
            set
            {
                m_fightTeamsPositions = value;
            }
        }
        
        private List<FightOptionsInformations> m_fightTeamsOptions;
        
        public virtual List<FightOptionsInformations> FightTeamsOptions
        {
            get
            {
                return m_fightTeamsOptions;
            }
            set
            {
                m_fightTeamsOptions = value;
            }
        }
        
        private int m_fightId;
        
        public virtual int FightId
        {
            get
            {
                return m_fightId;
            }
            set
            {
                m_fightId = value;
            }
        }
        
        private byte m_fightType;
        
        public virtual byte FightType
        {
            get
            {
                return m_fightType;
            }
            set
            {
                m_fightType = value;
            }
        }
        
        public FightCommonInformations(List<FightTeamInformations> fightTeams, List<System.UInt16> fightTeamsPositions, List<FightOptionsInformations> fightTeamsOptions, int fightId, byte fightType)
        {
            m_fightTeams = fightTeams;
            m_fightTeamsPositions = fightTeamsPositions;
            m_fightTeamsOptions = fightTeamsOptions;
            m_fightId = fightId;
            m_fightType = fightType;
        }
        
        public FightCommonInformations()
        {
        }
        
        public override void Serialize(ICustomDataOutput writer)
        {
            writer.WriteInt(m_fightId);
            writer.WriteByte(m_fightType);
            writer.WriteShort(((short)(m_fightTeams.Count)));
            int fightTeamsIndex;
            for (fightTeamsIndex = 0; (fightTeamsIndex < m_fightTeams.Count); fightTeamsIndex = (fightTeamsIndex + 1))
            {
                FightTeamInformations objectToSend = m_fightTeams[fightTeamsIndex];
                writer.WriteUShort(((ushort)(objectToSend.TypeID)));
                objectToSend.Serialize(writer);
            }
            writer.WriteShort(((short)(m_fightTeamsPositions.Count)));
            int fightTeamsPositionsIndex;
            for (fightTeamsPositionsIndex = 0; (fightTeamsPositionsIndex < m_fightTeamsPositions.Count); fightTeamsPositionsIndex = (fightTeamsPositionsIndex + 1))
            {
                writer.WriteVarUhShort(m_fightTeamsPositions[fightTeamsPositionsIndex]);
            }
            writer.WriteShort(((short)(m_fightTeamsOptions.Count)));
            int fightTeamsOptionsIndex;
            for (fightTeamsOptionsIndex = 0; (fightTeamsOptionsIndex < m_fightTeamsOptions.Count); fightTeamsOptionsIndex = (fightTeamsOptionsIndex + 1))
            {
                FightOptionsInformations objectToSend = m_fightTeamsOptions[fightTeamsOptionsIndex];
                objectToSend.Serialize(writer);
            }
        }
        
        public override void Deserialize(ICustomDataInput reader)
        {
            m_fightId = reader.ReadInt();
            m_fightType = reader.ReadByte();
            int fightTeamsCount = reader.ReadUShort();
            int fightTeamsIndex;
            m_fightTeams = new System.Collections.Generic.List<FightTeamInformations>();
            for (fightTeamsIndex = 0; (fightTeamsIndex < fightTeamsCount); fightTeamsIndex = (fightTeamsIndex + 1))
            {
                FightTeamInformations objectToAdd = ProtocolTypeManager.GetInstance<FightTeamInformations>((short)reader.ReadUShort());
                objectToAdd.Deserialize(reader);
                m_fightTeams.Add(objectToAdd);
            }
            int fightTeamsPositionsCount = reader.ReadUShort();
            int fightTeamsPositionsIndex;
            m_fightTeamsPositions = new System.Collections.Generic.List<ushort>();
            for (fightTeamsPositionsIndex = 0; (fightTeamsPositionsIndex < fightTeamsPositionsCount); fightTeamsPositionsIndex = (fightTeamsPositionsIndex + 1))
            {
                m_fightTeamsPositions.Add(reader.ReadVarUhShort());
            }
            int fightTeamsOptionsCount = reader.ReadUShort();
            int fightTeamsOptionsIndex;
            m_fightTeamsOptions = new System.Collections.Generic.List<FightOptionsInformations>();
            for (fightTeamsOptionsIndex = 0; (fightTeamsOptionsIndex < fightTeamsOptionsCount); fightTeamsOptionsIndex = (fightTeamsOptionsIndex + 1))
            {
                FightOptionsInformations objectToAdd = new FightOptionsInformations();
                objectToAdd.Deserialize(reader);
                m_fightTeamsOptions.Add(objectToAdd);
            }
        }
    }
}
