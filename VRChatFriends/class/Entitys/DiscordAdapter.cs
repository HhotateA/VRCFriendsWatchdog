using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord;
using Discord.WebSocket;
using System.Reflection;
using System.IO;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using VRChatFriends.Function;
using System.Windows.Threading;
using Newtonsoft.Json;
using VRChatFriends.Function;

namespace VRChatFriends.Entity
{
    class DiscordAdapter
    {
        static DiscordAdapter instance;
        public static DiscordAdapter Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new DiscordAdapter();
                }
                return instance;
            }
        }
        DiscordSocketClient client;
        CommandService commands;
        ServiceProvider services;
        ISocketMessageChannel messageChannel;
        ISocketMessageChannel logChannel;
        DiscordConfigData configData;

        public DiscordAdapter()
        {
            Initialize();
        }
        async void Initialize()
        {
            await LoadSettings();
            InitAsync();
            MessageSender();
        }

        async Task InitAsync()
        {
            if(!String.IsNullOrWhiteSpace(configData.discordTolken))
            {
                client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Info
                });

                client.Log += Log;
                commands = new CommandService();
                services = new ServiceCollection().BuildServiceProvider();
                client.MessageReceived += CommandRecieved;

                await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

                await client.LoginAsync(TokenType.Bot, configData.discordTolken);
                await client.StartAsync();

                await Task.Delay(-1);
            }
        }

        /// <summary>
        /// 何かしらのメッセージの受信
        /// </summary>
        /// <param name="msgParam"></param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            //メッセージがnullの場合
            if (message == null)
                return;

            //発言者がBotの場合無視する
            if (message.Author.IsBot)
                return;

            //デバッグ用メッセージを出力
            Debug.Log(message.Channel.Name + " " + message.Author.Username + ":" + message);

            var context = new CommandContext(client, message);

            var CommandContext = message.Content;

            // 召喚コマンド
            if (CommandContext == configData.summonCommand)
            {
                logChannel = message.Channel;
                await logChannel.SendMessageAsync(configData.summonMessage);
            }
            // 召喚コマンド
            if (CommandContext == configData.notificationCommand)
            {
                messageChannel = message.Channel;
                await messageChannel.SendMessageAsync(configData.notificationMessage);
            }
            // さよならコマンド
            if (CommandContext == configData.stopCommand)
            {
                if(message.Channel == messageChannel)
                {
                    messageChannel = null;
                    await message.Channel.SendMessageAsync(configData.stopMessage);
                }
                if(message.Channel == logChannel)
                {
                    logChannel = null;
                    await message.Channel.SendMessageAsync(configData.stopMessage);
                }
            }
            // 学習メッセージ
            foreach(var value in configData.botMessage)
            {
                bool isMutch = false;
                // key判定
                if(value.Key.StartsWith("%"))
                {
                    var origin = value.Key.Remove(0, 1);
                    if (Regex.IsMatch(CommandContext, origin))
                    {
                        isMutch = true;
                    }
                }
                else
                {
                    if (CommandContext.Contains(value.Key))
                    {
                        isMutch = true;
                    }
                }
                // value判定
                if(isMutch)
                {
                    if(value.Value.StartsWith("%"))
                    {
                        var origin = value.Value;
                        var rands = new Regex(@"\{(.+?)\}").Matches(origin);
                        for (int i= 0;i< rands.Count; i++)
                        {
                            var rand = rands[i].Value.Split(',');
                            var random = Functions.RandomInt(rand.Length);
                            origin = Functions.ReplaceFirst(origin,rands[i].Value, rand[random]);

                        }
                        origin = origin.Replace("%", "")
                                       .Replace("{", "")
                                       .Replace("}", "");
                        await message.Channel.SendMessageAsync(origin);
                        break;
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(value.Value);
                        break;
                    }
                }
            }
            // 新規学習
            if (CommandContext.Contains(configData.studyCommand))
            {
                var d = CommandContext.Split('&');
                if(d.Length == 3)
                {
                    if (!String.IsNullOrWhiteSpace(d[1]) && !String.IsNullOrWhiteSpace(d[2]))
                    {
                        if (configData.botMessage.ContainsKey(d[1]))
                        {
                            configData.botMessage[d[1]] = d[2];
                        }
                        else
                        {
                            configData.botMessage.Add(d[1], d[2]);
                        }
                        await message.Channel.SendMessageAsync(configData.studyMessage);
                        await SaveLog();
                    }
                }
            }
            // 忘れる
            if (CommandContext.Contains(configData.forgetCommand))
            {
                var d = CommandContext.Split('&');
                if(d.Length == 3)
                {
                    if (!String.IsNullOrWhiteSpace(d[1]) && !String.IsNullOrWhiteSpace(d[2]))
                    {
                        if (configData.botMessage.ContainsKey(d[1]))
                        {
                            configData.botMessage.Remove(d[1]);
                        }
                        await message.Channel.SendMessageAsync(configData.forgetMessage);
                        await SaveLog();
                    }
                }
            }
            // 辞書表示
            if (CommandContext.Contains(configData.dictionaryCommand))
            {
                await message.Channel.SendMessageAsync(configData.dictionaryMessage);
                foreach (var value in configData.botMessage)
                {
                    string msg = value.Key + "は," + value.Value + "にゃ！";
                    await message.Channel.SendMessageAsync(msg);
                }
            }
        }

        List<string> MessageStore { get; set; } = new List<string>();
        public async void SendMessage(string msg)
        {
            if (messageChannel != null)
            {
                if (MessageStore.Count > configData.messageLimmit &&
                    MessageStore.Last() != configData.limmitedMessage)
                {
                    MessageStore.Add(configData.limmitedMessage);
                }
                else
                {
                    MessageStore.Add(msg);
                }
            }
        }
        async Task MessageSender()
        {
            while (true)
            {
                await Task.Delay(500);
                if (MessageStore.Count != 0)
                {
                    await messageChannel?.SendMessageAsync(MessageStore[0]);
                    MessageStore.RemoveAt(0);
                }
                if (LogStore.Count != 0)
                {
                    await logChannel?.SendMessageAsync(LogStore[0]);
                    LogStore.RemoveAt(0);
                }
            }
        }
        List<string> LogStore { get; set; } = new List<string>();
        public async void SendLog(string msg)
        {
            if (logChannel != null)
            {
                if (LogStore.Count > configData.messageLimmit &&
                    LogStore.Last() != configData.limmitedMessage)
                {
                    LogStore.Add(configData.limmitedMessage);
                }
                else
                {
                    LogStore.Add(msg);
                }
            }
        }

        private Task Log(LogMessage message)
        {
            Debug.Log(message.ToString());
            return Task.CompletedTask;
        }

        async Task LoadSettings()
        {
            await Task.Run(() =>
            {
                Debug.Log("Load Discord File");
                string filePath = Functions.FileCheck(ConfigData.DiscordConfigPath);
                using (StreamReader sr = new StreamReader(
                    filePath,
                    Encoding.UTF8))
                {
                    var f = sr.ReadToEnd();
                    try
                    {
                        configData = JsonConvert.DeserializeObject<DiscordConfigData>(f);
                        if (configData == null) configData = new DiscordConfigData();
                    }
                    catch
                    {
                        configData = new DiscordConfigData();
                    }
                }
            }).ConfigureAwait(false);
            await SaveLog();
        }
        public async Task SaveLog()
        {
            await Task.Run(() =>
            {
                Debug.Log("Save Discord File");
                string filePath = Functions.FileCheck(ConfigData.DiscordConfigPath);
                if(configData==null)configData = new DiscordConfigData();
                var f = JsonConvert.SerializeObject(configData);
                using (StreamWriter sw = new StreamWriter(
                    filePath,
                    false, Encoding.UTF8))
                {
                    sw.Write(f);
                }
            }).ConfigureAwait(false);
        }
    }
}
