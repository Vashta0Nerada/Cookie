using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Cookie.API.Commands;
using Cookie.API.Core;
using Cookie.LUA.Commands;
using Cookie.API.Utils;
using System.Threading;

namespace Cookie.LUA
{
    class ScriptLoader
    {
        private string scriptPath;
        private Script script;
        private DataLUA data;

        private MapLUA map;
        private LoggerLUA logger;

        public ScriptLoader(IAccount account)
        {
            data = new DataLUA(account);

            map = new MapLUA(data);
            logger = new LoggerLUA(data);

            script = new Script();
            script.Globals["Map"] = map;
            script.Globals["Logger"] = logger;
        }

        public void LoadScript(string str)
        {
            scriptPath = str;
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            Thread oThread = new Thread(new ThreadStart(this.loadScriptInternal));

            // Start the thread
            oThread.Start();
        }

        public void loadScriptInternal()
        {
            try
            {
                script.DoFile(scriptPath);
            }
            catch (SyntaxErrorException ex)
            {
                Logger.Default.Log("Erreur dans le script : \n" + ex.DecoratedMessage);
            }
            catch (Exception ex)
            {
                Logger.Default.Log("Erreur lors du chargement du script : \n" + ex.Message);
            }
        }
    }
}
