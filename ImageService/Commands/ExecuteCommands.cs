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
    /// <summary>
    /// defines how the server will handle the commands
    /// </summary>
    class ExecuteCommands : IExecuteCommand
    {

        ILoggingService log;

        /// <summary>
        /// getting the log in order to write to it
        /// </summary>
        /// <param name="log"></param>
        public ExecuteCommands(ILoggingService log)
        {
            this.log = log;
        }


        /// <summary>
        /// handling the request
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="info"></param>
        /// <param name="client"></param>
        /// <param name="writer"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
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
                this.log.Log((info+" was deleted!"), Logging.Modal.MessageTypeEnum.INFO);
                JObject objToSend = new JObject();
                objToSend["res"] = "1";
                writer.Write(JsonConvert.SerializeObject(objToSend));
                //removing
                return info;
            }

            if (commandLine == "5")
            {

               
                    JObject objToSend = new JObject();
                objToSend["moved"] =movedPics.Instance.Amount;
                writer.Write(JsonConvert.SerializeObject(objToSend));
                return "5";
            }




                if (commandLine == "2")
            {
                JObject objToSend = new JObject();
                objToSend["2"] = LogListSIngleton.Instance.getListAsString();
                writer.Write(JsonConvert.SerializeObject(objToSend));
                return "2";
            }

            else { return null; }
        }
    }
}
