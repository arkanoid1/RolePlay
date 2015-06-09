using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RPP = ServerRolePlay.ServerCore;
namespace ServerRolePlay
{
    public static class ServerShop
    {
        public static string GetPrices(string category = null) {

            RPP.Log.Debug("GetPrices()");
            TextWriter writer = MyAPIGateway.Utilities.WriteFileInLocalStorage("prices.xml", typeof(ServerDatabase));
            writer.WriteLine("jetpackx100:20");
            writer.Close();
            
            StringBuilder prices = new StringBuilder();
            TextReader reader = MyAPIGateway.Utilities.ReadFileInLocalStorage("prices.xml", typeof(ServerDatabase));
            prices.Append(reader.ToString());
            reader.Close();

            RPP.Log.Debug("  * " + prices.ToString());
            return prices.ToString();
        }
    }
}
