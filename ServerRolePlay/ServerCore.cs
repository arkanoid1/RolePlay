using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using NLog;

using ServerRolePlay;
using ServerRolePlay.Proxy;
using System.Runtime.ExceptionServices;
using System.Security;
using VRage.Plugins;
using Sandbox;
using Sandbox.ModAPI;
using Sandbox.Game.Multiplayer;

namespace ServerRolePlay
{
    public class ServerCore : IPlugin
    {

        public static Logger Log;
        private PlayerMain PlayerMain;
        private static System.Object lockThis = new System.Object();
        private MySandboxGame _mySandboxGameInstance;


        public void Init(Object instance)
        {
            _mySandboxGameInstance = instance as MySandboxGame;
            Log = LogManager.GetLogger("MainLog");
            PlayerMain = new PlayerMain();
            ServerComProxy.Static.Init();
            Log.Info("ServerRolePlay Initializated");
        }

        public void Update()
        {
            Log.Debug("Update ServerRolePlay()");
            try
            {
                PlayerMain.Update();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

        }


        public void Dispose()
        {
            
        }
    }
}
