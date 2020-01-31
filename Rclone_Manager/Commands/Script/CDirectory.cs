using RClone_Manager.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RClone_Manager.Commands
{
    public class CDirectory
    {
        /// <summary>
        /// Will retrieve a whole folder's contents on the cloud, along with file sizes
        /// </summary>
        /// <param name="rCloneDirectory"></param>
        /// <param name="driveTargetDirectory"></param>
        /// <returns></returns>
        public static string getFilesStatsInDirectory(String rCloneDirectory, String driveTargetDirectory)
        {

            ///Timer, for diagnostics
            Stopwatch watch = Stopwatch.StartNew();

            ///rClone console commands
            String fullParameters = String.Format("\"{0}\"", driveTargetDirectory);
            String shellOutput =  Shell.runRCloneShell(rCloneDirectory, "lsl", fullParameters);

            FileInfo file = new FileInfo(null);
        

         
            return shellOutput;
            ///Return timer in string format

        }


        
        



    }


 


}
