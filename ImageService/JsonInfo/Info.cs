using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.JsonInfo
{
    public class Info
    {
        // 1=handler(listened folders) 2=output_folder 3=sourceName 4=LogName 5=thumbnail_Size  6=removed folder
        public Info(int command, string data)
        {
            this.Command = command;
            this.Data = data;
        }
        public int Command { get; set; }
        public string Data { get; set; }

    }
}

