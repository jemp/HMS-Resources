using System;
using System.Collections.Generic;
using System.Linq;
using static RClone_Manager.Utilities.Serialize.Expressions.CloudDirectoryInfo;

namespace RClone_Manager.Utilities.Serialize
{
    public class CloudDirectory
    {

        /// <summary>
        /// THis method will serialize, or objectisize all of the strings of list
        /// </summary>
        /// <param name="directorySource"></param>
        /// <returns></returns>
        public static List<FileCloudInfo> serializeDirectory(String directorySource)
        {
            ///Let's take a list of all of the file string names, then split them up by lines..
            List<string> directoryList = directorySource.Split('\n').ToList();
            ///Remove the 'blank' row that is constant
            directoryList.RemoveAt(directoryList.Count - 1); 
            ///initalize return list
            List<FileCloudInfo> toReturn = new List<FileCloudInfo>();
            ///FInally, let's  parse, then convert to objects we can use!
           toReturn.AddRange(directoryList.Select(x => modifiedTimeSizePath(x)));
            ///Returning the recipe we made! So proud.
            return toReturn;


        }

        public static object serializeDirectory(object p)
        {
            throw new NotImplementedException();
        }
    }
}
