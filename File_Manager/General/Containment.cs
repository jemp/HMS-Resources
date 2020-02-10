using ByteSizeLib;
using RClone_Manager.Commands;
using RClone_Manager.Utilities.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static RClone_Manager.Utilities.Serialize.Expressions.CloudDirectoryInfo;

namespace File_Manager.General
{
    public static class Containment
    {

        /// <summary>
        /// Will remove any files in specified cloud directory that is over the specified threshold
        /// </summary>
        /// <param name="rCloneDirectory"></param>
        /// <param name="directoryToParse"></param>
        /// <returns></returns>
        public static List<FileCloudInfo> getFIlesInDirectoryOverThreshold(List<FileCloudInfo> existingFiles, FileInfo  targetFileInfo, double thresholdInGigabytes)
        {
            ///First, let's get the maximum space we are allowing from the external library
            var maxFileSize = ByteSize.FromGigaBytes(thresholdInGigabytes);

            ///All of the files that are over the threshold we are allowing (As defined above)
            var filesOverThreshold = new List<FileCloudInfo>();

            ///Order the existing files
           existingFiles =  existingFiles.OrderBy(i => i.LastModified ).ToList();

            ///While the files are under the threshold..
            while (maxFileSize.Bytes < existingFiles.Sum(f => f.Length + targetFileInfo.Length)) 
            {
                ///Add the file to the 'remove' list
                filesOverThreshold.Add(existingFiles[0]);
                ///Finally, remove the file from the existing directory. It's not long for the world...
                existingFiles.RemoveAt(0);
            }

            ///Last but not least, return the files that need to be removed
            return filesOverThreshold;


        }

    }
}
