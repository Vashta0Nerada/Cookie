//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cookie.Protocol.Network.Messages.Game.Context.Fight
{
    using Cookie.Protocol.Network.Types.Game.Context.Fight;
    using Cookie.Protocol.Network.Types.Game.Context.Roleplay.Party;
    using Cookie.IO;
    using System.Collections.Generic;


    public class GameFightEndMessage : NetworkMessage
    {
        
        public const uint ProtocolId = 720;
        
        public override uint MessageID
        {
            get
            {
                return ProtocolId;
            }
        }
        
        private List<FightResultListEntry> m_results;
        
        public virtual List<FightResultListEntry> Results
        {
            get
            {
                return m_results;
            }
            set
            {
                m_results = value;
            }
        }
        
        private List<NamedPartyTeamWithOutcome> m_namedPartyTeamsOutcomes;
        
        public virtual List<NamedPartyTeamWithOutcome> NamedPartyTeamsOutcomes
        {
            get
            {
                return m_namedPartyTeamsOutcomes;
            }
            set
            {
                m_namedPartyTeamsOutcomes = value;
            }
        }
        
        private int m_duration;
        
        public virtual int Duration
        {
            get
            {
                return m_duration;
            }
            set
            {
                m_duration = value;
            }
        }
        
        private short m_ageBonus;
        
        public virtual short AgeBonus
        {
            get
            {
                return m_ageBonus;
            }
            set
            {
                m_ageBonus = value;
            }
        }
        
        private short m_lootShareLimitMalus;
        
        public virtual short LootShareLimitMalus
        {
            get
            {
                return m_lootShareLimitMalus;
            }
            set
            {
                m_lootShareLimitMalus = value;
            }
        }
        
        public GameFightEndMessage(List<FightResultListEntry> results, List<NamedPartyTeamWithOutcome> namedPartyTeamsOutcomes, int duration, short ageBonus, short lootShareLimitMalus)
        {
            m_results = results;
            m_namedPartyTeamsOutcomes = namedPartyTeamsOutcomes;
            m_duration = duration;
            m_ageBonus = ageBonus;
            m_lootShareLimitMalus = lootShareLimitMalus;
        }
        
        public GameFightEndMessage()
        {
        }
        
        public override void Serialize(ICustomDataOutput writer)
        {
            writer.WriteInt(m_duration);
            writer.WriteShort(m_ageBonus);
            writer.WriteShort(m_lootShareLimitMalus);
            writer.WriteShort(((short)(m_results.Count)));
            int resultsIndex;
            for (resultsIndex = 0; (resultsIndex < m_results.Count); resultsIndex = (resultsIndex + 1))
            {
                FightResultListEntry objectToSend = m_results[resultsIndex];
                writer.WriteUShort(((ushort)(objectToSend.TypeID)));
                objectToSend.Serialize(writer);
            }
            writer.WriteShort(((short)(m_namedPartyTeamsOutcomes.Count)));
            int namedPartyTeamsOutcomesIndex;
            for (namedPartyTeamsOutcomesIndex = 0; (namedPartyTeamsOutcomesIndex < m_namedPartyTeamsOutcomes.Count); namedPartyTeamsOutcomesIndex = (namedPartyTeamsOutcomesIndex + 1))
            {
                NamedPartyTeamWithOutcome objectToSend = m_namedPartyTeamsOutcomes[namedPartyTeamsOutcomesIndex];
                objectToSend.Serialize(writer);
            }
        }
        
        public override void Deserialize(ICustomDataInput reader)
        {
            m_duration = reader.ReadInt();
            m_ageBonus = reader.ReadShort();
            m_lootShareLimitMalus = reader.ReadShort();
            int resultsCount = reader.ReadUShort();
            int resultsIndex;
            m_results = new System.Collections.Generic.List<FightResultListEntry>();
            for (resultsIndex = 0; (resultsIndex < resultsCount); resultsIndex = (resultsIndex + 1))
            {
                FightResultListEntry objectToAdd = ProtocolTypeManager.GetInstance<FightResultListEntry>((short)reader.ReadUShort());
                objectToAdd.Deserialize(reader);
                m_results.Add(objectToAdd);
            }
            int namedPartyTeamsOutcomesCount = reader.ReadUShort();
            int namedPartyTeamsOutcomesIndex;
            m_namedPartyTeamsOutcomes = new System.Collections.Generic.List<NamedPartyTeamWithOutcome>();
            for (namedPartyTeamsOutcomesIndex = 0; (namedPartyTeamsOutcomesIndex < namedPartyTeamsOutcomesCount); namedPartyTeamsOutcomesIndex = (namedPartyTeamsOutcomesIndex + 1))
            {
                NamedPartyTeamWithOutcome objectToAdd = new NamedPartyTeamWithOutcome();
                objectToAdd.Deserialize(reader);
                m_namedPartyTeamsOutcomes.Add(objectToAdd);
            }
        }
    }
}