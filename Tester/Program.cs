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
          
            Organizer.createTimestampFolders("C:\\rclone", "E:\\Temp\\Archive-Buffer\\", @"(?:LIVCAM\.)(\d{8})(?:_)(\d{6})(?:\.*)", "avi");
            //Move.moveFile(@"C:\rclone test\rclone-v1.46-windows-amd64", @"C:\rclone-test", @"HMS:/HMS-File-Management/Archive/Home-Surveillance/Test", "zip", 3);

        }
    }
}
