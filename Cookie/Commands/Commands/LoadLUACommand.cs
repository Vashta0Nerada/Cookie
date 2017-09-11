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

        private ScriptManager loader;

        public void OnCommand(IAccount account, string[] args)
        {
            if(loader == null)
                loader = new ScriptManager(account);
            loader.LoadScript(args[0]);
        }
    }
}
