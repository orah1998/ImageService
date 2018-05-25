using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// what command we use
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand=0,
        GetConfigCommand=1,
        LogCommand=2,
        CloseCommand=3
    }
}
