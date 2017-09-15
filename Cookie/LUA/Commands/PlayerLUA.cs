using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.LUA.Commands
{
    class PlayerLUA : BaseLUA
    {
        public ulong Kamas => _data.Account.Character.Stats.Kamas;
        public int Level => _data.Account.Character.Level;
        public uint Life => _data.Account.Character.Stats.LifePoints;
        public uint MaxLife => _data.Account.Character.Stats.MaxLifePoints;
        public uint Energy => _data.Account.Character.Stats.EnergyPoints;
        public uint MaxEnergy => _data.Account.Character.Stats.MaxEnergyPoints;

        public PlayerLUA(DataLUA data) : base(data)
        {
        }
    }
}
