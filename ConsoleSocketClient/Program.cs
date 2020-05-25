using ConsoleSocketClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSocketClient
{
    class Program
    {
        //宣告網路資料流變數
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;
        static void Main(string[] args)
        {
            Program myNetworkClient = new Program();

            Console.WriteLine("輸入連接機名稱 : ");
            //取得主機名稱
            string hostName = Console.ReadLine();
            Console.WriteLine("輸入連接通訊埠 : ");
            //取得連線 IP 位址
            int connectPort = int.Parse(Console.ReadLine());
            //建立 TcpClient 物件
            myNetworkClient.myTcpClient = new TcpClient();
            try
            {
                //測試連線至遠端主機
                myNetworkClient.myTcpClient.Connect(hostName, connectPort);
                Console.WriteLine("連線成功 !!\n");
            }
            catch
            {
                Console.WriteLine
                           ("主機 {0} 通訊埠 {1} 無法連接  !!", hostName, connectPort);
                return;
            }


            myNetworkClient.WriteListData();
            myNetworkClient.ReadListData();

            //myNetworkClient.WriteData();
            //myNetworkClient.ReadData();
            Console.ReadKey();
        }

        //寫入資料
        void WriteData()
        {
            String strTest = "this is a test string !!";
            //將字串轉 byte 陣列，使用 ASCII 編碼
            Byte[] myBytes = Encoding.ASCII.GetBytes(strTest);

            Console.WriteLine("建立網路資料流 !!");
            //建立網路資料流
            myNetworkStream = myTcpClient.GetStream();

            Console.WriteLine("將字串寫入資料流　!!");
            //將字串寫入資料流
            myNetworkStream.Write(myBytes, 0, myBytes.Length);
        }

        //讀取資料
        void ReadData()
        {
            Console.WriteLine("從網路資料流讀取資料 !!");
            //從網路資料流讀取資料
            int bufferSize = myTcpClient.ReceiveBufferSize;
            byte[] myBufferBytes = new byte[bufferSize];
            myNetworkStream.Read(myBufferBytes, 0, bufferSize);
            //取得資料並且解碼文字
            Console.WriteLine(Encoding.ASCII.GetString(myBufferBytes, 0, bufferSize));
        }

        //寫入資料
        void WriteListData()
        {
            List<Emp> empList = new List<Emp>();

            empList.Add(new Emp() { FirstName = "kevan", LastName = "Chen" });
            empList.Add(new Emp() { FirstName = "tom", LastName = "Lin" });

            //Newtonsoft.Json序列化
            string jsonData = JsonConvert.SerializeObject(empList);

            //將字串轉 byte 陣列，使用 ASCII 編碼
            Byte[] myBytes = Encoding.ASCII.GetBytes(jsonData);
      
            Console.WriteLine("建立網路資料流 !!");
            //建立網路資料流
            myNetworkStream = myTcpClient.GetStream();

            Console.WriteLine("將字串寫入資料流　!!");
            //將字串寫入資料流
            myNetworkStream.Write(myBytes, 0, myBytes.Length);
        }

        //讀取資料
        void ReadListData()
        {
            Console.WriteLine("從網路資料流讀取資料 !!");
            //從網路資料流讀取資料
            int bufferSize = myTcpClient.ReceiveBufferSize;
            byte[] myBufferBytes = new byte[bufferSize];
            myNetworkStream.Read(myBufferBytes, 0, bufferSize);
            //取得資料並且解碼文字
            string jsonStr = Encoding.ASCII.GetString(myBufferBytes, 0, bufferSize);

            //Newtonsoft.Json反序列化
            List<Emp> myList = JsonConvert.DeserializeObject<List<Emp>>(jsonStr);//反序列化

            foreach (var item in myList)
                Console.WriteLine(string.Format("FirstName:{0} LastName:{1}", item.FirstName, item.LastName));
        }
    }
}
