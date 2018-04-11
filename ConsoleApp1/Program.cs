using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageServiceModal moda = new ImageServiceModal();
            moda.outputFolderSet("C:\\Users\\Operu\\Desktop\\mip\\untitiled.png");
        }
    }
}
