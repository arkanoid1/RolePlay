using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCom;
using CRP = ClientRolePlay.ClientCore;
using VRageMath;
using Sandbox.Game.Entities.Character;

namespace ClientRolePlay
{

    class ClientComProxy
    {
        static ClientComProxy _static = null;
        public static ClientComProxy Static { get { if (_static == null) _static = new ClientComProxy(); return _static; } }

        public static readonly Encoding encode = Encoding.ASCII;
        static uint _count = 0;

        public void Init()
        {
            MyAPIGateway.Multiplayer.RegisterMessageHandler((ushort)ClientCore.SYNC_PACKET, OnSyncRequest);
        }


        public void OnSyncRequest(byte[] bytes)
        {


            string data = encode.GetString(bytes);
            SyncPacket pck = new SyncPacket();

            pck = MyAPIGateway.Utilities.SerializeFromXML<SyncPacket>(data);
            ClientAPIGatewayProxy.ShowMessage("RPP", "[{0}]: OnSyncRequest {1} {2}", _count++, pck.steamId, pck.command.GetType().ToString());

            if (pck.protocolVersion != SyncPacket.Version)
            {
                ClientAPIGatewayProxy.ShowMessage("RPP", "Syn Protocol Error!");
                return;
            }

            switch ((Command)pck.command)
            {
                case Command.Jetpackstatus:
                    {
                        if (MyAPIGateway.Session.Player.SteamUserId != pck.steamId)
                        {   // Wrong player
                            // todo logging
                            return;
                        }

                        //MyAPIGateway.Session.Player.Controller.ControlledEntity.SwitchThrusts();
                        List<IMyPlayer> player = new List<IMyPlayer>();
                        MyAPIGateway.Players.GetPlayers(player, x => x.SteamUserId == pck.steamId && x.Controller != null && x.Controller.ControlledEntity != null);
                        if ((player[0].Controller.ControlledEntity.EnabledThrusts && !pck.jetpack) || (!player[0].Controller.ControlledEntity.EnabledThrusts && pck.jetpack))
                            player[0].Controller.ControlledEntity.SwitchThrusts();

                        ClientAPIGatewayProxy.ShowMessage("RPP", "ShowMessage Server send SYNC packet to off {0} jetpack", player[0].IdentityId);

                        //MyAPIGateway.Multiplayer.SendMessageToOthers(9000, newData);

                        break;
                    }
                case Command.ServerMessage:
                    {
                        ClientAPIGatewayProxy.ShowMessage("RPP", pck.textMessage);
                        break;
                    }
                case Command.ServerNotification:
                    {

                        ClientAPIGatewayProxy.ShowNotification(pck.fontColor, pck.notifyDelay, pck.textMessage);
                        break;
                    }
                case Command.cmdShopPricesResponse:
                    {

                        MyAPIGateway.Utilities.ShowMissionScreen("Prices", "1", "2", pck.textMessage);
                        break;
                    }

                case Command.cmdHelpResponse:
                    {

                        MyAPIGateway.Utilities.ShowMissionScreen("Role Play Help and Rules", "1", "2", pck.textMessage);
                        break;
                    }

                case Command.cmdShopBuyItemResponse:
                    {

                        ClientAPIGatewayProxy.ShowMessage("", pck.textMessage);

                        string model = "AstronautJetpack001";
                        Vector3 colorMaskHSV = new Vector3(0.0f, 1.0f, 0.0f);
                        IMyCharacter character = MyAPIGateway.Session.Player.Controller.ControlledEntity as IMyCharacter;
                        character.ChangeModelAndColor(model, colorMaskHSV);


                        break;
                    }

                default:
                    {
                        ClientAPIGatewayProxy.ShowMessage("", "UNKNOWN COMMAND!!!");
                        break;
                    }
            }




        }

        public void SendPriceRequest()
        {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive)
                return;
            SyncPacket pck = new SyncPacket();
            pck.protocolVersion = SyncPacket.Version;
            pck.command = (ushort)Command.cmdShopPricesRequest;
            pck.steamId = MyAPIGateway.Session.Player.SteamUserId;

            MyAPIGateway.Multiplayer.SendMessageToServer(ClientCore.SYNC_PACKET, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)));
        }

        public void SendHelpRequest()
        {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive)
                return;
            SyncPacket pck = new SyncPacket();
            pck.protocolVersion = SyncPacket.Version;
            pck.command = (ushort)Command.cmdHelpRequest;
            pck.steamId = MyAPIGateway.Session.Player.SteamUserId;

            MyAPIGateway.Multiplayer.SendMessageToServer(ClientCore.SYNC_PACKET, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)));
        }

        internal void SendBuyRequest(string items)
        {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive)
                return;
            SyncPacket pck = new SyncPacket();
            pck.protocolVersion = SyncPacket.Version;
            pck.command = (ushort)Command.cmdShopBuyItemRequest;
            pck.steamId = MyAPIGateway.Session.Player.SteamUserId;
            pck.textMessage = items;

            MyAPIGateway.Multiplayer.SendMessageToServer(ClientCore.SYNC_PACKET, encode.GetBytes(MyAPIGateway.Utilities.SerializeToXML<SyncPacket>(pck)));
        }
    }
}
