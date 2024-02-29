# Phoenix Nuker

[![Stars](https://img.shields.io/github/stars/extatent/Phoenix-Nuker?label=Stars&style=for-the-badge)](https://github.com/extatent/Phoenix-Nuker/stargazers)
[![Forks](https://img.shields.io/github/forks/extatent/Phoenix-Nuker?label=Forks&style=for-the-badge)](https://github.com/extatent/Phoenix-Nuker/network/members)
[![License](https://img.shields.io/github/license/extatent/Phoenix-Nuker?style=for-the-badge)](https://github.com/extatent/Phoenix-Nuker/blob/main/LICENSE)
[![Download Phoenix](https://img.shields.io/badge/Download-Phoenix-Green?style=for-the-badge)](https://github.com/extatent/Phoenix-Nuker/releases/download/Download/Phoenix.zip)

---
### Discord server
Join the Discord server for updates and support: https://dsc.gg/phoenix-nuker

---
### Updates
Phoenix Nuker is available at no cost. To keep it free, you can show your support by starring the project. Upon reaching specific milestones, marked by the number of stars achieved, new updates will be released. The progress is outlined as follows:
| ⭐ | # |
| --- | --- |
| 16 | ✅ |
| 32 | ✅ |
| 64 | ✅ |
| 128 | ✅ |
| 256 | ✅ |
| 288 | ✅ |
| 320 | ✅ |
| 352 | ✅ |
| 384 | ❌ |
| 448 | ❌ |
| 512 | ❌ |

---

### Features

* ` Easy-to-use interface`
* ` Secure (0% chance of getting banned or rate-limited)`
* ` Feature-rich (50+ different features)`
* ` No repeatable token or server ID entering required`
* ` Always up-to-date (updating the project every time a new API function/version is released)`
* ` Webhook spammer/deleter`
* ` Fully destroy an account or server`
* ` Supports bot and user authentication tokens`
* ` Selfbot (run commands through your own Discord account)`
* ` Custom Discord Rich Presence (custom playing status)`
* ` Stealer Builder (build a stealer that steals Discord tokens)`
* ` And more`

<details>
<summary>Phoenix Preview</summary>
<img src="https://i.imgur.com/A13TAMz.png">
<img src="https://i.imgur.com/aW5yttp.png">
<img src="https://i.imgur.com/QdRBdoJ.png">
<img src="https://i.imgur.com/FlayPSv.png">
</details>

---

## Frequently Asked Questions

**1) How do I get a Guild/User ID?**
> *User Settings > Advanced > Enable Developer Mode. After that, right click on a guild/user > Copy Server/User ID*

**2) How do I get my Discord token?**
<details>
<summary>APP</summary>

> *Press the Windows Key + R and type %appdata%\discord in the dialog box.*

> *Search for the settings.json file and open it in Notepad or any text editor of your choice.*

> *Add "DANGEROUS_ENABLE_DEVTOOLS_ONLY_ENABLE_IF_YOU_KNOW_WHAT_YOURE_DOING": true, so it should look like this:* 
```json
{
  "DANGEROUS_ENABLE_DEVTOOLS_ONLY_ENABLE_IF_YOU_KNOW_WHAT_YOURE_DOING": true,
  "IS_MAXIMIZED": false,
  "IS_MINIMIZED": false,
  "START_MINIMIZED": true,
  "WINDOW_BOUNDS": {
    "x": 0,
    "y": 0,
    "width": 0,
    "height": 0
  }
}
```
> *Save the file and exit the text editor.*

> *Restart the Discord app by first exiting and then relaunching the app.*

> *Press CTRL+Shift+J in the APP, and paste:*
```javascript
(webpackChunkdiscord_app.push([[''],{},e=>{m=[];for(let c in e.c)m.push(e.c[c])}]),m).find(m=>m?.exports?.default?.getToken!==void 0).exports.default.getToken()
```
> Note: you may need to type "allow pasting" before pasting the code.
</details>
<details>
<summary>Browser</summary>

> *Go to Discord in your browser, log in, press CTRL+SHIFT+J, and paste:*
```javascript
(webpackChunkdiscord_app.push([[''],{},e=>{m=[];for(let c in e.c)m.push(e.c[c])}]),m).find(m=>m?.exports?.default?.getToken!==void 0).exports.default.getToken()
```
> Note: you may need to type "allow pasting" before pasting the code.
</details>

**3) Why does the program automatically close after opening?**
> *If the program automatically closes after opening, you may need to install .NET Framework:*

[Download .NET Framework 4.8.1](https://download.microsoft.com/download/4/b/2/cd00d4ed-ebdd-49ee-8a33-eabc3d1030e3/NDP481-Web.exe)

**4) How do I select a token or guild ID in Phoenix?**
> *In the "Check Tokens" tab, paste the token and press the "Check Tokens" button. In the "Management" tab, right-click on an account and press "Select" to choose the token. After selecting the token, guilds should appear if the account owns or moderates any guilds. Right-click on a guild and press "Select" to choose the guild ID.*
