using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCom
{
    public enum Command
    {
        ServerMessage = 0x100,
        ServerNotification,
        Jetpackstatus,
        cmdHelpRequest,
        cmdHelpResponse,
        cmdShopPricesRequest,
        cmdShopPricesResponse,
        cmdShopBuyItemRequest,
        cmdShopBuyItemResponse
    }
}
