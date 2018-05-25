using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageService.Commands.Delegates;

namespace ImageService.Server
{
    class HandlerSingleton
    {

        public event DeleteFolder removedItem;

        private static HandlerSingleton instance = null;
        private static readonly object padlock = new object();
        private static List<string> list;

        HandlerSingleton()
        {
            list = new List<string>();
            removedItem +=removeItem;
        }

        public static HandlerSingleton Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new HandlerSingleton();
                }
                return instance;

            }
        }
        
        public DeleteFolder GetDelegate()
        {
            return new DeleteFolder(removeItem);
        }


        public static void addItem(string val)
        {
            list.Add(val);
        }

        public static void removeItem(string val)
        {
            list.Remove(val);
        }


        public static string getListAsString ()
        {
            string ret = "";
            foreach(string item in list)
            {
                ret +=item+";";
            }
            return ret;
        }







    }
}
