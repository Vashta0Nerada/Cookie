using Cookie.LUA.Objects;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class InventoryLUA : BaseLUA
    {
        public long Pods => _data.Account.Character.Weight;
        public long PodsPercent => _data.Account.Character.WeightPercentage;
        public long MaxPods => _data.Account.Character.MaxWeight;

        public List<ItemLUA> GetItems()
        {
            var list = new List<ItemLUA>();
            foreach (var item in _data.Account.Character.Inventory.Objects)
            {
                list.Add(new ItemLUA(item));
            }
            return list;
        }

        public InventoryLUA(DataLUA data) : base(data)
        {
        }

        public double ItemCount(long itemId)
        {
            long count = 0;
            foreach (var item in _data.Account.Character.Inventory.Objects)
            {
                if (item.ObjectGID == itemId)
                    count += item.Quantity;
            }

            return count;
        }

        public void DeleteItem(int itemid)
        {
            throw new NotImplementedException();
        }
    }
}
