using ByteSizeLib;
using RClone_Manager.Utilities.Serialize;
using RClone_Manager.Utilities.Serialize.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static List<FileCloudInfo> getFIlesInDirectoryOverThreshold(String rCloneDirectory, String directoryToParse, FileInfo  targetFileInfo, double thresholdInGigabytes)
        {
            ///First, let's get the maximum space we are allowing from the external library
            var maxFileSize = ByteSize.FromGigaBytes(thresholdInGigabytes);
            ///All of the existing files
            var existingFiles  = CloudDirectory.serializeDirectory(RClone_Manager.Commands.CDirectory.getFilesStatsInDirectory(rCloneDirectory, directoryToParse));
            ///All of the files that are over the threshold we are allowing (As defined above)
            var filesOverThreshold = new List<FileCloudInfo>();

            ///Order the existing files
            existingFiles.OrderByDescending(i => i.LastModified );

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
