using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RClone_Manager.Utilities
{
    public class Shell
    {
        /// <summary>
        /// This method will run a given rClone command through console
        /// </summary>
        /// <param name="rCloneDirectory">Where is the rclone program located?</param>
        /// <param name="command">What command should be initiate</param>
        /// <param name="parameters">What are the parameters for the command?</param>
        public static String runRCloneShell(String rCloneDirectory, String command, String parameters)
        {

            String commandOutput = String.Empty;
            ///Let's set up the command process
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = startInfo.FileName = "cmd.exe";

            ///Set working directory to where the rclone program is at
            startInfo.WorkingDirectory = String.Format("{0}", rCloneDirectory);
            ///Pass arguements
            startInfo.Arguments = String.Format("/C \"rclone {0} {1}", command, parameters);


            //Finally, after everything is set, start the process until exit
            process.StartInfo = startInfo;
            process.Start();
            commandOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return commandOutput;
        }


    }
}
