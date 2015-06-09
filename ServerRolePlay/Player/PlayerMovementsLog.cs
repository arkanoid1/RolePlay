using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRageMath;

namespace ServerRolePlay
{
    class PlayerMovementsLog
    {
        ulong _steamId;
        string _playerName;
        
        Logger Log;

        public PlayerMovementsLog(ulong id, string name)
        {
            _steamId = id;
            _playerName = name;
            Log = LogManager.GetLogger(_steamId.ToString());

        }

        public void LogPosition(Vector3 pos) {

            Log.Info("{0}({1})|{2};{3};{4}", _playerName, _steamId, pos.X, pos.Y, pos.Z);

            /* TODO Make log to database.
            
            */

        }
            

    }
}
