using Newtonsoft.Json.Linq;
using System.Drawing;
using static Phoenix.Config;
using Console = Colorful.Console;

namespace Phoenix
{
    public class Raid
    {
        public static void JoinGuild(string token, string code)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/invites/{code}", "POST", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGuild(string token, ulong? id)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/users/@me/guilds/{id}", "DELETE", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddFriend(string token, string username, uint discriminator)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/relationships", "POST", token, $"{{\"username\":\"{username}\",\"discriminator\":{discriminator}}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void SendMessage(string token, ulong? cid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddReaction(string token, ulong? cid, ulong? mid, string emoji)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages/{mid}/reactions/{emoji}/@me", "PUT", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void BlockUser(string token, ulong? uid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/users/@me/relationships/{uid}", "PUT", token, $"{{\"type\":\"2\"}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGroup(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{gid}", "DELETE", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void DMUser(string token, ulong? uid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/channels", token, "POST", $"{{\"recipient_id\":\"{uid}\"}}");
                var array = JObject.Parse(request);
                dynamic entry = array;
                Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void TriggerTyping(string token, ulong? cid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/typing", "POST", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void ReportMessage(string token, ulong? gid, ulong? cid, ulong? mid, int reason)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/report", "POST", token, $"{{\"channel_id\": \"{cid}\", \"guild_id\": \"{gid}\", \"message_id\": \"{mid}\", \"reason\": {reason}}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }
    }
}