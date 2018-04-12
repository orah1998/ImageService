using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal"></param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
        /// <summary>
        /// Execute to the command
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result">if worked</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                String ret = m_modal.AddFile(args[0], out result);
                if (!result)
                {
                    throw new Exception("ERROR at exectue");
                }
                return ret;
            }
            catch(Exception e)
            {
                result = false; 
                return e.ToString();
            }
        }
    }
}
