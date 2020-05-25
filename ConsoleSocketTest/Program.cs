using ConsoleSocketTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSocketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.IPAddress theIPAddress;
            //建立 IPAddress 物件(本機)
            theIPAddress = System.Net.IPAddress.Parse("127.0.0.1");

            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(theIPAddress, 36000);
            //啟動監聽
            myTcpListener.Start();
            Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
            Socket mySocket = myTcpListener.AcceptSocket();
           
            do
            {
                try
                {
                    //偵測是否有來自用戶端的連線要求，若是
                    //用戶端請求連線成功，就會秀出訊息。
                    if (mySocket.Connected)
                    {
                        int dataLength;
                        Console.WriteLine("連線成功 !!");
                        byte[] myBufferBytes = new byte[1000];
                        //取得用戶端寫入的資料
                        dataLength = mySocket.Receive(myBufferBytes);

                        Console.WriteLine("接收到的資料長度 {0} \n ", dataLength.ToString());
                        Console.WriteLine("取出用戶端寫入網路資料流的資料內容 :");

                        // Console.WriteLine(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength) + "\n");

                        string jsonStr = Encoding.ASCII.GetString(myBufferBytes, 0, dataLength);

                        //Newtonsoft.Json反序列化
                        List<Emp> myList = JsonConvert.DeserializeObject<List<Emp>>(jsonStr);//反序列化

                        foreach (var item in myList)
                            Console.WriteLine(string.Format("FirstName:{0} LastName:{1}", item.FirstName, item.LastName));


                        Console.WriteLine("按下 [任意鍵] 將資料回傳至用戶端 !!");
                        Console.ReadLine();
                        //將接收到的資料回傳給用戶端
                        mySocket.Send(myBufferBytes, myBufferBytes.Length, 0);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    mySocket.Close();
                    break;
                }

            } 
            while (true);
        }
    }
}
