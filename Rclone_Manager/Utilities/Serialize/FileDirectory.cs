using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RClone_Manager.Utilities.Serialize.Expressions.Directory;

namespace RClone_Manager.Utilities.Serialize
{
    public class FileDirectory
    {

        public static List<FileCloudInfo> serializeDirectory(String directorySource)
        {

            List<string> directoryList = directorySource.Split(',').ToList();
            List<FileCloudInfo> toReturn = new List<FileCloudInfo>();
           toReturn.AddRange(directoryList.Select(x => Expressions.Directory.modifiedTimeSizePath(x)));

            return toReturn;


        }


    }
}
