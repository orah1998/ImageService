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

            if (commandLine == "6")
            {
                JObject objToSend = new JObject();
                objToSend["Handler"] = HandlerSingleton.getListAsString();
                writer.Write(JsonConvert.SerializeObject(objToSend));
                return "6";
            }


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

                return "1";
            }
            if(commandLine=="3")
            {
                //removing
                return info;
            }
            else { return null; }
        }
    }
}
