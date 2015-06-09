using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRP = ServerRolePlay.ServerCore;
using Sandbox.Common;
using RPCom;
using VRageMath;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using SteamSDK;

namespace ServerRolePlay
{
    class ServerComProxy
    {
        static ServerComProxy _static = null;
        static ulong _count = 0;
        public static ServerComProxy Static { get { if (_static == null) _static = new ServerComProxy(); return _static; } }

        public static readonly Encoding encode = Encoding.ASCII;


        public void Init()
        {
            SRP.Log.Debug("ServerComProxy.Init()");
            MyAPIGateway.Multiplayer.RegisterMessageHandler(SyncPacket.SyncPacketID, OnSyncRequest);
        }

        private void OnSyncRequest(byte[] bytes)
        {
            string data = encode.GetString(bytes);
            SyncPacket pckIn = new SyncPacket();

            pckIn = MyAPIGateway.Utilities.SerializeFromXML<SyncPacket>(data);

            SRP.Log.Debug("[{0}]: OnSyncRequest {1} {2}", _count++, pckIn.steamId, typeof(Command).GetEnumName(pckIn.command).ToString());

            if (pckIn.protocolVersion != SyncPacket.Version)
            {
                SRP.Log.Debug("Syn Protocol Error!");
                return;
            }

            switch ((Command)pckIn.command)
            {
                case Command.cmdHelpRequest:
                    {
                        SRP.Log.Debug("cmdHelpRequest to user: {0}", pckIn.steamId);
                        SyncPacket pck = new SyncPacket();
                        pck.protocolVersion = SyncPacket.Version;
                        pck.command = (int)Command.cmdHelpResponse;
                        pck.steamId = pckIn.steamId;
                        pck.textMessage = "use /rp prices, /rp buy item";

                        MyAPIGateway.Multiplayer.SendMessageTo(SyncPacket.SyncPacketID, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)), pckIn.steamId, false);

                        break;
                    }

                case Command.cmdShopPricesRequest:
                    {
                        SRP.Log.Debug("cmdShopPricesRequest to user: {0}", pckIn.steamId);
                        SyncPacket pck = new SyncPacket();
                        pck.protocolVersion = SyncPacket.Version;
                        pck.command = (int)Command.cmdShopPricesResponse;
                        pck.steamId = pckIn.steamId;
                        pck.textMessage = ServerShop.GetPrices();

                        MyAPIGateway.Multiplayer.SendMessageTo(SyncPacket.SyncPacketID, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)), pckIn.steamId, false);

                        break;
                    }

                case Command.cmdShopBuyItemRequest:
                    {
                        SRP.Log.Debug("cmdShopBuyItemRequest to user: {0}", pckIn.steamId);
                        //string model = "AstronautJetpack001";
                        //Vector3 colorMaskHSV = new Vector3(0.0f, 1.0f, 0.0f);


                        //List<IMyPlayer> AllPlayers = new List<IMyPlayer>();
                        //MyAPIGateway.Players.GetPlayers(AllPlayers, x => x.Controller != null && x.Controller.ControlledEntity != null);

                        //foreach (IMyPlayer player in AllPlayers)
                        //{
                        //    if (player.SteamUserId == pckIn.steamId)
                        //    {

                        //        SRP.Log.Debug("player: {0}", player.DisplayName);
                        //        IMyCharacter character = player.Controller.ControlledEntity as IMyCharacter;
                        //        character.ChangeModelAndColor(model, colorMaskHSV);
                        //    }
                        //}
                        //ChangeModelAndColor
                        SRP.Log.Debug("cmdShopPricesRequest to user: {0}", pckIn.steamId);
                        SyncPacket pck = new SyncPacket();
                        pck.protocolVersion = SyncPacket.Version;
                        pck.command = (int)Command.cmdShopBuyItemResponse;
                        pck.steamId = pckIn.steamId;
                        pck.textMessage = "ok";

                        MyAPIGateway.Multiplayer.SendMessageTo(SyncPacket.SyncPacketID, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)), pckIn.steamId, false);

                        break;
                    }

                default:
                    {
                        SRP.Log.Debug("UNKNOWN COMMAND!!!");
                        break;
                    }
            }




        }


        public void SendJetpackStatusChange(ulong id, bool JetpackEnabled)
        {
            SyncPacket pck = new SyncPacket();
            pck.protocolVersion = SyncPacket.Version;
            pck.command = (ushort)Command.Jetpackstatus;
            pck.steamId = id;
            pck.jetpack = JetpackEnabled;

            MyAPIGateway.Multiplayer.SendMessageTo(SyncPacket.SyncPacketID, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)), id, false);

        }


        public void SendPlayerMessage(ulong id, string message, bool notify = false, MyFontEnum color = MyFontEnum.Red, int delay = 1000)
        {
            SyncPacket pck = new SyncPacket();
            pck.protocolVersion = SyncPacket.Version;
            pck.command = (ushort)Command.ServerNotification;
            pck.steamId = id;
            pck.textMessage = message;
            pck.notifyDelay = delay;
            pck.fontColor = color;

            MyAPIGateway.Multiplayer.SendMessageTo(SyncPacket.SyncPacketID, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)), id, false);
        }



    }
}
