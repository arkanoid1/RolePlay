using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerRolePlay.Proxy
{
    static class ServerAPIGatewayProxy
    {
        public static void ShowMessage(string from, string format, params object[] list)
        {
            MyAPIGateway.Utilities.ShowMessage(from, string.Format(format, list));
        }

        public static void SendMessage(string format, params object[] list)
        {
            MyAPIGateway.Utilities.SendMessage(string.Format(format, list));
        }
    }
}
