using ImageService.Controller.Handlers;
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

        public ClientHandler()
        {

        }

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
                string[] list=ConfigurationManager.AppSettings.AllKeys;
                foreach (string item in list)
                {
                    //sending as : key:information
                    writer.Write(item+":"+ConfigurationManager.AppSettings.Get(item));
                }
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
