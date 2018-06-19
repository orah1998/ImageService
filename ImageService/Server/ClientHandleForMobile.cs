using ImageService.Logging;
using ImageService.Logging.Modal;
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
    class ClientHandleForMobile : IClientHandler
    {
        private ILoggingService m_logging;
        
        public ClientHandleForMobile(ILoggingService logging)
        {
            this.m_logging = logging;
        }

        
        public string HandleClient(TcpClient client)
        {
                new Task(() =>
                {
                    using (NetworkStream stream = client.GetStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        m_logging.Log("handle mobile client", MessageTypeEnum.INFO);
                        string fileName = GetFileName(stream);
                        Byte[] b = new Byte[1];
                        b[0] = 1;
                        stream.Write(b, 0, 1);
                        

                        byte[] photoArr = GetPhoto(stream);
                        

                        File.WriteAllBytes(HandlerSingleton.getList()[0] +"\\" + fileName + ".jpg", photoArr);

                        stream.Write(b, 0, 1);
                        System.Threading.Thread.Sleep(500);
                    }
                }).Start();

            return null;
        }




        public String GetFileName(NetworkStream stream)
        {
            List<Byte> byteList = new List<Byte>();
            Byte[] b = new Byte[1];
            do
            {
                stream.Read(b, 0, 1);
                byteList.Add(b[0]);
            } while (stream.DataAvailable);


            return Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(byteList.ToArray()));
        }




        public byte[] GetPhoto(NetworkStream stream)
        {
            int i = 0;
            List<Byte> byteList = new List<Byte>();
            Byte[] b = new Byte[1];
            do
            {
                i = stream.Read(b, 0, b.Length);
                byteList.Add(b[0]);
            } while (stream.DataAvailable);
            return byteList.ToArray();
        }






    }
}
