using ImageService.Controller.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
   public class ClientHandler : IClientHandler
    {
        string toRemove;
        Dictionary<string, IDirectoryHandler> dic;
        

        public void addDic(Dictionary<string, IDirectoryHandler> dic)
        {
            this.dic = dic;
        }


        public string HandleClient(TcpClient client, Dictionary<string, IDirectoryHandler> dic)
        {
            string result;
            using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                    Console.WriteLine("Got command: {0}", commandLine);
                    result = ExecuteCommand(commandLine, client,writer,reader);
                }
                client.Close();
            return result;
        }

        private string ExecuteCommand(string commandLine, TcpClient client, StreamWriter writer,StreamReader reader)
        {
            if (commandLine == "AppConfig")
            {
                JObject objToSend = new JObject();
                string[] list=ConfigurationManager.AppSettings.AllKeys;
                JObject obj = new JObject();
                foreach (string item in list)
                {
                    //sending as : key:information
                    objToSend[item] = ConfigurationManager.AppSettings.Get(item);
                   // Handler" value="C: \Users\Operu\Desktop\mip"/>
                   //"OutputDir" value = "C:\Users\Operu\Desktop\dest" />
                   //"SourceName" value = "ImageServiceSource" />
                   //"LogName" value = "ImageServiceLog" />
                   // "ThumbnailSize"


                }
                writer.Write(JsonConvert.SerializeObject(objToSend));
                return null;
            }
            else
            {
                toRemove = reader.ReadLine();
                //removing
                return toRemove;
            }


        }
    }
}
