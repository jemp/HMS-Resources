using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RClone_Manager.Utilities.Serialize.Expressions
        {
    /// <summary>
        /// This Serializer class will parse Strings into object oriented format.
        /// To be used in conjuction with the Serializer class
        /// </summary>
    public static class Directory
    {
        /// <summary>
        /// Will parse a string in a similar format to:
        /// 916643446 2019-04-01 08:30:08.000000000 03-24-2019_04-01-2019.zip
        /// Using Regex Format:
        /// (0*[1-9][0-9]*) ([12]\d{3}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01]) \d+:\d{2}:\d{2}\.[0-9]{9}) ([^\\]*(?=[.][a-zA-Z])\.[^.]+$)
        /// </summary>
        /// <param name="fileInput">The Info string from the cloud source</param>
        /// <returns></returns>
        public static FileCloudInfo modifiedTimeSizePath(String fileInput)
        {
   
            
            ///The Regex Expression
            String fileInfoExpression = @"(0*[1-9][0-9]*) ([12]\d{3}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01]) \d+:\d{2}:\d{2}\.[0-9]{9}) ([^\\]*(?=[.][a-zA-Z])\.[^.]+$)";

            ///Let's throw the input into the regex format
            Regex regex = new Regex(fileInput, RegexOptions.IgnoreCase);

            ///let's get the matches!
            Match match = regex.Match(fileInfoExpression);

            ///Initialize the FileCloudInfo object so we can use it
            FileCloudInfo result = new FileCloudInfo( Int32.Parse(match.Groups[0].Value),DateTime.Parse( match.Groups[1].Value),  match.Groups[2].Value);
  

            return result;

        }


        /// <summary>
        /// Object for storing basic information for Cloud File Information
        /// </summary>
        public class FileCloudInfo
        {
            /// <summary>
            /// Length of the file (in bytes)
            /// </summary>
            public int Length { get; } 
            /// <summary>
            /// Last time file was modified
            /// </summary>
            public DateTime LastModified { get; }
            /// <summary>
            /// FilePath in which it exists on the cloud
            /// </summary>
            public String FilePath { get; }

            /// <summary>
            /// Initialize the FileCLoudInfo Object
            /// </summary>
            /// <param name="initLength"></param>
            /// <param name="initLastModified"></param>
            /// <param name="initFilePath"></param>
            public FileCloudInfo(int initLength, DateTime initLastModified, String initFilePath)
            {
                Length = initLength;
                LastModified = initLastModified;
                FilePath = initFilePath;

            }
           
          
        }


    }
}
