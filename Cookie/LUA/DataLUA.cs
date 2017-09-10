using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookie.API.Core;
using Cookie.API.Core.Frames;
using Cookie.API.Core.Network;
using Cookie.API.Messages;
using Cookie.API.Network;
using Cookie.API.Plugins;
using Cookie.API.Utils;
using Cookie.Core.Frames;

namespace Cookie.LUA
{
    class DataLUA
    {
        public IAccount Account;

        public DataLUA(IAccount account)
        {
            Account = account;
        }
    }
}
