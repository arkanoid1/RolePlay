using Sandbox.ModAPI;
using Sandbox.Game.Entities.Character;

using RPP = ServerRolePlay.ServerCore;
using ServerRolePlay.Proxy;
using Sandbox.Common;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.Multiplayer;
//using Sandbox.Game.Entities;

namespace ServerRolePlay
{
    class Player
    {
        #region Fields
        ulong _steamId;
        IMyPlayer _playerEntity;
        string _playerName;
        float _jetpackHeatLevel;
        bool _jetpackOverheated;

        PlayerMovementsLog PMLog;

        public string PlayerName { get { return _playerName; } }
        public ulong SteamId { get { return _steamId; } }
        public IMyPlayer PlayerEntity { get { return _playerEntity; } }

        #endregion Fields

        public Player(IMyPlayer player)
        {
            // TODO: Complete member initialization
            _playerEntity = player;
            _steamId = _playerEntity.SteamUserId;
            _playerName = _playerEntity.DisplayName;
            _jetpackHeatLevel = 0;
            _jetpackOverheated = false;
//            player.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
            PMLog = new PlayerMovementsLog(_steamId, _playerName);
        }

        //public void Controller_ControlledEntityChanged(IMyControllableEntity arg1, IMyControllableEntity arg2)
        //{
        //    RPP.Log.Debug("Controller_ControlledEntityChanged");
        //    //throw new System.NotImplementedException();
        //}

        public void Update()
        {
            if (_playerEntity.Controller != null && _playerEntity.Controller.ControlledEntity != null)
            {
                MyCharacter character = _playerEntity.Controller.ControlledEntity as MyCharacter;
                // Player may have a different or non stock jetpacks. checking overheat...
                if (character.IsDead)
                {
                    _jetpackHeatLevel = 0;
                    _jetpackOverheated = true;
                }
                else
                    CheckJetpackOverheat(character);
            }

            // Log player position for futher investigations
            PMLog.LogPosition(_playerEntity.GetPosition());





        }

        private void CheckJetpackOverheat(MyCharacter character)
        {
            // TODO make thread safe.

            RPP.Log.Info("checking jetpack for player {0} -----------------{1}--------{2}", _playerName, character.JetpackEnabled, Sandbox.Engine.Physics.MyPhysics.SimulationRatio);

            if (character.JetpackEnabled) // If jetpack is on, 
            {
                if (_jetpackOverheated)
                {
                    // turn off jetpack if overheated and player try to enable it, and reset overheating
                    _jetpackHeatLevel += 1;
                    RPP.Log.Info("Jetpack off player {0}, beacause STILL overheated", _playerName);
                    ServerComProxy.Static.SendJetpackStatusChange(_steamId, false);
                    ServerComProxy.Static.SendPlayerMessage(_steamId, "Jetpack is still overheated!", true, MyFontEnum.Red, 2000);
                }
                else
                {
                    _jetpackHeatLevel++;
                    if (_jetpackHeatLevel == 200 / Sandbox.Engine.Physics.MyPhysics.SimulationRatio) // jetpack is on, count ticks to overheating
                    {
                        RPP.Log.Info("player {0} warnned that jetpack overheating soon", _playerName);
                        ServerComProxy.Static.SendPlayerMessage(_steamId, "Jetpack overheating soon!!!", true, MyFontEnum.Red, 1000);
                    }
                    else if (_jetpackHeatLevel > 250 / Sandbox.Engine.Physics.MyPhysics.SimulationRatio)
                    {
                        _jetpackOverheated = true;
                        RPP.Log.Info("Jetpack off player {0}, because overheated", _playerName);
                        ServerComProxy.Static.SendJetpackStatusChange(_steamId, false);
                        ServerComProxy.Static.SendPlayerMessage(_steamId, "Jetpack Overheated!!!", true, MyFontEnum.Red, 5000);

                    }
                }
            }
            else  // If jetpack is off, 
            {
                if (_jetpackHeatLevel > 0)
                {
                    _jetpackHeatLevel -= (_jetpackOverheated) ? 1 * Sandbox.Engine.Physics.MyPhysics.SimulationRatio : 2 * Sandbox.Engine.Physics.MyPhysics.SimulationRatio;
                    if (_jetpackHeatLevel < 0)
                        _jetpackHeatLevel = 0;
                }

                if (_jetpackOverheated && _jetpackHeatLevel <= 0)
                {
                    // check cooldown for overheating
                    _jetpackOverheated = false;
                    _jetpackHeatLevel = 0;
                    RPP.Log.Info("player {0} warnned that jetpack overheating soon", _playerName);
                    ServerComProxy.Static.SendPlayerMessage(_steamId, "Jetpack is ready!", true, MyFontEnum.Green, 2000);
                }

            }

        }

    }
}
