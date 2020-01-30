using RClone_Manager.Utilities.Serialize;
using RClone_Manager.Utilities.Serialize.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RClone_Manager.Utilities.Serialize.Expressions.Directory;

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
        public static void removeFilesOverThreshold(String rCloneDirectory, String directoryToParse, FileInfo  targetFIleINfo)
        {

           List<FileCloudInfo> parsedFiles =  FileDirectory.serializeDirectory(RClone_Manager.Commands.Directory.getFilesStatsInDirectory(rCloneDirectory, directoryToParse));

            ///Return timer in string format

        }

    }
}
