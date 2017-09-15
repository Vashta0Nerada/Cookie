using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cookie.API.Core
{
    public interface IScriptManager
    {
        bool WaitingForMapChange { get; set; }

        /// <summary>
        ///     This method loads the specified script
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        void LoadScript(string filePath);
        void ProcessMove(object sender = null, ElapsedEventArgs e = null);
    }
}
