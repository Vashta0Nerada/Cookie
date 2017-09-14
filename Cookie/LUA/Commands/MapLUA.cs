﻿using MoonSharp.Interpreter;
using Cookie.API.Game.Map;
using System.Threading;

namespace Cookie.LUA.Commands
{
    [MoonSharpUserData]
    class MapLUA : BaseLUA
    {
        private AutoResetEvent _messageReceived;
        private bool GoToCell_Result;

        public MapLUA(DataLUA data) : base(data)
        {
            _messageReceived = new AutoResetEvent(false);
        }

        public int Id => _data.Account.Character.Map.Id;
        public int X => _data.Account.Character.Map.X;
        public int Y => _data.Account.Character.Map.Y;
        public int WorldId => _data.Account.Character.Map.WorldId;
        public int SubAreaId => _data.Account.Character.Map.SubAreaId;

        public bool GoToCell(int cell)
        {
            if (cell <= 0 || cell >= 560) return false;
            GoToCell_Result = false;
            var movement = _data.Account.Character.Map.MoveToCell(cell);
            movement.MovementFinished += GoToCell_OnMovementFinished;
            movement.PerformMovement();
            
            _messageReceived.WaitOne(10000);

            return GoToCell_Result;
        }

        private void GoToCell_OnMovementFinished(object sender, CellMovementEventArgs e)
        {
            GoToCell_Result = e.Sucess;

            _messageReceived.Set();
        }

        public void ChangeMap(string direction, int cellId = -1)
        {
            IMapChangement move = null;
            switch (direction)
            {
                case "top":
                case "up":
                    move = _data.Account.Character.Map.ChangeMap(MapDirectionEnum.North, cellId);
                    break;
                case "left":
                    move = _data.Account.Character.Map.ChangeMap(MapDirectionEnum.West, cellId);
                    break;
                case "right":
                    move = _data.Account.Character.Map.ChangeMap(MapDirectionEnum.East, cellId);
                    break;
                case "bottom":
                case "down":
                    move = _data.Account.Character.Map.ChangeMap(MapDirectionEnum.South, cellId);
                    break;
            }

            if (move == null) return;
            move.PerformChangement();
            _data.Account.Character.ScriptManager.WaitingForMapChange = true;
        }
    }
}
