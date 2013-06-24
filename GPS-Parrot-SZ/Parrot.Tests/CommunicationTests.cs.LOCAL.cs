using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using System.Globalization;

namespace Parrot.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CommunicationTests
    {
        public CommunicationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Tx()
        {
            TcpClient client = new TcpClient();
            //client.Connect(IPAddress.Parse("218.16.125.144"), 3300);
            client.Connect(IPAddress.Parse("127.0.0.1"), 3300);
            if (client.Connected)
            {
                Console.WriteLine("Connected.");
            }

            NetworkStream sendStream = client.GetStream();
            string s = "7E 52 77 45 41 44 41 41 41 42 4E 49 41 41 42 59 75 41 41 41 6A 4E 41 41 44 39 32 33 7A 75 57 38 3D 7F";
            Console.WriteLine(s);
            s = s.Replace(" ", "");

            int size = s.Length / 2;
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = byte.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            s = ASCIIEncoding.ASCII.GetString(data);
            s = string.Format("##,1,1,0,9300,{0},{1},0\r\n", "117.136.12.244:11021", s);

            Console.WriteLine("Send:");
            Console.WriteLine(s);

            data = Encoding.ASCII.GetBytes(s);
            sendStream.Write(data, 0, data.Length);
            sendStream.Flush();

            client.Close();
            Console.WriteLine();
            Console.WriteLine("关闭连接。");
        }

        [TestMethod]
        public void Tx2()
        {
            TcpClient client = new TcpClient();
            //client.Connect(IPAddress.Parse("218.16.125.144"), 3300);
            client.Connect(IPAddress.Parse("127.0.0.1"), 3300);
            if (client.Connected)
            {
                Console.WriteLine("Connected.");
            }

            NetworkStream sendStream = client.GetStream();
            Console.WriteLine("Send:");
            string s = "##,2,1,0\r\n";
            Console.WriteLine(s);


            sendStream.Write(Encoding.Default.GetBytes(s),0,s.Length);
            sendStream.Flush();

            client.Close();
            Console.WriteLine();
            Console.WriteLine("关闭连接。");
        }

        [TestMethod]
        public void Rx()
        {
            TcpClient client = new TcpClient();
            //client.Connect(IPAddress.Parse("218.16.125.144"), 3300);
            client.Connect(IPAddress.Parse("127.0.0.1"), 3300);
            Assert.IsTrue(client.Connected);
            Console.WriteLine("Connected.");

            Console.WriteLine("Recv.:");
            NetworkStream recvStream = client.GetStream();
            recvStream.ReadTimeout = 8000;
            byte[] data = new byte[512];
            int size = recvStream.Read(data, 0, data.Length);
            foreach (byte item in data)
            {
                Console.Write((char)item);
            }
            Console.WriteLine();

            Console.WriteLine("pdu:");
            string s = ASCIIEncoding.ASCII.GetString(data, 0, size);
            Console.WriteLine(s);

            string[] dataParts = s.Split(',');
            Console.WriteLine("\tSequence Number: " + dataParts[2]);
            Console.WriteLine("\tListening Port: " + dataParts[4]);
            string endPoint = dataParts[5];
            Console.WriteLine("\tEnd Point: " + endPoint);
            string[] endPointParts = endPoint.Split(':');
            string ip = endPointParts[0];
            Console.WriteLine("\t\tIP: " + ip);
            string port = endPointParts[1];
            Console.WriteLine("\t\tPort: " + port);
            string pdu = dataParts[6];
            Console.WriteLine("\t\tPDU: " + pdu);


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Decoded from base64:");
            byte[] pduData = Convert.FromBase64String(pdu);
            foreach (byte item in pduData)
            {
                Console.Write("{0:X2} ", item);
            }

            client.Close();
            Console.WriteLine();
            Console.WriteLine("关闭连接。");
        }
    }
}
