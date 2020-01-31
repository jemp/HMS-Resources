using System;
using System.Collections.Generic;
using System.Linq;
using static RClone_Manager.Utilities.Serialize.Expressions.CloudDirectoryInfo;

namespace RClone_Manager.Utilities.Serialize
{
    public class CloudDirectory
    {

        public static List<FileCloudInfo> serializeDirectory(String directorySource)
        {

            List<string> directoryList = directorySource.Split(',').ToList();
            List<FileCloudInfo> toReturn = new List<FileCloudInfo>();
           toReturn.AddRange(directoryList.Select(x => modifiedTimeSizePath(x)));

            return toReturn;


        }


    }
}
