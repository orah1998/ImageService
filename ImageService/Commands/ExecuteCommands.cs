using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Newtonsoft.Json;
using ImageService.Logging;
using ImageService.Infrastructure.Enums;
using ImageService.Server;

namespace ImageService.Commands
{
    class ExecuteCommands : IExecuteCommand
    {

        ILoggingService log;

        public ExecuteCommands(ILoggingService log)
        {
            this.log = log;
        }



        public string ExecuteCommand(string commandLine,string info, TcpClient client, BinaryWriter writer, BinaryReader reader)
        {
            string toRemove;
            if (commandLine == "1")
            {
                JObject objToSend = new JObject();
                string[] list = ConfigurationManager.AppSettings.AllKeys;
                JObject obj = new JObject();
                foreach (string item in list)
                {
                    if (item != "Handler") { 
                    objToSend[item] = ConfigurationManager.AppSettings.Get(item);
                    }
                }
                objToSend["Handler"] =HandlerSingleton.getListAsString();

                writer.Write(JsonConvert.SerializeObject(objToSend));
                string toSend = "message code " + commandLine + " " + CommandEnum.GetConfigCommand + " was Successful with Result:" + JsonConvert.SerializeObject(objToSend);
                this.log.Log(toSend, Logging.Modal.MessageTypeEnum.INFO);

                using (StreamWriter outputFile = File.AppendText(@"C:\Users\Operu\Desktop\testGui\GUI.txt"))
                {
                    outputFile.WriteLine("about to send 1");
                }

                return "1";
            }
            if(commandLine=="3")
            {
                using (StreamWriter outputFile = File.AppendText(@"C:\Users\Operu\Desktop\testGui\GUI.txt"))
                {
                    outputFile.WriteLine("inside commandline 3!!!");
                }

                //removing
                return info;
            }
            else { return null; }
        }
    }
}
