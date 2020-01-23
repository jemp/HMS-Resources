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

        internal static string gDriveConfigurationName { get; set; } = ConfigurationManager.AppSettings["gDriveProfileName"];
        public static string driveProfileName { get; set; } = ConfigurationManager.AppSettings["DriveProfileName"];
        public static string cloudProcessName { get; set; } = ConfigurationManager.AppSettings["cloudProcessName"];
        internal static int connectionAttempts { get; set; } = Int32.Parse(ConfigurationManager.AppSettings["connectionAttempts"]);
        internal static string compressionFormat { get; set; } = ConfigurationManager.AppSettings["commpressionFormat"];
        public static string cloudProcessPath { get; set; } = ConfigurationManager.AppSettings["cloudProcessPath"];



    }
}
