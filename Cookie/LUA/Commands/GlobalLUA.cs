using Cookie.API.Protocol.Network.Messages.Game.Dialog;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class GlobalLUA : BaseLUA
    {
        public GlobalLUA(DataLUA data) : base(data)
        {
        }

        public void LeaveDialog()
        {
            var message = new LeaveDialogRequestMessage();
            _data.Account.Network.SendToServer(message);
        }

        public async void Sleep(int ms)
        {
            await Task.Delay(ms);
        }
    }
}
