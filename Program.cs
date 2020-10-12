using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Telegram.Bot.Types;
using HtmlAgilityPack;

namespace ShittyTea
{
    class Program
    {
        static ITelegramBotClient botClient;
        static string path = @"C:\Users\Zane Salti\source\repos\ShittyTea\wl.txt";
        static string[] fileArray = System.IO.File.ReadAllLines(path);
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Recieved message in chat {e.Message.Chat.Id}.");
                /*await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"
                );*/

                foreach (string s in fileArray)
                {
                    if (e.Message.Text.Contains(s, StringComparison.OrdinalIgnoreCase))
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"{e.Message.From.FirstName}, congrats u get a 1 cow credit.");
                        Stats(s, e.Message.From.Username, 1);
                        break;
                    } 
                    /*switch (e.Message.Text)
                    {
                        case string a when a.Contains(s, StringComparison.OrdinalIgnoreCase):
                            break;
                    }*/
                }
                if (e.Message.Text.Contains("/balance", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, $"{ReadStats(e.Message.From.Username, e.Message.From.FirstName, "balance")}");
                } else if (e.Message.Text.Contains("/creditsFull", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, $"{ReadStats(e.Message.From.Username, e.Message.From.FirstName, "creditsFull")}");
                } else if (e.Message.Text.Contains("/transfer", StringComparison.OrdinalIgnoreCase))
                {
                    string[] transferData = e.Message.Text.Split("to");
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"{Transfer(e.Message.From.Username, transferData[0], transferData[1])}");
                    } catch
                    {

                    }
                    /*
                    string sendToUser = e.Message.Text;
                    int amount = Convert.ToInt32(e.Message.Text);
                    await botClient.SendTextMessageAsync(e.Message.Chat, $"{Transfer(e.Message.From.Username, sendToUser, amount)}");
                    /*

                    await botClient.SendTextMessageAsync(e.Message.Chat, $"{e.Message.From.FirstName} who do you want to give your credits too?");
                    if (e.Message.Text.Contains(""))
                    {
                        sendToUser = e.Message.Text;
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"How many creds do you want to give?");
                        if (int.TryParse(e.Message.Text, out amount))
                        {
                            amount = Convert.ToInt32(e.Message.Text);
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"{Transfer(e.Message.From.Username, sendToUser, amount)}");

                        }
                    }
                    
                    */
                    /*
                    switch (step)
                    {
                        case 1:
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"{e.Message.From.FirstName} who do you want to give your credits too?");
                            sendToUser = e.Message.Text;
                            step++;
                            break;
                        case 2:
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"How many creds do you want to give?");
                            if (int.TryParse(e.Message.Text, out amount))
                            {
                                amount = Convert.ToInt32(e.Message.Text);
                                await botClient.SendTextMessageAsync(e.Message.Chat, $"{Transfer(e.Message.From.Username, sendToUser, amount)}");
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(e.Message.Chat, $"Invalid number");
                            }
                            break;

                    }*/
                } else if (e.Message.Text.Contains("/ctfUp", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, $"{CTFupcoming()}");
                }
                else if (e.Message.Text.Contains("/help", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, "To get full list of commands do /IdoNotKnowTheSyntaxOfThisBotAndMyLifeIsFullOfShameAndSorrow");
                }
                else if (e.Message.Text.Contains("/IdoNotKnowTheSyntaxOfThisBotAndMyLifeIsFullOfShameAndSorrow", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, "/IdoNotKnowTheSyntaxOfThisBotAndMyLifeIsFullOfShameAndSorrow,IsuckAtEverythingIncludingUsingAbotForTelegram -- Help Menu\n" +
                        "/balance -- View how many cow credits you have\n/creditsFull -- View everyones available credits\n/transfer<#ofCredits>to<username> -- Give someone credits(no spaces)\n/ctfUp -- Upcoming ctf's according to ctftime");
                }
            }
        }
        private static string CTFupcoming()
        {
            int count = 0;
            string scraped = "";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://ctftime.org/event/list/upcoming");
            foreach (var item in doc.DocumentNode.SelectNodes("//table[@class='table table-striped']//tr"))
            {
                scraped += item.InnerText;
                count++;
                if (count >= 5)
                {
                    break;
                }
            }
            return Convert.ToString(scraped.Replace("\n\n\n", "\n"));
        }
        private static string Transfer(string fromUsername, string amount, string toUsername)
        {
            string suAmount = amount.Remove(0, "/transfer".Length);
            Console.WriteLine(suAmount);
            string json = System.IO.File.ReadAllText(@"C:\Users\Zane Salti\source\repos\ShittyTea\Stat.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            if (jsonObj["Credits"][0][fromUsername] == null || jsonObj["Credits"][0][fromUsername] == 0)
            {
                jsonObj["Credits"][0][fromUsername] = 0;
                return $"{fromUsername} you don't have any credits, to broke";
            }
            if (jsonObj["Credits"][0][toUsername] == null)
            {
                jsonObj["Credits"][0][toUsername] = 0;
            }
            
            int fromBalance = jsonObj["Credits"][0][fromUsername];
            int recBalance = jsonObj["Credits"][0][toUsername];
            int realAmount = 0;
            try
            {
                realAmount = Convert.ToInt32(suAmount);
                if (realAmount <= 0)
                {
                    return $"Fuck off {fromUsername}.";
                }
            }
            catch
            {
                return $"{suAmount} is not valid as number tard";
            }
            if (fromBalance < realAmount)
            {
                return $"lulz you dont even have {realAmount} credits.";
            }
            if (fromBalance >= realAmount)
            {
                int fromBalanceNewTrans = fromBalance - realAmount;
                int recBalanceNewTrans = recBalance + realAmount;
                jsonObj["Credits"][0][fromUsername] = fromBalanceNewTrans;
                jsonObj["Credits"][0][toUsername] = recBalanceNewTrans;
                string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(@"C:\Users\Zane Salti\source\repos\ShittyTea\Stat.json", output);
                return $"{fromUsername} sent {realAmount} credits to {toUsername}";
            }

            return "";
        }
        private static string ReadStats(string username, string firstName, string mode)
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\Zane Salti\source\repos\ShittyTea\Stat.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            if (mode.Equals("balance"))
            {
                int balance = 0;
                try
                {
                    balance = jsonObj["Credits"][0][username];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return $"{firstName} you got {balance} cow credits.";
            }
            else if (mode.Equals("creditsFull"))
            {
                return $"{jsonObj["Credits"][0]}";
            }


            else if (mode.Equals("transfer"))
            {
                string output = "";
                if (jsonObj["Credits"][0][username] == null)
                {
                    jsonObj["Credits"][0][username] = 0;
                    output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    return "You got zero credits and ur broke";
                } else return "";
                
            }


            else return null;
        }
        private static void Stats(string word, string username, int credits)
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\Zane Salti\source\repos\ShittyTea\Stat.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            if (jsonObj["Credits"][0][username] != null)
            {
                jsonObj["Credits"][0][username] += credits;
            }
            if (jsonObj["Words"][0][word] != null)
            {
                jsonObj["Words"][0][word] += 1;
            }
            if (jsonObj["Credits"][0][username] == null)
            {
                jsonObj["Credits"][0][username] = 0;
                jsonObj["Credits"][0][username] += credits;
            }
            if (jsonObj["Words"][0][word] == null)
            {
                jsonObj["Words"][0][word] = 0;
                jsonObj["Words"][0][word] += 1;
            }


            string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(@"C:\Users\Zane Salti\source\repos\ShittyTea\Stat.json", output);
        }
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1255929985:AAFZVSUX-nxjTXGb15RFA-mXt0GiPam7sWs");
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hey fags, I am {me.Id} but u can call me {me.FirstName}.");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Console.ReadKey();
            botClient.StopReceiving();
        }
    }
}
