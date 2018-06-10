using ImageService.Commands;
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
using static ImageService.Commands.Delegates;

namespace ImageService.Server
{
    /// <summary>
    /// sending the client to a certain format of executing
    /// </summary>
    public class ClientHandler : IClientHandler
    {
       
        public event DeleteFolder handle;
        string toRemove;
        Dictionary<string, IDirectoryHandler> dic;
        private IExecuteCommand exec;

        public ClientHandler(IExecuteCommand exec,DeleteFolder handl)
        {
            this.handle = handl;
            this.exec = exec;
        }

        /// <summary>
        /// creating a task for the recieved client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string HandleClient(TcpClient client)
        {
            string result="";
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    string commandLine = reader.ReadString();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(commandLine);
                    try {
                        using (StreamWriter sw = File.AppendText(@"C: \Users\Operu\Desktop\testing\info.txt"))
                        {
                            sw.WriteLine(obj["inst"].ToString()+" "+ obj["etc"].ToString());
                        }
                        result = exec.ExecuteCommand(obj["inst"].ToString(), obj["etc"].ToString(), client, writer, reader);
                    }catch(Exception e)
                    {
                        result = exec.ExecuteCommand(obj["inst"].ToString(), null, client, writer, reader);
                    }
                    this.handle?.Invoke(result);


                }
                client.Close();
            }).Start();
            return result;
        }
    }
}
