using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AOSharp.Core;
using AOSharp.Core.UI;
using AOSharp.Common.GameData;
using SmokeLounge.AOtomation.Messaging.Messages.ChatMessages;
using SmokeLounge.AOtomation.Messaging.Messages;
using SmokeLounge.AOtomation.Messaging.GameData;

namespace AutoInv
{
    public class AutoInv : AOPluginEntry
    {
        private bool enabled = true;
        public string FILEBANNED;

        HashSet<string> Banned = new HashSet<string>();
        HashSet<BannedEntry> Banlist = new HashSet<BannedEntry>();
        
        [Obsolete]
        public override void Run(string pluginDir)
        {
            SimpleChar c = new SimpleChar(DynelManager.LocalPlayer);
            // FILEBANNED = Path.Combine(Path.Combine(pluginDir, "Banlist"),  c.Name + " Banlist.txt");
            try
            {
                // Checks banlist
                string[] banList = File.ReadAllLines(FILEBANNED);
                foreach (string banned in banList)
                {
                    Banned.Add(banned.ToUpper().Trim());
                }

                // Displays initialization messages
                Chat.WriteLine("AutoInv Plugin Loaded");
                Chat.WriteLine("Any tell you receive with the word 'inv', 'invite' or 'team' will send the sender a team invite if there are room in your team and you've got leadership");
                Chat.WriteLine("To disable auto-invites, use <font color='white'>/disableAutoInv</font>. To re-enable, use <font color='white'>/enableAutoInv</font>.");
                Chat.WriteLine("To see the current auto-invite status, use <font color='white'>/autoinv</font>.");

                // Registers chat commands
                Chat.RegisterCommand("enableAutoInv", Command);
                Chat.RegisterCommand("disableAutoInv", Command);
                Chat.RegisterCommand("autoinv", Command);
                Chat.RegisterCommand("temp", Command);

                // Manages all tells
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
                case "temp":
                    Chat.WriteLine(FILEBANNED);
                    break;
                default:
                    return;
            }
        }

        private void Network_ChatMessageReceived(object s, ChatMessageBody chatMessage)
        {
            if (!enabled) return;
            if (chatMessage.PacketType == ChatMessageType.PrivateMessage) ;
            Identity player = new Identity(IdentityType.SimpleChar, Convert.ToInt32(((PrivateMsgMessage)chatMessage).Sender));
            // SimpleChar temp = new SimpleChar(test);
            switch(((PrivateMsgMessage)chatMessage).Text.ToLower()) {
                case "invite":
                case "team":
                case "inv":
                    Chat.WriteLine($"Received {((PrivateMsgMessage)chatMessage).Text} from {player}");
                    Team.Invite(player);
                    break;
                case "test":
                    Chat.WriteLine("Test OK");
                    break;
                case "checkbanned":
                    Chat.WriteLine("Checking Banned peeps...");
                    string[] banList = File.ReadAllLines(FILEBANNED);
                    if (banList.Length > 0)
                    {
                        Chat.WriteLine($"Currently {File.ReadAllLines(FILEBANNED).Length} people banned.");
                        foreach (string banned in banList)
                        {
                            Chat.WriteLine(banned, ChatColor.White);
                        }
                    }
                    else
                        Chat.WriteLine("No one is banned for now !");
                    break;
                default:
                    break;
            };
        }

        public class BannedEntry
        {
            public string BannedName;
            public string BannedReason;
            public BannedEntry(string BannedName, string BannedReason)
            {
                this.BannedName = BannedName;
                this.BannedReason = BannedReason;
            }
        }

        //private bool isBanned(string User)
        //{
        //    return (Banned.Where(o => o == User.ToUpper()).FirstOrDefault() != null);
        //}
    }
}