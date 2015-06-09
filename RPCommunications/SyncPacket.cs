using Sandbox.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCom
{
    public struct SyncPacket
    {
        public static readonly ushort SyncPacketID = (1 << 13) + (1 << 14);
        public static readonly int Version = SyncPacketID + 0x0001;

        public int protocolVersion;
        public int command;
        public ulong steamId;
        public string textMessage;
        public MyFontEnum fontColor;
        public bool jetpack;
        public int notifyDelay;
    }
}
