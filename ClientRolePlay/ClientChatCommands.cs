using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRP = ClientRolePlay.ClientCore;
namespace ClientRolePlay
{
    public class ClientChatCommands
    {
        static ClientChatCommands _static = null;
        public static ClientChatCommands Static
        {
            get
            {
                if (_static != null)
                    return _static;
                _static = new ClientChatCommands();
                return _static;
            }
        }
        public void Init()
        {
            CRP.Log.WriteLine(string.Format("ChatCommands MessageEntered set"));

            MyAPIGateway.Utilities.MessageEntered += ChatMessageEntered;
        }

        public void ChatMessageEntered(string messageText, ref bool sendToOthers)
        {
            try
            {
                CRP.Log.WriteLine(string.Format("ChatMessageEntered: {0}", messageText));
                if (messageText.StartsWith("/help"))
                {
                    // TODO: extended help by all SERVER commands
                    ClientAPIGatewayProxy.ShowMessage("", "You can use commands:");
                    ClientAPIGatewayProxy.ShowMessage("", "/ch to get chat history");
                    ClientAPIGatewayProxy.ShowMessage("", "/rp help");
                    ClientAPIGatewayProxy.ShowMessage("", "/rp prices");
                    ClientAPIGatewayProxy.ShowMessage("", "/rp buy $item");
                }
                else if (messageText.StartsWith("/rp "))
                {
                    string[] commands = (messageText.Remove(0, 4)).Split(null as string[], 2, StringSplitOptions.RemoveEmptyEntries);
                    if (commands.Length > 0)
                    {
                        string internalCommand = commands[0];
                        string arguments = (commands.Length > 1) ? commands[1] : "";
                        CRP.Log.WriteLine(string.Format("internalCommand: {0}", internalCommand));

                        if (internalCommand == "prices")
                        {   // TODO: list of prices by category in dialog
                            CRP.Log.WriteLine(string.Format("ClientComProxy.Static.SendPriceRequest();"));
                            ClientComProxy.Static.SendPriceRequest();
                        }
                        else if (internalCommand == "buy")
                        {   // TODO: buy item

                            CRP.Log.WriteLine(string.Format("ClientComProxy.Static.SendBuyRequest(arguments);"));
                            ClientComProxy.Static.SendBuyRequest(arguments);
                        }
                        else if (internalCommand == "help")
                        {   // TODO: extended help by all RP commands
                            CRP.Log.WriteLine(string.Format("ClientComProxy.Static.SendHelpRequest();"));
                            ClientComProxy.Static.SendHelpRequest();
                        }

                    }
                }
                else if (messageText.StartsWith("/ch "))
                {
                    // TODO: show last 20 lines in dialog.


                    // sendmessage to client and show chat history on client side
                }
                else
                {
                    // TODO: Just log message for chathistory


                }
            }
            catch (Exception ex)
            {
                CRP.Log.WriteLine(string.Format("Excepyion in ChatCommands: {0}", ex.Message));
            }

        }
    }
}
