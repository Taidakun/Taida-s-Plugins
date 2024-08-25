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
        private bool enabled = true;
        [Obsolete]
        public override void Run(string pluginDir)
        {
            try
            {
                Chat.WriteLine("AutoInv Plugin Loaded");
                Chat.WriteLine("Any tell you receive with the word 'inv', 'invite' or 'team' will send the sender a team invite if there are room in your team and you've got leadership");
                Chat.WriteLine("To disable auto-invites, use <font color='white'>/disableAutoInv</font>. To re-enable, use <font color='white'>/enableAutoInv</font>.");
                Chat.WriteLine("To see the current auto-invite status, use <font color='white'>/autoinv</font>.");
                Chat.RegisterCommand("enableAutoInv", Command);
                Chat.RegisterCommand("disableAutoInv", Command);
                Chat.RegisterCommand("autoinv", Command);
                Network.ChatMessageReceived += Network_ChatMessageReceived;

            }
            catch (Exception e)
            {
                Chat.WriteLine(e.Message);
            }
        }

        private void Command(string command, string[] param, ChatWindow chatWindow)
        {
            switch(command)
            {
                case "autoinv":
                    if (enabled)
                    {
                        Chat.WriteLine("Current status: <font color='green'>enabled</font>.");
                        Chat.WriteLine("To disable, use <font color='white'>/disableAutoInv</font>.");
                    }
                    else
                    {
                        Chat.WriteLine("Current status: <font color='red'>disabled</font>.");
                        Chat.WriteLine("To enable, use <font color='white'>/enableAutoInv</font>.");
                    }
                    break;
                case "enableAutoInv":
                    if (enabled)
                    {
                        Chat.WriteLine("Already <font color='green'>enabled</font>, nothing happened.");
                        return;
                    }
                    enabled = true;
                    Chat.WriteLine("Auto invite <font color='green'>enabled</font> !");
                    break;
                case "disableAutoInv":
                    if (!enabled)
                    {
                        Chat.WriteLine("Already <font color='red'>disabled</font>, nothing happened.");
                        return;
                    }
                    enabled = false;
                    Chat.WriteLine("Auto invite <font color='red'>disabled</font> !");
                    break;
                default:
                    return;
            }
        }

        private void Network_ChatMessageReceived(object s, SmokeLounge.AOtomation.Messaging.Messages.ChatMessageBody chatMessage)
        {
            if (!enabled) return;
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
