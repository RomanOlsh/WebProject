using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace mySite.Models.DataModels
{
    public class FileManipulations
    {
        public static int fileMaxSize = 1024 * 850;

        //Get folder path:
        public static string getStoreagefolder()
        {
            return HttpContext.Current.Server.MapPath("~/Uploads") + "\\";
        }

        //Gett all existing images:
        public static List<string> getAllImages()
        {
            List<string> listOfImages = null;
            try
            {
                string[] vec = Directory.GetFiles(getStoreagefolder(), "*.jpg");
                for (int i = 0; i < vec.Length; i++)
                    vec[i] = vec[i].Remove(0, vec[i].IndexOf(@"\Uploads"));
                listOfImages = vec.ToList<string>();
            }
            catch
            {
                listOfImages = null;
            }
            return listOfImages;
        }

        //Add new file:
        public static bool addFile(HttpPostedFileBase fileToAdd)
        {
            bool result = false;
            try
            {
                fileToAdd.SaveAs(getStoreagefolder() + Path.GetFileName(fileToAdd.FileName));
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        //Check if file exists:
        public static bool isExist(string originalflnm)
        {
            string fullfilename = getStoreagefolder() + Path.GetFileName(originalflnm);
            return File.Exists(fullfilename);
        }

        //Delete given file:
        public static bool deleteFile(string filename)
        {
            bool result = false;
            try
            {
                File.Delete(getStoreagefolder() + filename);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
   
        
    
    }
}