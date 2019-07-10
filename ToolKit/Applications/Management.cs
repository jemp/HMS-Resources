using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Applications
{
    public static class Management
    {

        public static void killProcess(String applicationName)
        {

            foreach (var process in Process.GetProcessesByName("googledrivesync"))
            {
                process.Kill();
            }
        }


    }
}
