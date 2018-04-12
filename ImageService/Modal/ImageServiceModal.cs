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


                DateTime dt = File.GetCreationTime(path);
                year=dt.Year.ToString();
                month= dt.Month.ToString();
                DirectoryInfo outputFold = Directory.CreateDirectory(m_OutputFolder);
                outputFold.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

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

                    string destPathThumb = m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path);

                    createByYearAndMonth(m_OutputFolder + "\\" + "Thumbnails", year, month);


                    if (File.Exists(destPathThumb))
                    {
                        destPathThumb = PathForSameName(m_OutputFolder + "\\" + year + "\\" +month,
                            m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                    }

                    Image thumbnail = Image.FromFile(destPath);
                    thumbnail = (Image)(new Bitmap(thumbnail, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
                    thumbnail.Save(destPathThumb);
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