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



        public string OutputFolder
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


        /// <summary>
        /// this function adds a file to our desntination folder
        /// </summary>
        /// <param name="path"></param>
        /// the path of the file thats needs to be added
        /// <param name="result"></param>
        /// in result, we will return true if the function was successful and false if it was not.
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            string answer="";
            string year;
            string month;
            try
            {
                //if the file doesnt exist we want to throw a new exception
                if (!File.Exists(path))
                {
                    throw new Exception("file does not exist !!!");
                }

                //getting the date of the creation time of the file that needs to be moved to our dest folder
                DateTime dt = File.GetCreationTime(path);
                year=dt.Year.ToString();
                month= dt.Month.ToString();
                //getting to the dest folder and making it hidden
                DirectoryInfo outputFold = Directory.CreateDirectory(m_OutputFolder);
                outputFold.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                // if we could not create year and month folder according to the file's creation
                if (createByYearAndMonth(m_OutputFolder,year, month) != false)
                {
                    string destPath = m_OutputFolder + "\\" + year + "\\" + month + "\\" +Path.GetFileName(path);
                    if (File.Exists(destPath))
                    {
                        destPath = PathForSameName(path,destPath);
                    }
                        File.Move(path, destPath);
                    result = true; 
                    answer+="copy item to destination folder. ";

                    //path to the thumbnail folder
                    string destPathThumb = m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path);
                    //creating thumbnail folders
                    createByYearAndMonth(m_OutputFolder + "\\" + "Thumbnails", year, month);


                    if (File.Exists(destPathThumb))
                    {
                        destPathThumb = PathForSameName(m_OutputFolder + "\\" + year + "\\" +month,
                            m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                    }




                    //creating the thumbnail photo.
                    
                    using (Image image = Image.FromFile(destPath))
                    using (Image thumbnail = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero))
                    {
                        thumbnail.Save(destPathThumb);
                       
                        image.Dispose();
                    }
                


                answer += " thumbnail added as well." + Path.GetFileName(path);
                    result = true;
                    return answer;

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
       

        /// <summary>
        /// we will use this function to create the required folders for the dates of the file to be moved
        /// </summary>
        /// <param name="outputFold"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool createByYearAndMonth(string outputFold,string year,string month)
        {
            try { 
            DirectoryInfo output = Directory.CreateDirectory(outputFold + "\\"+year);
            }catch(Exception e1) {
                return false;
            }

            try
            {
                DirectoryInfo output = Directory.CreateDirectory(outputFold + "\\" + year+"\\"+month);
            }
            catch (Exception e1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// this function will rename a file that needs to be moved and has the same name as another file which is already in the dest folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="outputFold"></param>
        /// <returns></returns>
        public string PathForSameName(string path,string outputFold)
        {
            int counter = 0;
            //if a file with the same name already exists in the destination folder

                string ans="";
            counter++;

                while(File.Exists(outputFold + "\\" + Path.GetFileNameWithoutExtension(path)+"("+counter.ToString()+")"+Path.GetExtension(path))  )
                {
                    counter++;
                    ans=outputFold + "\\" + Path.GetFileNameWithoutExtension(path) + "(" + counter.ToString() + ")" + Path.GetExtension(path);
                }

                return ans;
        }





    }
}
#endregion