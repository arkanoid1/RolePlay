using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sandbox.ModAPI;
using Sandbox.Common;

namespace ClientRolePlay
{

    static class ClientAPIGatewayProxy
    {
        static ulong _messageId = 0;
        static ulong _notifyId = 0;
        public static void ShowMessage(string from, string format, params object[] list)
        {
            MyAPIGateway.Utilities.ShowMessage(from, (++_messageId).ToString()+": "+string.Format(format, list));
        }

        public static void SendMessage(string format, params object[] list)
        {
            MyAPIGateway.Utilities.SendMessage(string.Format(format, list));
        }

        public static void ShowNotification(MyFontEnum font, int time, string format, params object[] list)
        {
            MyAPIGateway.Utilities.ShowNotification((++_notifyId).ToString() + ": " + string.Format(format, list), time, font);
        }
    }

}
