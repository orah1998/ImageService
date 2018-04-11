﻿using ImageService.Controller;
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

        private List<IDirectoryHandler> handlerList;
            #endregion
        public ImageServer(IImageController controller,ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;

            string[] paths = ConfigurationManager.AppSettings.Get("Handler").Split(';');
            foreach(string path in paths)
            {
                IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_logging, this.m_controller);
                this.handlerList.Add(directoryHandler);
                directoryHandler.Activate(); 
            }
        }
    }
}
