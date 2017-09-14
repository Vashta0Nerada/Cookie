using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookie.API.Core;
using MoonSharp.Interpreter;
using Cookie.API.Commands;
using Cookie.API.Utils;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class BaseLUA
    {
        protected DataLUA _data;
        public BaseLUA(DataLUA data)
        {
            _data = data;
        }
    }
}
