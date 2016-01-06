﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Codeaddicts.Lizard
{
    public class DataStream : IDisposable
    {
        public TcpClient Client;
        public NetworkStream Stream { get; private set; }

        private IPEndPoint _endPoint;
        public IPEndPoint EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if (value == null) return;

                _endPoint = value;
                Client = new TcpClient();

                try
                {
                    Client.Connect(EndPoint);
                    Stream = Client.GetStream();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Client = null;
                    Stream = null;
                    EndPoint = null;
                }
            }
        }

        public DataStream () {
            Client = null;
            Stream = null;
            EndPoint = null;
        }

        public void Close () {
            Stream.Dispose ();
            Client = null;
            EndPoint = null;
            Stream = null;
        }

        public void Send (byte[] data) {
            Stream.Write (data, 0, data.Length);
        }

        #region IDisposable implementation

        public void Dispose () {
            Stream.Dispose ();
        }

        #endregion
    }
}

