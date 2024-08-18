using System;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.ChatMessages;
using SmokeLounge.AOtomation.Messaging.Messages;

namespace AutoInv
{
    public class AutoInv : AOPluginEntry
    {
        [Obsolete]
        public override void Run(string pluginDir)
        {
            try
            {
                Chat.WriteLine("AutoInv Plugin Loaded");
                Chat.WriteLine("Any tell you receive with the word 'inv', 'invite' or 'team' will send the sender a team invite if there are room in your team and you've got leadership");

                Network.ChatMessageReceived += Network_ChatMessageReceived;

            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void Network_ChatMessageReceived(object s, SmokeLounge.AOtomation.Messaging.Messages.ChatMessageBody chatMessage)
        {
            if (chatMessage.PacketType == ChatMessageType.PrivateMessage)
            {
                switch(((PrivateMsgMessage)chatMessage).Text) {
                    case "invite":
                    case "team":
                    case "inv":
                    Chat.WriteLine($"Received {((PrivateMsgMessage)chatMessage).Text} from {((PrivateMsgMessage)chatMessage).Sender}");
                    Team.Invite(new Identity(IdentityType.SimpleChar, Convert.ToInt32(((PrivateMsgMessage)chatMessage).Sender)));
                    break;
                    default:
                        break;
                };
            }
        }
    }
}
