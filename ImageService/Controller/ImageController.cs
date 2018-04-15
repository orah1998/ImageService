using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// controller
    /// </summary>
    public class ImageController : IImageController
    {

        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal"></param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                {((int)CommandEnum.NewFileCommand) ,new NewFileCommand(this.m_modal) }
            };
        }
        /// <summary>
        /// Execute to the specific command
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="resultSuccesful"></param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            return this.commands[commandID].Execute(args,out resultSuccesful);
        }
    }
}
