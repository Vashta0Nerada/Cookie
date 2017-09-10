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
            var movement = Data.Account.Character.Map.MoveToCell(cell);
            movement.MovementFinished += OnMovementFinished;
            movement.PerformMovement();
            
            this._messageReceived.WaitOne(5000);

            return true;
        }

        private void OnMovementFinished(object sender, CellMovementEventArgs e)
        {
            switch (e.Sucess)
            {
                case true:
                    Logger.Default.Log($"Déplacement réussi ! Cell d'arrivé: {e.EndCell}");
                    break;
                case false:
                    Logger.Default.Log($"Echec du déplacement :'( StartCell: {e.StartCell} -> EndCell: {e.EndCell}");
                    break;
            }

            this._messageReceived.Set();
        }
    }
}
