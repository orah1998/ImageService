﻿using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private string[] endings = { ".gif", ".jpg", ".png", ".bmp" };
        #endregion

        #region events
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        
        public DirectoyHandler(ILoggingService logging, IImageController controller,string path)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_path = path;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
        }



        // The Function Recieves the directory to Handle
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("now using StartHandleDirectory function" + " " + dirPath, MessageTypeEnum.INFO);
            this.m_dirWatcher.Created += new FileSystemEventHandler(watchCreated);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(watchModified);
            //start listen to directory
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("Starting to handle directory: " + dirPath, MessageTypeEnum.INFO);


        }




        public void watchModified(object sender, FileSystemEventArgs e)
        {
            string[] files;
            this.m_logging.Log("Entered watchModified looking at file: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath);

            if (this.endings.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, "");
                this.OnCommandRecieved(this, commandRecievedEventArgs);

            }
        }











        public void watchCreated(object sender, FileSystemEventArgs e)
        {
            this.m_logging.Log("Enterd watchCreated looking at file: " + e.FullPath, MessageTypeEnum.INFO);
            string extension = Path.GetExtension(e.FullPath);

            if (this.endings.Contains(extension))
            {
                string[] args = { e.FullPath };
                CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs( (int)CommandEnum.NewFileCommand , args  ,  "");
                this.OnCommandRecieved(this, commandRecievedEventArgs);

            }
        }





        // The Event that will be activated upon new Command
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            // execute the command
            string ans = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            //informing the result to the log
            if (result)
            {

                this.m_logging.Log(ans, MessageTypeEnum.INFO);
            }
            else
            {
                this.m_logging.Log(ans, MessageTypeEnum.FAIL);
            }

        }


        public void Activate()
        {

        }
    }
}
#endregion