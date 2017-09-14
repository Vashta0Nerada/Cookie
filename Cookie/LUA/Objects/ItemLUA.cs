using Cookie.API.Gamedata.D2i;
using Cookie.API.Gamedata.D2o;
using Cookie.API.Protocol.Network.Types.Game.Data.Items;
using MoonSharp.Interpreter;

using Item = Cookie.API.Datacenter.Item;

namespace Cookie.LUA.Objects
{
    [MoonSharpUserData]
    class ItemLUA
    {
        private ObjectItem _item;
        private Item _itemData;

        public long ID => _item.ObjectGID;
        public long UID => _item.ObjectUID;
        public long Quantity => _item.Quantity;
        public long Pos => _item.Position;
        public long Weight => _itemData.Weight;
        public string Name => FastD2IReader.Instance.GetText(_itemData.NameId);
        public string Description => FastD2IReader.Instance.GetText(_itemData.DescriptionId);
        public long Level => _itemData.Level;
        

        public ItemLUA(ObjectItem item)
        {
            _item = item;
            _itemData = ObjectDataManager.Instance.Get<Item>(item.ObjectGID);
        }
    }
}
