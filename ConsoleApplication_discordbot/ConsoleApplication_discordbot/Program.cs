using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication_discordbot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Discord.DiscordClient();
            bot.MessageReceived += bot_MessageReceived;
            bot.ExecuteAndWait(async () =>
            {
                await bot.Connect("SPECIAL CODE HERE", TokenType.Bot);

            });

        }
        
        static void bot_MessageReceived(object sender, Discord.MessageEventArgs e)
        {

            if (e.Message.RawText.StartsWith("#TestCommand"))
            {
                e.Channel.SendMessage(e.User.Mention + "Test Reply");
            }
            else if (e.Message.RawText.StartsWith("#DMCommand"))
            {
                e.User.SendMessage("This is a test direct message");
            }
            else if (e.Message.RawText.StartsWith("#request"))
            {
                string msg = e.Message.RawText.Replace("#request", "");
                Array args = msg.Split(',');
                if (args.Length < 2)
                {
                    e.Channel.SendMessage("Quack? QUACK?");
                }
                else
                {
                    //keep track of request  (just for counting)
                    string summaryFile = "Summary.txt";
                    string summaryPath = System.IO.Path.Combine(Environment.CurrentDirectory, summaryFile);
                    var lineCount = File.ReadLines(summaryPath).Count();

                    System.IO.StreamWriter summaryWriteFile = new System.IO.StreamWriter(summaryPath, true);
                    lineCount = lineCount + 1;
                    summaryWriteFile.WriteLine("Request" + lineCount + "," + e.User);
                    summaryWriteFile.Close();


                    //add request
                    string fileName = "Records.txt";
                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                    string lines = "Request" + lineCount + ": Not Complete" + "\r\n" + "User:" + e.User + "\r\n" + "Item:" + args.GetValue(0) + "\r\n" + "Reward:" + args.GetValue(1) + " Gil\r\n";
                    System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                    file.WriteLine(lines);

                    file.Close();


                    //msg to user
                    e.Channel.SendMessage(e.User.Mention + "\n" + "Item:" + args.GetValue(0) + "\n" + "Reward:" + args.GetValue(1) + " Gil \n");
                    e.Server.GetChannel(318426507628380170).SendMessage("There's a new request!" + "\r\n" + lines);
                }
            }
            else if (e.Message.RawText.StartsWith("#showRequests"))
            {
                string fileName = "Records.txt";
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                string readText = File.ReadAllText(path);
                e.Channel.SendMessage(e.User.Mention + "\n" + readText);
            }

            else if (e.Message.RawText.StartsWith("#updateRequest"))
            {
                string msg = e.Message.RawText.Replace("#updateRequest ", "");
                Array args = msg.Split(',');
                if (args.Length < 2)
                {
                    e.Channel.SendMessage("Quack? QUACK?");
                }
                else
                {
                    string fileName = "Records.txt";
                    string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                    string readText = File.ReadAllText(path);



                    string requestNumber = "Request" + args.GetValue(0);

                    string[] lines = File.ReadAllLines(path);
                    string requestLine = lines.SingleOrDefault(l => l.StartsWith(requestNumber));


                    readText = readText.Replace(requestLine, requestNumber + ":" + args.GetValue(1));
                    File.WriteAllText(path, readText);

                    e.Channel.SendMessage("Request Updated");
                }

            }

            else if (e.Message.RawText.StartsWith("#help"))
            {
                e.Channel.SendMessage("Quack?");
                e.User.SendMessage("Maybe you should try '#quack?' instead >:V.");
            }
            else if (e.Message.RawText.StartsWith("#quack?"))
            {
                string fileName = "Help.txt";
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

                string helpLines = File.ReadAllText(path);
                e.Channel.SendMessage(helpLines);
            }

            else if (e.Message.RawText.StartsWith("#shutdown"))
            {
                e.Channel.SendMessage("Quack! Night O<O/ Quackum Bot is going offline");
            }
            else if (e.Message.RawText.StartsWith("#cleanRequests"))
            {
                string fileName = "Records.txt";
                string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                File.WriteAllText(path, String.Empty);
                fileName = "Summary.txt";
                path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                File.WriteAllText(path, String.Empty);

                e.Channel.SendMessage("**QUACK QUACK! EVERYTHING HAS BEEN ERASED!! REQUESTS CLEANED!**");
            }

        }
    }
}
