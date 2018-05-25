﻿using ImageService.Controller.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
   public interface IClientHandler
    {
        string HandleClient(TcpClient client);
    }
}
