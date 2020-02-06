using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace RClone_Manager.Utilities.Serialize.Expressions
        {
    /// <summary>
        /// This Serializer class will parse Strings into object oriented format.
        /// To be used in conjuction with the Serializer class
        /// </summary>
    public static class CloudDirectoryInfo
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
            Regex regex = new Regex(fileInfoExpression, RegexOptions.IgnoreCase);

            ///let's get the matches!
            Match match = regex.Match(fileInput);

            ///Initialize the FileCloudInfo object so we can use it
            FileCloudInfo result = new FileCloudInfo( Int32.Parse(match.Groups[1].Value),DateTime.Parse( match.Groups[2].Value),  match.Groups[3].Value);
  

            return result;

        }


        /// <summary>
        /// Object for storing basic information for Cloud File Information
        /// </summary>
        public class FileCloudInfo : IEnumerable<FileCloudInfo>
        {


            #region Properties
            /// <summary>
            /// Length of the file (in bytes)
            /// </summary>
            public long Length { get; }
            /// <summary>
            /// Last time file was modified
            /// </summary>
            public DateTime LastModified { get; }
            /// <summary>
            /// FilePath in which it exists on the cloud
            /// </summary>
            public String FilePath { get; }

            //FileInfoLIst
             List<FileCloudInfo> fileInfoList = new List<FileCloudInfo>();
            #endregion

            #region Instantiators
            /// <summary>
            /// Initialize the FileCLoudInfo Object
            /// </summary>
            /// <param name="initLength"></param>
            /// <param name="initLastModified"></param>
            /// <param name="initFilePath"></param>
            public FileCloudInfo(long initLength, DateTime initLastModified, String initFilePath)
            {
                Length = initLength;
                LastModified = initLastModified;
                FilePath = initFilePath;

            }

            public FileCloudInfo()
            {


            }
            /// <summary>
            /// Cast operand for conversion to: FileInfo -> FileCloudInfo
            /// </summary>
            /// <param name="operand"></param>
            public static implicit operator FileCloudInfo(FileInfo operand)
            {

                return new FileCloudInfo(operand.Length, operand.LastWriteTime, operand.FullName);

            }
            #endregion

            #region Enumerator Ovverides
            public IEnumerator<FileCloudInfo> GetEnumerator()
            {
                return fileInfoList.GetEnumerator();
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion

        }


    }
}
