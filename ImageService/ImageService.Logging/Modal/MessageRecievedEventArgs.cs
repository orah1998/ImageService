using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        private MessageTypeEnum msg_status;
        private string msg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            this.msg_status = status;
            this.msg = message;
        }

        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }



    }
}
