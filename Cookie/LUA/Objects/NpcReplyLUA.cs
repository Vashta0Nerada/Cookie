using Cookie.API.Datacenter;
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
        public string Message
        {
            get
            {
                return FastD2IReader.Instance.GetText(_npc.DialogReplies.Find(rep => rep[0] == Id)[1]);
            }
        }

        private Npc _npc;

        public NpcReplyLUA(int id, Npc npc)
        {
            Id = id;
            _npc = npc;
        }
    }
}
