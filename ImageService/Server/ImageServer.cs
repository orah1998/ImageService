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
using System.IO;
using ImageService.Commands;
using static ImageService.Commands.Delegates;

namespace ImageService.Server
{
    /// <summary>
    /// Server
    /// </summary>
    public class ImageServer
    {


        public event DeleteFolder handle;

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
        Dictionary<string, IDirectoryHandler> dic=new Dictionary<string, IDirectoryHandler>();
        #endregion

        /// <summary>
        /// ImageServer is the server that in charge on all the Handlers.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="logging"></param>
        public ImageServer(IImageController controller, ILoggingService logging, int port)
        {
            



            this.port = port;
            //function to remove handler
            this.handle += this.handling;
            //function to inform our list of handlers that a handler was removed:
            this.handle+=HandlerSingleton.Instance.GetDelegate();


            // intilaize Server's controller and logger.
            this.m_controller = controller;
            this.m_logging = logging;


            //we will do the mobile server communication as a task in order to prevent damage
            //to the reset of the functionality of the code.
            new Task(() =>
            {
                MobileServer serv = new MobileServer(this, this.m_controller, this.m_logging, 8234);
                serv.Start();
            }).Start();



            this.ch = new ClientHandler(new ExecuteCommands(this.m_logging), this.handle);
            // get all directories path
           
            string[] paths = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach (string path in paths)
            {
                // handler creation
                IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_logging, this.m_controller, path);
                CommandRecieved += directoryHandler.OnCommandRecieved;
                this.server_close += directoryHandler.closeHandler;
                directoryHandler.StartHandleDirectory(path);
                //adding the handler to our dictary so we can close it when we are told to by the GUI
                dic.Add(path, directoryHandler);
                HandlerSingleton.addItem(path);
                this.m_logging.Log("Create handler for path - " + path, Logging.Modal.MessageTypeEnum.INFO);

            }
           

            //after looping through the folders:


            this.m_logging.Log("starting to listen", Logging.Modal.MessageTypeEnum.INFO);

            

            Start();

        }
        
        
        /// <summary>
        /// start connection with clients
        /// </summary>
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            listener = new TcpListener(ep);
            
            // searching for clients
            listener.Start();
            //connecting with every GUI's Settings Model
                Task task = new Task(() =>
            {
                while (true)
                {
                        try
                        {
                        
                        TcpClient client = listener.AcceptTcpClient();
                        using (StreamWriter sw = File.AppendText(@"C: \Users\Operu\Desktop\testing\info.txt"))
                        {
                            sw.WriteLine("accepted");
                        }
                        string toRemove = ch.HandleClient(client);
                    }
                        catch (SocketException)
                        {
                            break;
                        }
                }
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
        ///  what happend when Server close.
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




    public void handling(string str)
        {

            if (str != null) {
                if (dic[str]!= null)
                {
                    
                    dic[str].closeHandler(this,null);
                    
                }
            }




            
        }





    }
}
