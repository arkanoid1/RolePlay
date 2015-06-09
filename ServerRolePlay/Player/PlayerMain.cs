using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

using Sandbox.ModAPI;
using Sandbox.Game.Entities.Character;

using VRageMath;
using System.Reflection;
using VRage.ModAPI;

using RPP = ServerRolePlay.ServerCore;
using ServerRolePlay.Proxy;
using System.Security.Permissions;
using System.Diagnostics;

namespace ServerRolePlay
{
    class PlayerMain
    {

        //Definition
        private List<Player> _players;

        private List<ulong> _lastConnectedPlayerList;

        public PlayerMain()
        {
            RPP.Log.Info("PlayerMain constructor");
            _lastConnectedPlayerList = new List<ulong>();
            _players = new List<Player>();
//            MyAPIGateway.Entities.OnEntityAdd += Entities_OnEntityAdd;
        }
        public void Update()
        {
            RPP.Log.Debug("PlayerMain.Update()");
            // Get list of spawned players
            List<IMyPlayer> connectedPlayers = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(connectedPlayers, x => x.Controller != null && x.Controller.ControlledEntity != null);

            foreach (IMyPlayer player in connectedPlayers)
            {
                if (!_lastConnectedPlayerList.Contains(player.SteamUserId))
                    _players.Add(new Player(player));
            }

            RPP.Log.Debug(" * Update players statuses");
            try
            {
                foreach (Player player in _players)
                {
                    RPP.Log.Debug(" * * update player {0}({1})", player.PlayerName, player.SteamId);
                    player.Update();
                }

            }
            catch (Exception ex)
            {
                ServerCore.Log.Error(ex);
            }

            _lastConnectedPlayerList = connectedPlayers.ConvertAll(x => x.SteamUserId).ToList();
        }

        void Entities_OnEntityAdd(IMyEntity obj)
        {   // added entinity
            RPP.Log.Debug("Entities_OnEntityAdd = {0}", obj.DisplayName);
            if (obj is MyCharacter)
            {
                MyCharacter pl = (MyCharacter)obj;
                pl.EnableJetpack(false);
                //pl.OnWeaponChanged += pl_OnWeaponChanged;
                //pl.Crouch();
            }
        }

        //private void pl_OnWeaponChanged(object sender, EventArgs e)
        //{
        //    RPP.Log.Debug("***pl_OnWeaponChanged*** {0} {1} {2}", (sender as MyCharacter).DisplayName, (sender as MyCharacter).EntityId);
        //}

    }
}
