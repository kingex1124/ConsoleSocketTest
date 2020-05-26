using ConsoleSocketClient3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSocketClient3
{
    class Program
    {
        //宣告網路資料流變數
        NetworkStream myNetworkStream;
        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient;


        static void Main(string[] args)
        {
            

            Console.WriteLine("Main-Start【ThreadId=" + Thread.CurrentThread.ManagedThreadId + "】：" + DateTime.Now);
            TaskFactory taskFactory = new TaskFactory();
            Task[] taskArr = new Task[100];

            for (int i = 0; i < 100; i++)
            {
                taskArr[i] = taskFactory.StartNew(() =>
                {
                    Console.WriteLine("任務-Start【ThreadId=" + Thread.CurrentThread.ManagedThreadId + "】：" + DateTime.Now);
                    Program myNetworkClient = new Program();

                    myNetworkClient.ConnectToSocket();

                    Console.WriteLine("任務-End【ThreadId=" + Thread.CurrentThread.ManagedThreadId + "】：" + DateTime.Now);

                });
            }


            Task.WaitAll(taskArr);

            Console.WriteLine("WaitAll執行之後【ThreadId=" + Thread.CurrentThread.ManagedThreadId + "】：" + DateTime.Now);

            Console.WriteLine("Main-End【ThreadId=" + Thread.CurrentThread.ManagedThreadId + "】：" + DateTime.Now);

            Console.ReadKey();
        }

        void ConnectToSocket()
        {
            //取得主機名稱
            string hostName = "127.0.0.1";

            //取得連線 IP 位址
            int connectPort = 36000;
            //建立 TcpClient 物件
            myTcpClient = new TcpClient();
            try
            {
                //測試連線至遠端主機
                myTcpClient.Connect(hostName, connectPort);
                Console.WriteLine("連線成功 !!\n");
            }
            catch
            {
                Console.WriteLine
                           ("主機 {0} 通訊埠 {1} 無法連接  !!", hostName, connectPort);
                myTcpClient.Close();
                return;
            }


            WriteListData();
            ReadListData();

            // Console.ReadKey();
            myTcpClient.Close();
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
