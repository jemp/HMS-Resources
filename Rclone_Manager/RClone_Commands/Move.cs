using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Network_Tools;
using RClone_Manager.Utilities;
using RClone_Manager.Diagnostics;
using System.Diagnostics;

namespace RClone_Manager.RClone_Commands
{
    /// <summary>
    /// This class is reponsible for all move operations inside of RClone
    /// </summary>
    public static class Move
    {

        /// <summary>
        /// This Function will move a set directory to the cloud storage
        /// </summary>
        /// <param name="rCloneDirectory">Directory of rClone EXE</param>
        /// <param name="sourceDirectory">Locally, where are the files we are looking for?</param>
        /// <param name="driveTargetDirectory">Where on the cloud is this going?</param>
        /// <param name="fileExtenstion">What extentension are we looking for?</param>
        /// <param name="maxAttempts">How many attempts to connect before we give up?</param>
        /// <returns></returns>
        public static String moveFile(String rCloneDirectory, String sourceDirectory, String driveTargetDirectory, String fileExtenstion, int maxAttempts)
        {

            try
            {
                ///initate variables for transfer
                int attempts = 0;
                DirectoryInfo d = new DirectoryInfo(sourceDirectory);
                FileInfo[] files = d.GetFiles(String.Format("*.{0}", fileExtenstion)); //Getting files of a certain thype


                Stopwatch watch = Stopwatch.StartNew();

                ///For Each file in specified location
                foreach (FileInfo file in files)
                {
                    ///While we haven't exceeded up max attempts for internet connecting
                    while (attempts != maxAttempts)
                    {
                        ///If we can connect to the internet
                        if (Status.CheckForInternetConnection())
                        {
                            ///Full parameters for cloud directory
                            String fullParameters = String.Format("\"{0}\" {1}\"", file.FullName, driveTargetDirectory);
                            ///Finally, run move command
                            Shell.runRCloneShell(rCloneDirectory, "move", fullParameters);
                            break;
                        }
                        else
                        {
                            ///Whoops, we tried in vien, abort ABORT
                            if (attempts == 3) { throw new Exception("Internet Connection not reliable - aborting moveFile"); }
                            ///Else, keep on truckin'
                            else { System.Threading.Thread.Sleep(300000); attempts += 1; }


                        }
                        
                    }
                    

                }
                ///Return elapse timer in string format
                return watch.Elapsed.ToString();
            }
            catch (Exception e)
            {
                throw new Rclone_Move_Exception(e.Message,e);

            }
           



        }




    }
}





