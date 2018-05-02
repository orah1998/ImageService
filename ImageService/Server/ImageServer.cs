using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Net.Sockets;
using System.Net;

namespace ImageService.Server
{
    /// <summary>
    /// Server
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<DirectoryCloseEventArgs> server_close;
        #endregion

        #region serverClient
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        #endregion

        /// <summary>
        /// ImageServer is the server that in charge on all the Handlers.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logging"></param>
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            // intilaize Server's controller and logger.
            this.m_controller = controller;
            this.m_logging = logging;
            // get all directories path
            string[] paths = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string path in paths)
            {
                // handler creation
                IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_logging, this.m_controller, path);
                CommandRecieved += directoryHandler.OnCommandRecieved;
                this.server_close += directoryHandler.closeHandler;
                directoryHandler.StartHandleDirectory(path);
                this.m_logging.Log("Create handler for path - " + path, Logging.Modal.MessageTypeEnum.INFO);
            }

        }
        /// <summary>
        /// start connection with clients
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            // searching for clients
            listener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            listener.Stop();
        }


        /// <summary>
        ///  waht happend when Server close.
        /// </summary>
        public void OnCloseServer()
        {
            try
            {
                server_close.Invoke(this, null);
            }
            catch (Exception ex)
            {
                this.m_logging.Log("Close Server ERROR", Logging.Modal.MessageTypeEnum.FAIL);
            }
        }
    }
}
