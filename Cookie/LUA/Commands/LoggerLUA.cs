using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookie.API.Commands;
using Cookie.API.Core;
using Cookie.API.Utils;
using MoonSharp.Interpreter;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class LoggerLUA : BaseLUA
    {
        public LoggerLUA(DataLUA data) : base(data)
        {
        }

        public void LogMessage(string str)
        {
            Logger.Default.Log(str);
        }
    }
}
