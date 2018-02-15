using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Globalization;

namespace EmailReceiving
{
    
    class EReceiving
    {
        public const string login = "snorocker@gmail.com";
        public static string pass = "";
        public static string hostname;
        public static int port;
        public static bool usessl;
        public static DateTime updt;
        public static List<Message> DownloadAllMail()
        {
            SetMailprovider(login);
            updt = DateTime.Now;
            Console.WriteLine("{0:HH:mm:ss tt}", updt);
            bool loginsuccess = true;
            Pop3Client client = new Pop3Client();
            client.Connect(hostname, port, usessl);
            Console.Write("login: " + login + "\nPassword: ");
            pass = (string) Console.ReadLine();
            try {
                client.Authenticate(login, pass, AuthenticationMethod.UsernameAndPassword);
            }
            catch(Exception e)
            {
                Console.WriteLine("Wrong login/password");
                loginsuccess = false;
            }
            if (loginsuccess)
            {
                int totalamount = client.GetMessageCount();

                Console.WriteLine("Number of emails: " + totalamount);
                Console.WriteLine("Please wait...");
                List<Message> allMail = new List<Message>();
                for (int i = totalamount; i > 0; i--)
                {
                    try {
                        allMail.Add(client.GetMessage(i));
                    }
                    catch(Exception e)
                    {
                       // Console.WriteLine(i + " ==--== " + e.Message);
                        client.Dispose();
                        client = new Pop3Client();
                        client.Connect("pop.gmail.com", 995, true);
                        client.Authenticate(login, pass, AuthenticationMethod.UsernameAndPassword);
                        i++;

                    }
                }
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine("Emails dowloaded: " + allMail.Count());
                // Message msgtest = client.GetMessage(totalamount);
                // Console.WriteLine(msgtest.Headers.From + " " + msgtest.Headers.Subject);

                // Console.WriteLine(msgtest.ToMailMessage().Body);

                // msgtest = client.GetMessage(1);
                // Console.WriteLine(msgtest.Headers.From + " " + msgtest.Headers.Subject);
                // ////Console.WriteLine(msgtest.ToMailMessage().Body);
                // //msgtest = client.GetMessage(totalamount-2);
                // //Console.WriteLine(msgtest.Headers.From + " " + msgtest.Headers.Subject);
                // //try {
                // //   // Console.WriteLine(msgtest.ToMailMessage().IsBodyHtml);
                // //}
                // //catch(Exception ex)
                // //{
                // //   Console.WriteLine(ex.Message);
                // //}
                // //msgtest = client.GetMessage(totalamount-3);
                // //Console.WriteLine(msgtest.Headers.From + " " + msgtest.Headers.Subject);
                // //Console.WriteLine(msgtest.ToMailMessage().Body);
                //client.Disconnect();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return allMail;
            }
            else
                return null;
        }

        public static void SetMailprovider(string login)
        {
            string[] parts = login.Split('@');
            //Console.WriteLine(parts[1]);
            switch(parts[1])
            {
                case "gmail.com":
                    hostname = "pop.gmail.com";
                    port = 995;
                    usessl = true;
                    ESending.host = "smtp.gmail.com";
                    ESending.port = 587;
                    break;
                default:
                    break;
            }
        }
        public static List<Message> UpdateMail(List<Message> allMail)
        {
            Pop3Client client = new Pop3Client();
            client.Connect(hostname, port, usessl);
            client.Authenticate(login, pass, AuthenticationMethod.UsernameAndPassword);
            int countu = client.GetMessageCount();
            if (countu!=allMail.Count)
            {
               int i = countu;
                try {
                    while (Convert.ToDateTime(client.GetMessage(i).Headers.Date, CultureInfo.CurrentCulture) >= updt && i > 0)
                    {
                        allMail.Insert(0, client.GetMessage(i));
                        i--;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(client.GetMessage(i).Headers.Date);
                    Console.ReadKey();
                }
                updt = DateTime.Now;
                Program.SortMailSender(allMail, login, true);
            }
            //Console.Beep();
            return allMail;
        }
        
    }
}
