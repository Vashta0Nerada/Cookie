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
    class ScriptManager : IScriptManager
    {
        public int waitTime = 500;

        private string scriptPath;
        private Script script;
        private DataLUA data;

        private MapLUA map;
        private LoggerLUA logger;
        private System.Timers.Timer _processTimer;
        private Table _currentMapTable;

        public bool ScriptLoaded;
        public bool WaitingForMapChange { get; set; }

        public ScriptManager(IAccount account, ICharacter character)
        {
            ScriptLoaded = false;
            WaitingForMapChange = false;
            data = new DataLUA(account);

            map = new MapLUA(data);
            logger = new LoggerLUA(data);

            script = new Script();
            script.Globals["Map"] = map;
            script.Globals["Logger"] = logger;

            character.Map.MapChanged += MapChanged;
            character.GatherManager.GatherFinished += GatherFinished;
            _processTimer = new System.Timers.Timer(waitTime);
            _processTimer.Elapsed += processMove;
        }

        private void GatherFinished(object sender, EventArgs e)
        {
            if (!ScriptLoaded)
                return;

            Logger.Default.Log("Récolte sur la carte terminée.");
            ProcessDoor();
        }

        private void MapChanged(object sender, MapChangedEventArgs e)
        {
            if (!ScriptLoaded)
                return;

            Console.WriteLine("Trying to get move");
            _processTimer.Start();
        }

        private void processMove(object sender, ElapsedEventArgs e)
        {
            _processTimer.Stop();
            _currentMapTable = null;
            WaitingForMapChange = false;

            DynValue val = script.Call(script.Globals["move"]);
            Table matching = null;
            Logger.Default.Log(data.Account.Character.Map.Id.ToString());
            foreach (var item in val.Table.Values)
            {
                if (item.Table.Get("map").String == data.Account.Character.Map.Id.ToString() || Regex.IsMatch(item.Table.Get("map").String, "^ ?" + data.Account.Character.Map.X.ToString() + " ?, ?" + data.Account.Character.Map.Y.ToString()))
                {
                    matching = item.Table;
                    break;
                }
            }

            if (matching == null)
            {
                Logger.Default.Log("Carte non gérée par le script.");
                return;
            }

            _currentMapTable = matching;
            
            ProcessCustom();
        }

        private void ProcessCustom()
        {
            if (_currentMapTable.Get("custom").IsNil())
            {
                ProcessFight();
                return;
            }

            script.Call(script.Globals[_currentMapTable.Get("custom").String]);
            if (!WaitingForMapChange)
            {
                ProcessFight();
            }
        }

        private void ProcessFight()
        {
            if (_currentMapTable.Get("fight").IsNil() || !_currentMapTable.Get("fight").Boolean)
            {
                ProcessGather();
                return;
            }

            Logger.Default.Log("Fight not implemented yet.");
            ProcessGather();
        }

        private void ProcessGather()
        {
            if(_currentMapTable.Get("gather").IsNil() || !_currentMapTable.Get("gather").Boolean)
            {
                ProcessDoor();
                return;
            }

            if(script.Globals.Get("GATHER").IsNil())
            {
                Logger.Default.Log("Empty GATHER is not implemented yet.");
                ProcessDoor();
                return;
            }
            List<int> list = new List<int>();
            foreach (var ressource in script.Globals.Get("GATHER").Table.Values)
            {
                list.Add(Convert.ToInt32(ressource.Number));
            }

            Logger.Default.Log("Démarrage de la récolte des éléments suivants : " + string.Join(", ", list) + ".");
            data.Account.Character.GatherManager.Gather(list, false, true);
        }

        private void ProcessDoor()
        {
            if (_currentMapTable.Get("door").IsNilOrNan())
            {
                ProcessPath();
                return;
            }

            Logger.Default.Log("Door is not implemented yet.");
            var door = data.Account.Character.Map.Doors[(int)_currentMapTable.Get("door").Number];
            data.Account.Character.Map.UseElement((int) door.Id, door.EnabledSkills[0].SkillInstanceUid);
            ProcessPath();
        }

        private void ProcessPath()
        {
            if (_currentMapTable.Get("path").IsNil())
            {
                Logger.Default.Log("Aucune action à réaliser sur cette carte.");
                return;
            }

            string path = _currentMapTable.Get("path").String;
            string[] paths = path.Split('|');
            path = paths[Randomize.GetRandomNumber(0, paths.Length)];

            var dirs = path.Split('(');
            var dir = dirs[0];

            string[] acceptedDir = { "top", "up", "right", "left", "bottom", "down" };

            if (acceptedDir.Contains(dir))
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
            LoadScriptInternal();
            ScriptLoaded = true;
        }

        private void LoadScriptInternal()
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
