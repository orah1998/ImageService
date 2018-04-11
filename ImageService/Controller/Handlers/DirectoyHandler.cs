using ImageService.Modal;
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
        #endregion

        public DirectoyHandler(IImageController m_controller,
            ILoggingService m_logging,string m_path)
        {
            this.m_controller = m_controller;
            this.m_path = m_path;
            this.m_logging = m_logging;
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
        }



        // The Function Recieves the directory to Handle
        public void StartHandleDirectory(string dirPath)
        {
            m_logging.Log("now using StartHandleDirectory function" + " " + dirPath, MessageTypeEnum.INFO);
            // add all images in the directory to the output directory.
            string[] images = Directory.GetFiles(m_path);
            foreach (string filepath in images)
            {
                m_logging.Log("StartHandleDirectory" + " " + filepath, MessageTypeEnum.INFO);
                string end = Path.GetExtension(filepath);
                if (this.endings.Contains(end))
                {
                    string[] args = { filepath };
                    //if the picture we found had the ending that we were looking for:
                    OnCommandRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                        args, filepath));
                }
            }
            this.m_dirWatcher.Created += new FileSystemEventHandler(M_dirWatcher_Created);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(M_dirWatcher_Created);
            //start listen to directory
            this.m_dirWatcher.EnableRaisingEvents = true;
            this.m_logging.Log("Start handle directory: " + dirPath, MessageTypeEnum.INFO);


        }





        // The Event that will be activated upon new Command
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {


        }

    }
}
