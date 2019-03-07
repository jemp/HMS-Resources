using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.RClone_Commands;
using File_Manager.General;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //Move.moveFile(@"C:\rclone test\rclone-v1.46-windows-amd64", @"C:\rclone-test", @"HMS:/Archive/Home-Surveillance/Test", "Zip", 3);
            Organizer.createTimestampFolders("C:\\rclone-test\\", "C:\\rclone-test\\", @"(?:LIVCAM\.)(\d{8})(?:_)(\d{6})(?:\.*)", "avi");

        }
    }
}
