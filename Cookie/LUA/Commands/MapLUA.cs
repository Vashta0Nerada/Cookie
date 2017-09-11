using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookie.API.Game.Map;
using System.Threading;
using Cookie.API.Utils;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class MapLUA : BaseLUA
    {
        private AutoResetEvent _messageReceived;
        private bool GoToCell_Result;

        public MapLUA(DataLUA data) : base(data)
        {
            this._messageReceived = new AutoResetEvent(false);
        }

        public int MapID
        {
            get { return Data.Account.Character.Map.Id; }
        }

        public bool GoToCell(int cell)
        {
            if (cell <= 0 || cell >= 560) return false;
            GoToCell_Result = false;
            var movement = Data.Account.Character.Map.MoveToCell(cell);
            movement.MovementFinished += GoToCell_OnMovementFinished;
            movement.PerformMovement();
            
            this._messageReceived.WaitOne(10000);

            return GoToCell_Result;
        }

        private void GoToCell_OnMovementFinished(object sender, CellMovementEventArgs e)
        {
            GoToCell_Result = e.Sucess;

            this._messageReceived.Set();
        }

        public void ChangeMap(string direction, int cellID = -1)
        {
            IMapChangement move = null;
            switch (direction)
            {
                case "top":
                case "up":
                    move = Data.Account.Character.Map.ChangeMap(MapDirectionEnum.North, cellID);
                    break;
                case "left":
                    move = Data.Account.Character.Map.ChangeMap(MapDirectionEnum.West, cellID);
                    break;
                case "right":
                    move = Data.Account.Character.Map.ChangeMap(MapDirectionEnum.East, cellID);
                    break;
                case "bottom":
                case "down":
                    move = Data.Account.Character.Map.ChangeMap(MapDirectionEnum.South, cellID);
                    break;
            }

            if (move == null) return;
            move.PerformChangement();
            Data.Account.Character.ScriptManager.WaitingForMapChange = true;
        }
    }
}
