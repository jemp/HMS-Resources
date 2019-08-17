using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Applications
{
    public static class Management
    {

        /// <summary>
        /// Start a process of a given path
        /// </summary>
        /// <param name="processPath">Path to the executable</param>
        public static void startProcess(String processPath)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, processPath);
            Process.Start(new ProcessStartInfo(path));
        }


        /// <summary>
        /// Kill an executable by a given process name
        /// </summary>
        /// <param name="applicationName"></param>
        public static void killProcess(String applicationName)
        {

            foreach (var process in Process.GetProcessesByName("googledrivesync"))
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Restart a process of a givev name, and wil start based on a process path
        /// </summary>
        /// <param name="applicationName">Application executable name</param>
        /// <param name="processPath">file path of the executable</param>
        public static void restartProcess(String applicationName, String processPath)
        {
            killProcess(applicationName);
            startProcess(processPath);

        }




    }
}
