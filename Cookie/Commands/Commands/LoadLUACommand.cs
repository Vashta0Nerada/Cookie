using Cookie.API.Commands;
using Cookie.API.Core;
using Cookie.API.Utils;
using Cookie.LUA;

namespace Cookie.Commands.Commands
{
    class LoadLUACommand : ICommand
    {
        public string CommandName => "loadlua";
        public string ArgsName => "string [path]";

        public void OnCommand(IAccount account, string[] args)
        {
            account.Character.ScriptManager.LoadScript(args[0]);
        }
    }
}
