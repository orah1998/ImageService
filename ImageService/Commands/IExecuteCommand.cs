using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface IExecuteCommand
    {
        string ExecuteCommand(string commandLine,string info, TcpClient client, BinaryWriter writer, BinaryReader reader);
    }
}
