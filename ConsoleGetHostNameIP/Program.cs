using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGetHostNameIP
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Dns.GetHostName();
            Console.WriteLine("主機名稱 ：{0} ", s);

            IPAddress[] IPS = Dns.GetHostEntry(s).AddressList;
            IEnumerator iEnums = IPS.GetEnumerator();
            while (iEnums.MoveNext())
            {
                Console.WriteLine("IP : {0}", iEnums.Current.ToString());
            }

            Console.ReadKey();
        }
    }
}
