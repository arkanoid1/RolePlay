using System;
using System.Collections.Generic;
using System.Linq;

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.ModAPI;

using Sandbox.Common;
using Logger;

/*
    TODO: Create a tool to upload mod on steam workshop by console.

*/
namespace ClientRolePlay
{
    [Sandbox.Common.MySessionComponentDescriptor(Sandbox.Common.MyUpdateOrder.BeforeSimulation)]
    public class ClientCore : Sandbox.Common.MySessionComponentBase
    {
        private bool _initializated = false;
        private const int FRAMES_BETWEEN_UPDATES = 200;
        private int updateCount = 0;
        
        private static Log _log;
        public static Log Log { get { if (_log == null) _log = new Log("debug.log"); return _log; } }

        public static readonly ushort SYNC_PACKET = (1 << 13) + (1 << 14);

        public static readonly ulong VERSION = 1;

        void Init()
        {
            Log.WriteLine(string.Format("Core.Init()"));
            ClientComProxy.Static.Init();
            ClientChatCommands.Static.Init();
            _initializated = true;
        }

        protected override void UnloadData()
        {
            if (_log != null)
            {
                _log.WriteLine("UnloadData()");
                _log.Close();
                _log = null;
            }
            //Dispose();
        }

        //
        // == SessionComponent Hooks
        //
        public override void UpdateBeforeSimulation()
        {
            if (MyAPIGateway.Utilities == null)
                return;

            if (!_initializated)
                Init();

            try
            {
                if (updateCount % FRAMES_BETWEEN_UPDATES == 0)
                {
                    Log.WriteLine("SSAPISESEP MAIN doUpdate...");
                }
            }
            catch (Exception coreEx)
            {
                Log.WriteLine("Exception in core: " + coreEx);
            }
            updateCount++;
        }


        //public void Dispose()
        //{
        //    SEMsgProxy.Static.Dispose();
        //}
    }

}
