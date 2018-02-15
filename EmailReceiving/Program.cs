using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Text.RegularExpressions;
using System.Threading;


namespace EmailReceiving
{
    class Program
    {
       
        public static List<Message> inBox = new List<Message>();
        public static List<Message> sentBox = new List<Message>();
        public static List<Message> allMail = new List<Message>();
        static void Main(string[] args)

        {
            //Console.SetWindowSize(160, 90);
            //string c = Console.ReadKey(true).Key.ToString();
            //Console.WriteLine(c);
            //Console.Beep();
            allMail = EReceiving.DownloadAllMail();
            SortMailSender(allMail, EReceiving.login, true);
            Timer tm = new Timer(TimerCallback, null, 180000, 180000);
            char k;
            do
            {
                Console.Clear();
                
                Console.WriteLine("|==== View:\n|= 1-All mail ({0})\n|= 2-Inbox ({1})\n|= 3-Sent Box({2})\n|= 4-Update\n\n0-Exit ", allMail.Count(), inBox.Count(), sentBox.Count());
                k = Console.ReadKey(true).KeyChar;
                switch (k)
                {
                    case '1':
                        PrintMessageList(allMail);
                        break;
                    case '2':
                        PrintMessageList(inBox);
                        break;
                    case '3':
                        PrintMessageList(sentBox);
                        break;
                    case '4':
                        EReceiving.UpdateMail(allMail);
                        break;
                    case '0':
                        Console.WriteLine("Press any key");
                        break;
                    default:
                        Console.WriteLine("Incorrect choice. Press any key");
                        Console.ReadKey();
                        break;
                }
            } while (k != '0');

            //++++++++++++++++++++++++++
            // Console.WriteLine("Press any key");
            // Console.ReadKey();
            // if (allMail != null)
            // { 
            //// SortMailSender(allMail, EReceiving.login, true);
            // PrintMessageList(allMail);
            // }
            // Console.ReadKey();
            // PrintMessage(allMail[4]);
            // Console.ReadKey();.


        }
        
        private static void TimerCallback(Object o)
        {
            EReceiving.UpdateMail(allMail);
        }
        public static List<Message> SortMailSender(List<Message> allMail, string from, bool fromthis)
        {
            List<Message> thisSender = new List<Message>();
            List<Message> notthisSender = new List<Message>();
            foreach (Message mail in allMail)
            {
                if (mail.Headers.From.ToString().Contains(from))
                    thisSender.Add(mail);
                else
                    notthisSender.Add(mail);
            }
            if (from == EReceiving.login)
            {
                inBox = notthisSender;
                sentBox = thisSender;
                return null;
            }
            else if (fromthis)
                return thisSender;
            else
                return notthisSender;
        }
        public static void PrintMessageList (List<Message> emlist)
        {
            
            int size = emlist.Count();
            int c=-1;
            int page=1;
            char choice='z';
            do
            {
                Console.Clear();
                Console.WriteLine("Emails in folder: " + size);
                Console.WriteLine("Page number: " + page);
                Console.WriteLine("\nTo open email press it's number from left column \nQ - exit to main menu   A - previous page   S - seearch email   D - next page");
                Console.WriteLine();
                Console.WriteLine("{0,3} = {1,25} == {2}", "#", "From:", "To:");
                for (int i = (page - 1) * 10; i < size && i < 10 * page; i++)
                {
                    try
                    {
                        Console.WriteLine("{0,3} = {3,15} = {1,25} \n== Subject: {2}", i % 10, GetAllTo(emlist[i]), emlist[i].Headers.Subject,emlist[i].Headers.From.Address);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("==== " + e.Message);
                    }
                }
                choice=Console.ReadKey().KeyChar;
                if (Char.IsDigit(choice))
                {
                    c = (int)Char.GetNumericValue(choice) + (page - 1) * 10;
                    PrintMessage(emlist[c]);
                }
                else if (choice == 'a' && page > 1)
                    page--;
                else if (choice == 'd' && !(size <= page*10))
                    page++;

            } while (choice != 'q');
        }
        public static void PrintMessage (Message mes)
        {
            string b = mes.ToMailMessage().Body;
           // Regex regex = new Regex(string.Format("\\<.*?\\>"));
            Console.Clear();
            Console.WriteLine("|==========================================|");
            Console.WriteLine("|==== From: " + mes.Headers.From);
            Console.WriteLine("|===== To(" + mes.ToMailMessage().To.Count + "): " + GetAllTo(mes));
            Console.WriteLine("|= Subject: " + mes.Headers.Subject);
            Console.WriteLine("|= Date: " + mes.Headers.Date);
            Console.WriteLine("|==========================================|");
            //Console.WriteLine("  " + mes.ToMailMessage().Body);
            //while(b.Contains("<")&&b.Contains(">"))
            //{
            //    b=regex.Replace(b, string.Empty);
            //}
            
        
        Console.WriteLine("     " + StripTagsCharArray(b));
            if (Console.ReadKey().KeyChar == 's')
            {
                ESending.SendEmail(mes.ToMailMessage());
            }
         
        }
        public static string GetAllTo(Message mes)
        {
            string toall="";

            if(mes.Headers.To != null)
            {
                foreach (var to in mes.Headers.To)
                {
                    toall += to.Address + "; ";
                }
            }

            return toall;
        }
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        
    }
}
