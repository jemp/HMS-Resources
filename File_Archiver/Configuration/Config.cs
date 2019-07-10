using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace File_Archiver.Configuration
{
    static class Config
    {

        public static string driveProfileName { get; set; } = ConfigurationManager.AppSettings["DriveProfileName"];
        public static string cloudProcessName { get; set; } = ConfigurationManager.AppSettings["cloudProcessName"];



    }
}
