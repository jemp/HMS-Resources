using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO.Compression;
using System.Threading;
using File_Manager.Diagnostics;

namespace File_Manager.General
{
    public static class Organizer
    {
        private const String zipDumpTitle = "zip_Dump";
        /// <summary>
        /// This function is designed to do the following:
        /// 1. Grab containing folder from Directory Target
        /// 2. Grab Directory Source from Directory Target
        /// 3. Zip contents from Directory source, outputting resulting ZIP file to directory target
        /// 4. Cleanup files that have been compressed
        /// </summary>
        /// <param name="zipDirectoryTarget">Target of which contains folders that need compressed</param>
        public static void compressAndRemoveTargetFolder(String zipDirectoryTarget)
        {

            try
            {
                ///First, Let's get the containing folder, than the location in where the files need to go
                String zipContainingFolder = Path.GetDirectoryName(zipDirectoryTarget);
                String zipDirectorySource = String.Format(@"{0}\{1}", zipContainingFolder, zipDumpTitle);

                ///Let's zip it up!
                ZipFile.CreateFromDirectory(zipDirectorySource, zipDirectoryTarget);
                ///Housecleaning: Remove folders that were compressed
                Directory.Delete(zipDirectorySource, true);
            }

            catch(IOException e)
            {
                throw new OrganizerException("Exception encountered while compressing archived folders", e.InnerException);
            }

            catch (ArgumentException e)
            {
                throw new OrganizerException("Exception encountered while processing arguements for archive folder compression", e.InnerException);
            }
        }

        /// <summary>
        /// This function organizes a set ammount of files by their date, then creates folders based on the day that they were created
        /// </summary>
        /// <param name="sourceDirectory">Where are the files located at?</param>
        /// <param name="targetDirectory">Where are the daily folders going to be put at?</param>
        /// <param name="folderFormat">In Regex: the format in which the date will be extracted</param>
        /// <param name="fileType">What file types are we looking for?</param>
        /// <returns></returns>
        public static String createTimestampFolders(String sourceDirectory, String targetDirectory, String folderFormat, String fileType)
        {
            ///Let's set up the neccesary file statistics
            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] sourceFiles = sourceInfo.GetFiles(String.Format("*{0}", fileType)); //Getting Text files

            ///Initialize the start and end date based on minumum and maximum date
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            ///Initilize folder string format
            String folderStringFormat = String.Empty;

            ///The temperary directory folder based on the source directory name
            String dynamicTempDirectory = getTempFolderPath(sourceDirectory, targetDirectory);

            ///Temporary "zipdump" folder name
            String zipDump = String.Format(@"{0}\{1}", dynamicTempDirectory, zipDumpTitle);

            ///Let's story the source files, based on name (This will tidy up the start / end date)
            sourceFiles = sourceFiles.OrderBy(f => f.Name).ToArray();

            ///Initialize isNotFirst Bool, will indicate if it's the first file in teh loop
            bool isNotFirst = false;

            try
            {
                ///Looping through all the files in source...
                foreach (FileInfo file in sourceFiles)
                {

                    ///Has the file matched our requested name format?
                    if (Regex.IsMatch(file.Name, folderFormat) == true)
                    {

                        Regex rege = new Regex(String.Format("{0}", folderFormat)); ///Name format for the file
                        var results = rege.Matches(file.Name); 
                        DateTime time = new DateTime();

                        String folderName = String.Empty;
                        DateTime.TryParseExact(String.Format("{0}", results[0].Groups[1]), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out time); ///Let's extract the date

                        folderName = time.Date.ToString("MM-dd-yyyy"); ///Parsing out the date from the file

                        if (!isNotFirst) { startDate = time; isNotFirst = true; }; ///If it's the first file, set the start date to the date in the title


                        ///Does the folder exist with the same date? If yes, create the folder
                        if (!System.IO.Directory.Exists(String.Format(@"{0}\{1}", zipDump, time.Date.ToString())))
                        {
                            Directory.CreateDirectory(String.Format(@"{0}\{1}", zipDump, folderName));
                        }

                        ///Aslong as the file doesn't exist in the destination, copy it!
                        if (!Directory.Exists(String.Format(@"{0}\{1}\{2}", zipDump, folderName, file.Name)))
                        {

                            File.Copy(file.FullName, (String.Format(@"{0}\{1}\{2}", zipDump, folderName, file.Name)));

                        }

                        endDate = time; ///Finally, set the end date to the last folder's titled date
                    }


                    ///If there is more than one date, generate zip format with one date. Otherwise, give the range of two dates in title
                    if (startDate != endDate) { folderStringFormat = @"{0}\{1}_{2}.zip"; } else { folderStringFormat = @"{0}\{1}.zip"; };

                    
                }

                return String.Format(folderStringFormat, dynamicTempDirectory, startDate.ToString("MM-dd-yyyy"), endDate.ToString("MM-dd-yyyy"));
            }
            catch(ArgumentException e)
            {
                throw new OrganizerException("Exception encountered while processessing files for timestamp folders",e.InnerException);
            }
            catch (FormatException e)
            {
                throw new OrganizerException("Exception encountered  while processessing files for timestamps", e.InnerException);
            }
            catch (IOException e)
            {
                throw new OrganizerException("Exception encountered  while processing folders/files for timestamp folders", e.InnerException);
            }

            catch (Exception e)
            {
                throw new OrganizerException("Exception encountered  while running createTimestampFolders", e.InnerException);
            }





        }

        /// <summary>
        /// Will generate a Temparary file path based on paramters
        /// </summary>
        /// <param name="sourceDirectory">Where is the source at?</param>
        /// <param name="targetDirectory">Where are the files going?</param>
        /// <returns></returns>
        public static String getTempFolderPath(String sourceDirectory, String targetDirectory)
        {
            return String.Format(@"{0}\{1}_buffer_temp", targetDirectory, System.IO.Path.GetFileName(sourceDirectory)); 
        }



    }
}
