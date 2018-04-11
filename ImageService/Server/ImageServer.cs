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

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved

            #endregion
        public ImageServer(IImageController controller,ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;

            string[] paths = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach(string path in paths)
            {
                // handler creation
                IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_logging, this.m_controller, path);
                CommandRecieved += directoryHandler.OnCommandRecieved;
                directoryHandler.StartHandleDirectory(path);
                this.m_logging.Log("Create handler for path - " + path,Logging.Modal.MessageTypeEnum.INFO);
            }
        
        }


        public void OnCloseServer()
        {

        }



    }
}
