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

        private string _scriptPath;
        private Script _script;
        private DataLUA _data;
        private int _bankPercent { get { return _script.Globals.Get("BANK_PERCENT").IsNil() ? 0 : (int)_script.Globals.Get("BANK_PERCENT").Number; } }

        private MapLUA _map;
        private LoggerLUA _logger;
        private InventoryLUA _inventory;
        private System.Timers.Timer _processTimer;
        private Table _currentMapTable;

        public bool ScriptLoaded;
        public bool WaitingForMapChange { get; set; }
        public bool GoingToBank { get; set; }

        public ScriptManager(IAccount account, ICharacter character)
        {
            ScriptLoaded = false;
            WaitingForMapChange = false;
            GoingToBank = false;
            _data = new DataLUA(account);

            _map = new MapLUA(_data);
            _logger = new LoggerLUA(_data);
            _inventory = new InventoryLUA(_data);

            _script = new Script();
            _script.Globals["Map"] = _map;
            _script.Globals["Logger"] = _logger;
            _script.Globals["Inventory"] = _inventory;

            character.Map.MapChanged += MapChanged;
            character.GatherManager.GatherFinished += GatherFinished;
            _processTimer = new System.Timers.Timer(waitTime);
            _processTimer.Elapsed += processMove;
        }

        public void LoadScript(string str)
        {
            _scriptPath = str;
            LoadScriptInternal();
            ScriptLoaded = true;
        }

        private void LoadScriptInternal()
        {
            try
            {
                _script.DoFile(_scriptPath);
            }
            catch (SyntaxErrorException ex)
            {
                Logger.Default.Log("Erreur dans le script : \n" + ex.DecoratedMessage, API.Utils.Enums.LogMessageType.Error);
            }
            catch (Exception ex)
            {
                Logger.Default.Log("Erreur lors du chargement du script : \n" + ex.Message, API.Utils.Enums.LogMessageType.Error);
            }

            if (_script.Globals.Get("BANK_PERCENT").IsNil())
            {
                Logger.Default.Log("Aucun paramètre trouvé pour BANK_PERCENT, retour en banque désactivé.");
            }
        }

        private void GatherFinished(object sender, EventArgs e)
        {
            if (!ScriptLoaded)
                return;

            Logger.Default.Log("Récolte sur la carte terminée.", API.Utils.Enums.LogMessageType.Info);
            _data.Account.PerformAction(ProcessDoor, 2000);
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

            if (!GoingToBank && _bankPercent != 0 && _data.Account.Character.WeightPercentage >= _bankPercent)
            {
                GoingToBank = true;
                Logger.Default.Log("Inventaire rempli à " + _data.Account.Character.WeightPercentage.ToString() + "%, retour en banque.", API.Utils.Enums.LogMessageType.Info);
            }

            if (GoingToBank && (_bankPercent == 0 || _data.Account.Character.WeightPercentage < _bankPercent))
            {
                GoingToBank = false;
                Logger.Default.Log("Fin du retour en banque.", API.Utils.Enums.LogMessageType.Info);
            }

            DynValue val;
            if (GoingToBank)
            {
                if (_script.Globals.Get("bank").IsNil())
                {
                    Logger.Default.Log("Aucune fonction bank trouvée.", API.Utils.Enums.LogMessageType.Error);
                    return;
                }

                val = _script.Call(_script.Globals["bank"]);
            }
            else
            {
                if (_script.Globals.Get("move").IsNil())
                {
                    Logger.Default.Log("Aucune fonction move trouvée.", API.Utils.Enums.LogMessageType.Error);
                    return;
                }

                val = _script.Call(_script.Globals["move"]);
            }

            
            Table matching = null;
            Logger.Default.Log(_data.Account.Character.Map.Id.ToString());
            foreach (var item in val.Table.Values)
            {
                if (item.Table.Get("map").String == _data.Account.Character.Map.Id.ToString() || Regex.IsMatch(item.Table.Get("map").String, "^ ?" + _data.Account.Character.Map.X.ToString() + " ?, ?" + _data.Account.Character.Map.Y.ToString() + " ?$"))
                {
                    matching = item.Table;
                    break;
                }
            }

            if (matching == null)
            {
                Logger.Default.Log("Carte non gérée par le script.", API.Utils.Enums.LogMessageType.Error);
                return;
            }

            _currentMapTable = matching;

            _data.Account.PerformAction(ProcessCustom, 10);
        }

        private void ProcessCustom()
        {
            if (_currentMapTable.Get("custom").IsNil())
            {
                ProcessFight();
                return;
            }

            if(_currentMapTable.Get("custom").Type != DataType.Function)
            {
                Logger.Default.Log("Une erreur est présente dans votre script, fonction custom mal configurée.");
                ProcessFight();
                return;
            }

            _currentMapTable.Get("custom").Function.Call();
            if (!WaitingForMapChange)
            {
                ProcessFight();
            }
        }

        private void ProcessFight()
        {
            if (GoingToBank || _currentMapTable.Get("fight").IsNil() || !_currentMapTable.Get("fight").Boolean)
            {
                ProcessGather();
                return;
            }

            Logger.Default.Log("Fight not implemented yet.");
            ProcessGather();
        }

        private void ProcessGather()
        {
            if(GoingToBank || _currentMapTable.Get("gather").IsNil() || !_currentMapTable.Get("gather").Boolean)
            {
                ProcessDoor();
                return;
            }

            if(_script.Globals.Get("GATHER").IsNil())
            {
                Logger.Default.Log("Empty GATHER is not implemented yet.");
                ProcessDoor();
                return;
            }
            List<int> list = new List<int>();
            foreach (var ressource in _script.Globals.Get("GATHER").Table.Values)
            {
                list.Add(Convert.ToInt32(ressource.Number));
            }

            Logger.Default.Log("Démarrage de la récolte des éléments suivants : " + string.Join(", ", list) + ".");
            _data.Account.Character.GatherManager.Gather(list, false, true);
        }

        private void ProcessDoor()
        {
            if (_currentMapTable.Get("door").IsNilOrNan())
            {
                ProcessPath();
                return;
            }

            Logger.Default.Log("Door is not implemented yet.");
            var door = _data.Account.Character.Map.Doors[(int)_currentMapTable.Get("door").Number];
            _data.Account.Character.Map.UseElement((int) door.Id, door.EnabledSkills[0].SkillInstanceUid);
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
                _map.ChangeMap(dir, cell);
                return;
            }
            else
            {
                int cell = Convert.ToInt32(dir);
                if (cell > 0 && cell < 560)
                {
                    var movement = _data.Account.Character.Map.MoveToCell(cell);
                    Logger.Default.Log("Déplacement vers la cellid " + cell.ToString() + ".");
                    movement.PerformMovement();
                    return;
                }
            }

            Logger.Default.Log("Aucune action réalisée");
        }

        public void TerminateScript()
        {
            Logger.Default.Log("Script arrêté");
            throw new NotImplementedException();
        }
    }
}
