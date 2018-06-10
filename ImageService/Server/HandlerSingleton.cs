using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageService.Commands.Delegates;

namespace ImageService.Server
{
    /// <summary>
    /// a list that holds all the current directory handlers that are still working
    /// </summary>
    class HandlerSingleton
    {
        private static HandlerSingleton instance = null;
        private static List<string> list;

        HandlerSingleton()
        {
            list = new List<string>();
        }

        /// <summary>
        /// instance of it
        /// </summary>
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
           

            if (ret == "")
            {
                ret = "NONE";
            }
            else
            {
                ret = ret.Remove(ret.Length - 1);
            }
            return ret;
        }







    }
}
