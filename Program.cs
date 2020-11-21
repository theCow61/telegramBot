using System.Transactions;
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
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
//MAKE IT OBJECT INSTANCE, I HAVE TO DECLARE GLOBALLY EACH VARIABLE FROM CONFIG.JSON TO USE IT WHICH IS INNIFIECIENT
namespace ShittyTea
{
    static class Program
    {
        private static ITelegramBotClient botClient;
        private static string pathToProj;
        private static string pathToWL;
        private static string pathToStat;
        private static string pathToExp;
        private static string[] fileArray;
        private static bool verb = true;

        //aws

        private static string bucketName;
        private static string AWSandLocalfolderContainer;
        //private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2; GOTO WHERE awsWorker.bucketRegion is set to change
        //private static AWSCredentials credentials;

        //
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null && e.Message.From.Username != null)
            {
                if(e.Message.Text.Contains("/verbOn", StringComparison.OrdinalIgnoreCase))
                {
                    verb = true;
                }
                else if(e.Message.Text.Contains("/verbOff", StringComparison.OrdinalIgnoreCase))
                {
                    verb = false;
                }

                Console.WriteLine($"Recieved message in chat {e.Message.Chat.Id}.");
                /*await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"
                );*/

                foreach (string s in fileArray)
                {
                    if (e.Message.Text.Contains(s, StringComparison.OrdinalIgnoreCase))
                    {
                        if (verb == true)
                        {
                            await botClient.SendTextMessageAsync(e.Message.Chat, $"{e.Message.From.FirstName}, congrats u get a 1 cow credit.");
                        }
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
                } else if (e.Message.Text.Contains("/searchsploit -m", StringComparison.OrdinalIgnoreCase))
		    {
			string[] splitText = e.Message.Text.Split("searchsploit -m ");
			try
			{
				string pathModule = splitText[1].Replace("../", "");
				using (FileStream fs = System.IO.File.OpenRead($@"{pathToExp}{pathModule}"))
				{
					InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, "UrExploit");
					await botClient.SendDocumentAsync(e.Message.Chat, inputOnlineFile);
				}
				//await botClient.SendDocumentAsync(e.Message.Chat, $@"/opt/exploitdb/exploits/{pathModule}");
			} catch
			{
				await botClient.SendTextMessageAsync(e.Message.Chat, "lol no");
			}
		} else if (e.Message.Text.Contains("/searchsploit", StringComparison.OrdinalIgnoreCase))
		{
			string[] splitText = e.Message.Text.Split("searchsploit");
			try
			{
				//string output = $"searchsploit \"{splitText[1]}\"".Bash();
				await botClient.SendTextMessageAsync(e.Message.Chat, $"{$"searchsploit \"{splitText[1]}\"".Bash()}");
			} catch 
			{

			}
		} else if (e.Message.Text.Contains("/transfer", StringComparison.OrdinalIgnoreCase))
                {
                    //string[] transferData = e.Message.Text.Split("to");
                    var match = Regex.Match(e.Message.Text, @"/transfer (?<amount>.*) (?<touser>.*)");
                    string amount = Convert.ToString(match.Groups["amount"]);
                    string toUser = Convert.ToString(match.Groups["touser"]);
                    string toUserClean = "";
                    if(toUser[0] == '@')
                    {
                        toUserClean = toUser.Remove(0, 1);
                    }
                    else
                    {
                        toUserClean = toUser;
                    }
                    try
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, $"{Transfer(e.Message.From.Username, amount, toUserClean)}");
                    } catch
                    {
			await botClient.SendTextMessageAsync(e.Message.Chat, "oops");
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
                else if (e.Message.Text.Contains("/IdoNotKnowTheSyntaxOfThisBot", StringComparison.OrdinalIgnoreCase))
                {
                    await botClient.SendTextMessageAsync(e.Message.Chat, "/IdoNotKnowTheSyntaxOfThisBot -- Help Menu\n" +
                        "/balance -- View how many cow credits you have\n/creditsFull -- View everyones available credits\n/transfer <#ofCredits> <username> -- Give someone credits(no spaces)\n/ctfUp -- Upcoming ctf's according to ctftime" + 
                        "\n/verbOff -- Turns off getting told when you are given credit for saying something\n/verbOn -- Turns on getting told when you are given credit.\n/searchsploit <term> -- Gives exploits back (if a lot of exploits it wont spit any).\n/searchsploit -m <path> -- Sends exploit over telegram.\nA lot of commands are possition sensitive like the path has to be one space after the m but transfer needs to have no space inbetween \"to\" and username so make sure you use commands exactly possitioned like in this menu." + 
                        "\n/IamGayIfIuseThis \"(Message)\" (# of spam)\nUpload a single document/file with the caption of /upload for it to upload to AWS\n/download <file name> -- Have telegram send you file from AWS");
                }
                else if(e.Message.Text.Contains("/IamGayIfIuseThis", StringComparison.OrdinalIgnoreCase))
                {
                    string toCompare = "/(?i)iamgayifiusethis \"(?<spamsg>.*)\" (?<count>.*)";
                    var match = Regex.Match(e.Message.Text, toCompare);
                    string messageToSpam = "";
                    int count = 0;
                    try
                    {
                        messageToSpam = Convert.ToString(match.Groups["spamsg"]);
                        Console.WriteLine(messageToSpam);
                        string stCount = Convert.ToString(match.Groups["count"]);
                        count = Convert.ToInt32(stCount);
                        Console.WriteLine(count);
                        if (count <= 0)
                        {
                            await botClient.SendTextMessageAsync(e.Message.Chat, "format is /iamgayifiusethis \"(message)\" (amount spammed)");
                        }
                        else 
                        {
                            for(int i = 1; i <= count; i++)
                            {
                                await botClient.SendTextMessageAsync(e.Message.Chat, $"{messageToSpam}");
                            }
                        }
                    }
                    catch
                    {
                        await botClient.SendTextMessageAsync(e.Message.Chat, "Format is /iamgayifiusethis \"(message)\" (amount to spam)");
                    }
                }
                /*else if(e.Message.Text.Equals("/upload"))
                {
                    //credentials = new StoredProfileAWSCredentials("spikey");
                    s3Client = new AmazonS3Client(bucketRegion);
                    UploadFileAsync().Wait();
                }
                else if(e.Message.Text.Contains("/uptoad"))
                {
                    Console.WriteLine("emoji man");
                    var file = await botClient.GetFileAsync(e.Message.Document.FileId);
                    FileStream fs = new FileStream("trest.txt", FileMode.Create);
                    await botClient.DownloadFileAsync(file.FilePath, fs);
                }
                */
                else if(e.Message.Text.Contains("/download"))
                {
                    try
                    {
                        var match = Regex.Match(e.Message.Text, @"/download (?<path>.*)");
                        string givinPath = Convert.ToString(match.Groups["path"]);
                        AWSworker awsWorker = new AWSworker();
                        awsWorker.bucketName = bucketName;
                        awsWorker.bucketRegion = RegionEndpoint.USEast2;
                        awsWorker.filePlacement = $"{AWSandLocalfolderContainer}{givinPath}";
                        awsWorker.assignS3();
                        await awsWorker.DownloadFileAsync();

                        using (FileStream fs = System.IO.File.OpenRead($@"{AWSandLocalfolderContainer}{givinPath}"))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, givinPath);
                            await botClient.SendDocumentAsync(e.Message.Chat, inputOnlineFile);
                        }
                    }
                    catch (Exception ea)
                    {
                        Console.WriteLine(ea);
                    }
                }
            }

            else if(e.Message.Type == MessageType.Document)
            {
                if(e.Message.Caption == "/upload")
                {
                    //s3Client = new AmazonS3Client(bucketRegion);
                    var file = await botClient.GetFileAsync(e.Message.Document.FileId);
                    //FileStream fs = new FileStream(file.FilePath, FileAc);
                    //await botClient.DownloadFileAsync(file.FilePath, fs);
                    //fs.Close();
                    //fs.Dispose();
                    MemoryStream ms = new MemoryStream();
                    await botClient.DownloadFileAsync(file.FilePath, ms);
                    //await UploadFileAsync(ms, e.Message.Document.FileName);

                    AWSworker awsWorker = new AWSworker();
                    awsWorker.bucketName = bucketName;
                    awsWorker.bucketRegion = RegionEndpoint.USEast2;
                    awsWorker.ms = ms;
                    awsWorker.filePlacement = $"{AWSandLocalfolderContainer}{e.Message.Document.FileName}";
                    awsWorker.assignS3();
                    await awsWorker.UploadFileAsync();

                }
            }
        }

	private static string Bash(this string cmd)
	{
		string escapedArgs = cmd.Replace("\"", "\\\"");
		Process process = new Process()
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "/bin/bash",
				Arguments = $"-c \"{escapedArgs}\"",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			}
		};
		process.Start();
		string result = process.StandardOutput.ReadToEnd();
		string refResult = result.Replace("-", "");
		process.WaitForExit();
		return refResult;
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
            Console.WriteLine(amount);
            string json = System.IO.File.ReadAllText(pathToStat);
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
                realAmount = Convert.ToInt32(amount);
                if (realAmount <= 0)
                {
                    return $"Fuck off {fromUsername}.";
                }
            }
            catch
            {
                return $"{amount} is not valid as number tard";
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
                System.IO.File.WriteAllText(pathToStat, output);
                return $"{fromUsername} sent {realAmount} credits to {toUsername}";
            }

            return "";
        }
        private static string ReadStats(string username, string firstName, string mode)
        {
            string json = System.IO.File.ReadAllText(pathToStat);
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
            string json = System.IO.File.ReadAllText(pathToStat);
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
            System.IO.File.WriteAllText(pathToStat, output);
        }

        private static void setConfigOps()
        {

            string json = System.IO.File.ReadAllText("config.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            pathToProj = jsonObj["Settings"]["PathToProject"];
            pathToWL = pathToProj + jsonObj["Settings"]["Wordlist"];
            pathToStat = pathToProj + jsonObj["Settings"]["Stat"];
            pathToExp = jsonObj["Settings"]["PathToExploitdb"];
            string token = jsonObj["Settings"]["Token"];
            fileArray = System.IO.File.ReadAllLines(pathToWL);
            //aws
            bucketName = jsonObj["Settings"]["AWSbucketName"];
            AWSandLocalfolderContainer = jsonObj["Settings"]["AWSandLocalContainFolder"];
            botClient = new TelegramBotClient(token);
        }
        static void Main(string[] args)
        {
            setConfigOps();
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hey fags, I am {me.Id} but u can call me {me.FirstName}.");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Console.ReadKey();
            botClient.StopReceiving();
        }
    }
}
