using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO.Compression;

namespace File_Manager.General
{
    public static class Organizer
    {
        
        public  static void createTimestampFolders(String sourceDirectory, String targetDirectory, String folderFormat, String fileType)
        {
            DirectoryInfo d = new DirectoryInfo(sourceDirectory);
            FileInfo[] files = d.GetFiles(String.Format("*{0}", fileType)); //Getting Text files
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            String folderStringFormat = String.Empty;
            sourceDirectory = String.Format(@"{0}{1}", sourceDirectory, "temp");

            files = files.OrderBy(f => f.Name).ToArray();


            bool isNotFirst = false;

           

            foreach (FileInfo file in files)
            {
                if (Regex.IsMatch(file.Name, folderFormat) == true)
                {
                    Regex rege = new Regex(String.Format("{0}", folderFormat));
                    var results = rege.Matches(file.Name);
                    DateTime time = new DateTime();
                   
                    String folderName = String.Empty;
                    DateTime.TryParseExact(String.Format("{0}", results[0].Groups[1]), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);

                    folderName = time.Date.ToString("MM-dd-yyyy");

                    if (!isNotFirst) { startDate = time; isNotFirst = true; };

                    if (!System.IO.Directory.Exists(String.Format(@"{0}\{1}", sourceDirectory, time.Date.ToString())))
                    {
                        Directory.CreateDirectory(String.Format(@"{0}\{1}", sourceDirectory, folderName));
                    }

                    if (!Directory.Exists(String.Format(@"{0}\{1}\{2}", sourceDirectory, folderName, file.Name)))
                    {

                        File.Move(file.FullName, (String.Format(@"{0}\{1}\{2}", sourceDirectory, folderName, file.Name)));
                    }

                    endDate = time;

                }



            }



            if (startDate != endDate) { folderStringFormat = "{0}/{1}_{2}.zip"; } else { folderStringFormat = "{0}/{1}.zip"; };



            compressTargetFolder(folderStringFormat, sourceDirectory, targetDirectory, startDate, endDate);
            d = new DirectoryInfo(sourceDirectory); // cleanup
            d.Delete(true);

        }

        public static void compressTargetFolder(String folderStringFormat, String zipDirectory,String targetDirectory, DateTime startDate, DateTime endDate)
        {

            ZipFile.CreateFromDirectory(zipDirectory, String.Format(folderStringFormat, targetDirectory, startDate.ToString("MM-dd-yyyy"), endDate.ToString("MM-dd-yyyy")));
           
        }

        private  static void deleteFolderContents(String directory)
        {
            DirectoryInfo dInfo = new System.IO.DirectoryInfo(directory);
            foreach (System.IO.DirectoryInfo subDirectory in dInfo.GetDirectories())
                subDirectory.Delete(true);
        }
        

    }
}
