
using microServiceBus.BizTalkReceiveeAdapter.RunTime;
using Microsoft.BizTalk.Message.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using microServiceBus.BizTalkReceiveeAdapter.Helper;
using System.Net.Sockets;
using System.Net;

namespace microServiceBus.BizTalkReceiveeAdapter.TestConsole
{
    class Program
    {
        static TcpClient client;
        const string submitURI = @"microservicebus://onramp";
        static void Main(string[] args) {

            new BizTalkServiceHelper().Test();
            //new BizTalkServiceHelper().UnInstallTest();
            //new BizTalkServiceHelper().InstallTest();

            //Console.WriteLine("wait");
            //Console.ReadKey();
            //try
            //{
            //    client = new TcpClient();
            //    client.Connect("localhost", 3001);

            //    // Send
            //    var stream = client.GetStream();
            //    var bytesToSend = Encoding.UTF8.GetBytes("Hello");
            //    stream.Write(bytesToSend, 0, bytesToSend.Length);

            //    // Receive
            //    var bytesToRead = new byte[client.ReceiveBufferSize];
            //    var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);


            //    client.Close();
            //    Console.ReadKey();
            //}
            //catch (Exception ex) {
            //    Console.WriteLine();
            //}
        }

    }
}
