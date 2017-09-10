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

        private ScriptLoader loader;

        public void OnCommand(IAccount account, string[] args)
        {
            loader = new ScriptLoader(account);
            loader.LoadScript(args[0]);
            Logger.Default.Log($"Vous êtes niveau {account.Character.Level}.");
        }
    }
}
