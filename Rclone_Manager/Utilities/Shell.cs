using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RClone_Manager.Utilities
{
    public class Shell
    {
        public static void runRCloneShell(String rCloneDirectory, String command, String parameters)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = String.Format("{0}", rCloneDirectory);
            startInfo.Arguments = String.Format("/C \"rclone {0} {1}", command, parameters);
            process.StartInfo = startInfo;

            process.Start();
            process.WaitForExit();
        }


    }
}
