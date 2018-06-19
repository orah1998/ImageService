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
                        //getting the picture's name
                        string fileName = GetName(stream);
                        Byte[] b = new Byte[1];
                        b[0] = 1;
                        //informing the client to send the next stream, which is the picture itself.
                        stream.Write(b, 0, 1);
                        
                        //getting the byte stream of the picture
                        byte[] photoArr = GetPhotoBytes(stream);
                        
                        //writing the bytes into a picture
                        File.WriteAllBytes(HandlerSingleton.getList()[0] +"\\" + fileName + ".jpg", photoArr);

                        //informing the end of this picture information
                        stream.Write(b, 0, 1);
                        System.Threading.Thread.Sleep(500);
                    }
                }).Start();

            return null;
        }




        public String GetName(NetworkStream stream)
        {
            List<Byte> byteList = new List<Byte>();
            Byte[] buff = new Byte[1];
            do
            {
                stream.Read(buff, 0, 1);
                byteList.Add(buff[0]);
            } while (stream.DataAvailable);


            return Path.GetFileNameWithoutExtension(System.Text.Encoding.UTF8.GetString(byteList.ToArray()));
        }




        public byte[] GetPhotoBytes(NetworkStream stream)
        {
            int i = 0;
            List<Byte> byteList = new List<Byte>();
            Byte[] buff = new Byte[1];
            do
            {
                i = stream.Read(buff, 0,1);
                byteList.Add(buff[0]);
            } while (stream.DataAvailable);
            return byteList.ToArray();
        }






    }
}
