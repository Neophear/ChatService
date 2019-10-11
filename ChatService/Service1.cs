using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();


        }

        private void HandleComm(object peer)
        {
            TcpClient tcpClient = (TcpClient)peer;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            Thread threadSendInfo = new Thread(new ParameterizedThreadStart(SendLocalInfo));
            threadSendInfo.Start(tcpClient);

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    clientStream.Close();
                    break;
                }

                //message has successfully been received
                ReadMessage(tcpClient, (Message)Utilities.ByteArrayToObject(message));
            }
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }
    }
}
