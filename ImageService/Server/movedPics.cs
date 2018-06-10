using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    //singleton for remembering how much pictures were handled
    class movedPics
    {
        public movedPics()
        {
            this.Amount = 0;
        }

        private static movedPics instance = null;
        public int Amount { get; set; }

        public static movedPics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new movedPics();
                }
                return instance;

            }
        }



        public FileSystemEventHandler AddItem()
        {
            this.Amount++;
            return null;
        }
    }
}
