using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// event args
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        private MessageTypeEnum msg_status;
        private string msg;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            this.msg_status = status;
            this.msg = message;
        }
        /// <summary>
        /// 
        /// </summary>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
