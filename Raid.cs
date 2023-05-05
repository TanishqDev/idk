using Newtonsoft.Json.Linq;
using System.Drawing;
using Console = Colorful.Console;
using static Phoenix.API.Config;
using System.Security.Authentication;
using WebSocketSharp;
using Newtonsoft.Json;

namespace Phoenix.API
{
    public class Raid
    {
        public static void JoinGroup(string token, string code)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/invites/{code}", "POST", token);
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void LeaveGuild(string token, ulong? id)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{id}", "DELETE", token);
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void SendMessage(string token, ulong? cid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void AddReaction(string token, ulong? cid, ulong? mid, string emoji)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages/{mid}/reactions/{emoji}/@me", "PUT", token);
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void LeaveGroup(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{gid}", "DELETE", token);
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DMUser(string token, ulong? cid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void TriggerTyping(string token, ulong? cid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/typing", "POST", token);
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ReportMessage(string token, ulong? gid, ulong? cid, ulong? mid, int reason)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/report", "POST", token, $"{{\"channel_id\": \"{cid}\", \"guild_id\": \"{gid}\", \"message_id\": \"{mid}\", \"reason\": {reason}}}");
                Console.WriteLine($"Succeed: {token}", Color.Lime);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void Boost(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var subid = Request.Send("/users/@me/guilds/premium/subscription-slots", "GET", token);
                var array = JArray.Parse(subid);
                int subidcount = 0;
                foreach (dynamic entry in array)
                {
                    subidcount++;
                    if (subidcount != 0) Request.Send($"/guilds/{gid}/premium/subscriptions", "PUT", token, $"{{\"user_premium_guild_subscription_slot_ids\":\"{subidcount}\"}}");
                }
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void OnlineTokens(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var ws = new WebSocket("wss://gateway.discord.gg/?v=10&encoding=json");
                ws.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
                ws.OnOpen += (sender, e) =>
                {
                    var auth = new
                    {
                        op = 2,
                        d = new
                        {
                            token,
                            properties = new
                            {
                                os = "Windows 11",
                                device = "Windows"
                            }
                        }
                    };
                    ws.Send(JsonConvert.SerializeObject(auth));
                };
                ws.Connect();
                ws.Close();
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void JoinVC(string token, ulong? gid, ulong? vid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var ws = new WebSocket("wss://gateway.discord.gg/?v=10&encoding=json");
                ws.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
                ws.OnOpen += (sender, e) =>
                {
                    var auth = new
                    {
                        op = 2,
                        d = new
                        {
                            token,
                            properties = new
                            {
                                os = "Windows 11",
                                device = "Windows"
                            }
                        }
                    };
                    ws.Send(JsonConvert.SerializeObject(auth));
                    var vc = new
                    {
                        op = 4,
                        d = new
                        {
                            guild_id = gid,
                            channel_id = vid,
                            self_mute = true,
                            self_deaf = true
                        }
                    };
                    ws.Send(JsonConvert.SerializeObject(vc));
                };
                ws.Connect();
                ws.Close();
            }
            catch (Exception e) { Console.WriteLine($"Failed: {token}\nError: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }
    }
}
