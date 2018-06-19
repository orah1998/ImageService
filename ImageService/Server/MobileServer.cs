﻿using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class MobileServer
    {
        private ImageServer m_imageServer;
        private IImageController m_controller;
        private ILoggingService m_logging;

        private int m_port;
        private TcpListener m_listener;
        private IClientHandler m_ch;
        //will be used to close the server using the "father" thread
        private bool closeCommunication;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="imageServer"></param>
        /// <param name="imageController"></param>
        /// <param name="logging"></param>
        /// <param name="port"></param>
        public MobileServer(ImageServer imageServer, IImageController imageController, ILoggingService logging, int port)
        {
            this.m_port = port;
            this.m_logging = logging;
            this.m_controller = imageController;
            this.m_imageServer = imageServer;
            this.m_ch = new ClientHandleForMobile(this.m_logging);
        }




        /// <summary>
        /// start the server
        /// </summary>
        public void Start()
        {
            this.closeCommunication = false;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), this.m_port);
            this.m_listener = new TcpListener(ep);
            this.m_listener.Start();
            m_logging.Log("starting to listen to mobile server", MessageTypeEnum.INFO);



            Task task = new Task(() => {
                while (!closeCommunication)
                {
                    try
                    {
                        // accept a new client connection
                        TcpClient client = this.m_listener.AcceptTcpClient();
                        this.m_ch.HandleClient(client);
                        m_logging.Log("start communication with mobile....", MessageTypeEnum.INFO);
                    }
                    catch (SocketException e)
                    {
                        this.m_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                }
                this.m_logging.Log("Server stopped", MessageTypeEnum.INFO);
            });
            task.Start();
        }

        /// <summary>
        /// closing the communication of the server from the "father" thread
        /// </summary>
        public void CloseCommunication()
        {
            this.closeCommunication = true;
            this.m_listener.Stop();
        }
    }
}