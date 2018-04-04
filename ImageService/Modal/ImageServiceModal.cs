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
            try { 
            //checking if we got a valid path
            if (File.Exists(path))
            {
                //getting the date of the file(its creation)
                DateTime dt = File.GetCreationTime(path);
                year = dt.Year.ToString();
                month = dt.Month.ToString();
                //getting to the given folder
                DirectoryInfo output = Directory.CreateDirectory(m_OutputFolder);
                //creating subdirectory for the thumbnails
                Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                //hiding the Output Folder
                output.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                bool outputfold = CreateDateFolders(m_OutputFolder, year, month);
                bool thumbnailfold = CreateDateFolders(m_OutputFolder + "\\" + "Thumbnails", year, month);
                if((outputfold & thumbnailfold) != true)
                {
                    throw new Exception("Error : folders could not be created");
                }


                    string funcResult = "";

                    string targetFold = m_OutputFolder + "\\" + year + "\\" + month + "\\";
                    //checking if we already added the file to out directory
                    if (File.Exists(targetFold + Path.GetFileName(path))==false)
                    {
                        //if not , we will copy it to out directory
                        File.Copy(path, targetFold + Path.GetFileName(path));
                        funcResult+="Added " + Path.GetFileName(path) + " to " + targetFold;
                    }

                    // creating the thumbnail photo
                    if (File.Exists((m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path)))==false)
                    {
                        Image thumb = Image.FromFile(path);
                        thumb = (Image)(new Bitmap(thumb, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
                        thumb.Save(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));
                        funcResult+= " and added thumb " + Path.GetFileName(path);
                    }
                    result = true;
                    return funcResult;






                }
                else
                {
                    throw new Exception("file does not exist");
                } 


            } catch(Exception e)
            {
                result=false;
                return e.ToString();
            }
        }




        private bool CreateDateFolders(string path, string year, string month)
        {
            try
            {
                Directory.CreateDirectory(path + "\\" + year + "\\" + month);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
#endregion