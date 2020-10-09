using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VRChatFriends.Function
{
    [JsonObject]
    class DiscordConfigData
    {
        [JsonProperty("token")]
        public string discordTolken = "";
        [JsonProperty("summon_command")]
        public string summonCommand = "%%StartLog";
        [JsonProperty("summon_message")]
        public string summonMessage = "にゃんにゃんさんじょーにゃ！ฅ(＾・ω・＾ฅ)";
        [JsonProperty("notification_command")]
        public string notificationCommand = "%%StartNotice";
        [JsonProperty("notification_message")]
        public string notificationMessage = "にゃんにゃんさんじょーにゃ！ฅ(＾・ω・＾ฅ)";
        [JsonProperty("stop_command")]
        public string stopCommand = "にゃんにゃんハウス！";
        [JsonProperty("stop_message")]
        public string stopMessage = "サッ";
        [JsonProperty("srudy_command")]
        public string studyCommand = "おべんきょにゃ&";
        [JsonProperty("study_message")]
        public string studyMessage = "おぼえたにゃっ！<(＾・ω・＾)>";
        [JsonProperty("forget_command")]
        public string forgetCommand = "わすれろ&";
        [JsonProperty("forget_message")]
        public string forgetMessage = "1...2...(*ﾟдﾟ)ﾎﾟｶ~ﾝ....";
        [JsonProperty("dictionary_command")]
        public string dictionaryCommand = "でぃくしょなりー";
        [JsonProperty("dictionary_message")]
        public string dictionaryMessage = "りょうかいにゃ(｀･ω･´)ゞ";
        [JsonProperty("message_limit")]
        public int messageLimmit = 30;
        [JsonProperty("limited_message")]
        public string limmitedMessage = "とぅーめにーめっせーじにゃ～(>_<)";
        [JsonProperty("bot_dictionary")]
        public Dictionary<string, string> botMessage = new Dictionary<string, string>()
        {
            {"きゃろ","にゃー"},
            {"さかな","はむはむ💕"},
            {"ちゅーる","にゃー！💕"},
        };
    }
}
