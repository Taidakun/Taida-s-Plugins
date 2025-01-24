using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Core;
using AOSharp.Core.UI;

namespace AutoAccept
{
    public class AutoAcceot : AOPluginEntry
    {
        private bool enabled = true;
        [Obsolete]
        public override void Run(string pluginDir)
        {
            Chat.WriteLine("AutoAccept Plugin Loaded");
            Chat.WriteLine("Commands: <font color='green'>enableAutoAccept</font>, <font color='red'>disableAutoAccept</font>, <font color='white'>autoAccept</font>");

            // Registers chat commands
            Chat.RegisterCommand("enableautoaccept", Command);
            Chat.RegisterCommand("disableautoaccept", Command);
            Chat.RegisterCommand("autoaccept", Command);

            Team.TeamRequest += OnTeamRequest;
        }

        private void Command(string command, string[] param, ChatWindow chatWindow)
        {
            switch (command.ToLower())
            {
                case "autoaccept":
                    if (enabled)
                    {
                        Chat.WriteLine("Current Auto Accept status: <font color='green'>enabled</font>.");
                        Chat.WriteLine("To disable, use <font color='white'>/disableAutoAccept</font>.");
                    }
                    else
                    {
                        Chat.WriteLine("Current Auto Accept status: <font color='red'>disabled</font>.");
                        Chat.WriteLine("To enable, use <font color='white'>/enableAutoAccept</font>.");
                    }
                    break;
                case "enableautoaccept":
                    if (enabled)
                    {
                        Chat.WriteLine("Auto Accept already <font color='green'>enabled</font>, nothing happened.");
                        return;
                    }
                    enabled = true;
                    Chat.WriteLine("Auto Accept <font color='green'>enabled</font> !");
                    break;
                case "disableautoaccept":
                    if (!enabled)
                    {
                        Chat.WriteLine("Auto Accept already <font color='red'>disabled</font>, nothing happened.");
                        return;
                    }
                    enabled = false;
                    Chat.WriteLine("Auto Accept <font color='red'>disabled</font> !");
                    break;
                default:
                    return;
            }
        }

        private void OnTeamRequest(object sender, TeamRequestEventArgs e)
        {
            Chat.WriteLine($"Team invitation received from {e.Requester}");
            if (!enabled) return;
            e.Accept();
        }
    }
}
