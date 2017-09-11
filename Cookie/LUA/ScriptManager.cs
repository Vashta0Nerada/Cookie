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
using Cookie.API.Game.Map;
using System.Text.RegularExpressions;
using System.Timers;

namespace Cookie.LUA
{
    class ScriptManager
    {
        public int waitTime = 500;

        private string scriptPath;
        private Script script;
        private DataLUA data;

        private MapLUA map;
        private LoggerLUA logger;
        private System.Timers.Timer _processTimer;

        public ScriptManager(IAccount account)
        {
            data = new DataLUA(account);

            map = new MapLUA(data);
            logger = new LoggerLUA(data);

            script = new Script();
            script.Globals["Map"] = map;
            script.Globals["Logger"] = logger;

            account.Character.Map.MapChanged += MapChanged;
            _processTimer = new System.Timers.Timer(waitTime);
            _processTimer.Elapsed += processMove;
        }

        private void MapChanged(object sender, MapChangedEventArgs e)
        {
            Console.WriteLine("Trying to get move");
            _processTimer.Start();
        }

        private void processMove(object sender, ElapsedEventArgs e)
        {
            _processTimer.Stop();

            DynValue val = script.Call(script.Globals["move"]);
            Table matching = null;
            Logger.Default.Log(data.Account.Character.Map.Id.ToString());
            foreach (var item in val.Table.Values)
            {
                if(item.Table.Get("map").String == data.Account.Character.Map.Id.ToString() || Regex.IsMatch(item.Table.Get("map").String, "^ ?" + data.Account.Character.Map.X.ToString() + " ?, ?" + data.Account.Character.Map.Y.ToString()))
                {
                    matching = item.Table;
                    break;
                }
            }

            if(matching == null)
            {
                Logger.Default.Log("Carte non gérée par le script.");
                return;
            }

            string path = matching.Get("path").String;
            string[] paths = path.Split('|');
            path = paths[Randomize.GetRandomNumber(0, paths.Length)];

            var dirs = path.Split('(');
            var dir = dirs[0];

            string[] acceptedDir = { "top", "up", "right", "left", "bottom", "down" };

            if(acceptedDir.Contains(dir))
            {
                int cell = -1;
                if (dirs.Length >= 2)
                {
                    cell = Convert.ToInt32(dirs[1].Replace(")", ""));
                    if (cell <= 0 || cell >= 560)
                        cell = -1;
                }

                Logger.Default.Log("Déplacement dans la direction " + dir + (cell == -1 ? "" : (" en empruntant la cellid " + cell.ToString())) + ".");
                map.ChangeMap(dir, cell);
                return;
            }
            else 
            {
                int cell = Convert.ToInt32(dir);
                if (cell > 0 && cell < 560)
                {
                    var movement = data.Account.Character.Map.MoveToCell(cell);
                    Logger.Default.Log("Déplacement vers la cellid " + cell.ToString() + ".");
                    movement.PerformMovement();
                    return;
                }
            }

            Logger.Default.Log("Aucune action réalisée");
        }

        public void LoadScript(string str)
        {
            scriptPath = str;
            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            //Thread oThread = new Thread(new ThreadStart(this.loadScriptInternal));

            // Start the thread
            //oThread.Start();
            loadScriptInternal();
        }

        private void loadScriptInternal()
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

        public void TerminateScript()
        {
            Logger.Default.Log("Script arrêté");
            throw new NotImplementedException();
        }
    }
}
