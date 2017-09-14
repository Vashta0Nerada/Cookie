using Cookie.API.Gamedata.D2i;
using Cookie.API.Gamedata.D2o;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.LUA.Objects
{
    [MoonSharpUserData]
    class NpcReplyLUA
    {
        public int Id;
        public string Message => FastD2IReader.Instance.GetText(ObjectDataManager.Instance.Get<API.Datacenter.NpcMessage>(Id)
             .MessageId);

        public NpcReplyLUA(int id)
        {
            Id = id;
        }
    }
}
