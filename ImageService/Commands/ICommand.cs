using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// commands interface
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// execute of command
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);          // The Function That will Execute The 
    }
}
