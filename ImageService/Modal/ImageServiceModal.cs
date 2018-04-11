using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size



        public string outputFolder
        {
            get
            {
                return this.m_OutputFolder;
            }
            set
            {
                this.m_OutputFolder = value;
            }
        }

      
        public int thumbnailSize
        {
            get
            {
                return this.m_thumbnailSize;
            }
            set
            {
                this.m_thumbnailSize = value;
            }
        }



        public string AddFile(string path, out bool result)
        {
            string year;
            string month;
            try
            {
                DateTime dt = File.GetCreationTime(path);
                year=dt.Year.ToString();
                month= dt.Month.ToString();
                DirectoryInfo outputFold = Directory.CreateDirectory(m_OutputFolder);
                
                if(createByYearAndMonth(year, month) != false)
                {
                    string destPath = m_OutputFolder + "\\" + year + "\\" + month + "\\" +Path.GetFileName(path);
                    File.Move(path, destPath);

                }
                else
                {
                    throw new Exception("EORROR: cant create folders");
                }



            }
            catch(Exception e)
            {
                result = false; 
                return e.ToString();
            }








        }




        //    string year;
        //    string month;
        //    try { 
        //    //checking if we got a valid path
        //    if (File.Exists(path))
        //    {
        //        //getting the date of the file(its creation)
        //        DateTime dt = File.GetCreationTime(path);
        //        //year and month holding the dates of the pictures
        //        year = dt.Year.ToString();
        //        month = dt.Month.ToString();
        //        //getting to the destination folder
        //        DirectoryInfo output = Directory.CreateDirectory(m_OutputFolder);
        //        //creating subdirectory for the thumbnails OR getting the path if it was already created
        //        Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
        //        //hiding the Output Folder
        //        output.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        //        bool outputfold = CreateDateFolders(m_OutputFolder, year, month);
        //            //creating the folders with thumbnails folder before date folders
        //        bool thumbnailfold = CreateDateFolders(m_OutputFolder + "\\" + "Thumbnails", year, month);
        //        // if both regular folders and thumbnail folders were not created corrently :
        //        if((outputfold & thumbnailfold) != true)
        //        {
        //            throw new Exception("Error : folders could not be created");
        //        }


        //        string funcResult = "";

        //        string targetFold = m_OutputFolder + "\\" + year + "\\" + month + "\\";
        //        string FileNewPath=targetFold+"\\"+






        //}



        public bool createByYearAndMonth(string year,string month)
        {
            try { 
            DirectoryInfo output = Directory.CreateDirectory(m_OutputFolder+"\\"+year);
            }catch(Exception e1) {
                return false;
            }

            try
            {
                DirectoryInfo output = Directory.CreateDirectory(m_OutputFolder + "\\" + year+"\\"+month);
            }
            catch (Exception e1)
            {
                return false;
            }

            return true;
        }




    }
}
#endregion