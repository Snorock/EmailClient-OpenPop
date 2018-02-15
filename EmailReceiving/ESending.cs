using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using OpenPop.Mime;
using System.Windows.Forms;

namespace EmailReceiving
{
    class ESending
    {
        public static string host;
        public static int port;
        public static void SendEmail(MailMessage mes)
        {
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(EReceiving.login, EReceiving.pass),
                EnableSsl = true
            };
            client.Send(mes);
        } 
        public static MailMessage CreateMessage(string to)
        {
            char c;
            MailMessage mes = new MailMessage(EReceiving.login, to);
            mes.To.Clear();
            
            mes.CC.Add("");
            mes.Subject = " ";
            mes.Body = " ";

            do
            {

                c = Console.ReadKey().KeyChar;
            } while (c!='0');
            return mes;
        }
        public static MailMessage PropAddTo(MailMessage mes)
        {
            string tos = "";
            if(mes.To.Count>0)
            {
                foreach(MailAddress to in mes.To)
                {
                    tos += to + "; ";
                }
                mes.To.Clear();
            }
            do
            {
                Console.WriteLine("|===== To: ");
                SendKeys.SendWait(tos);
                tos = string.Empty;
                tos = Console.ReadLine();
            } while (!tos.Contains('@'));
            int numto = tos.Count(c=> c=='@');
            if (numto > 1)
            {
                string[] tosea = tos.Split(new char[] { ',', ';', ' ' });
                foreach(string to in tosea)
                {
                    if (to.Contains('@'))//==== place for email format verification
                        mes.To.Add(to);
                }
            }
            else if (numto == 1)
                mes.To.Add(tos);

            return mes;
        }
        public static MailMessage PropAddCC(MailMessage mes)
        {
            string tos = "";
            if (mes.CC.Count > 0)
            {
                foreach (MailAddress to in mes.CC)
                {
                    tos += to + "; ";
                }
                mes.CC.Clear();
            }
                Console.WriteLine("|===== CC: ");
                SendKeys.SendWait(tos);
                tos = string.Empty;
                tos = Console.ReadLine();
            int numto = tos.Count(c => c == '@');
            if (numto > 1)
            {
                string[] tosea = tos.Split(new char[] { ',', ';', ' ' });
                foreach (string to in tosea)
                {
                    if (to.Contains('@'))//==== place for email format verification
                        mes.CC.Add(to);
                }
            }
            else if (numto == 1)
                mes.CC.Add(tos);

            return mes;
        }
        public static MailMessage PropAddBody(MailMessage mes)
        {
            //    Console.WriteLine("=================================");

            //    SendKeys.SendWait("Default text");
            //    string s = string.Empty;
            //    string inp = string.Empty;
            //    while (inp.ToUpper() != "END")
            //    {
            //        s += inp + "\n";

            //        inp = Console.ReadLine();

            //    }
            //    string[] str = s.Split(new string[] { "\n", " "}, StringSplitOptions.None );
            //    foreach(string st in str)
            //    {
            //        Console.WriteLine("=> " + st);
            //    }
            //    Console.ReadKey();
            return mes;
        }
        }
}
