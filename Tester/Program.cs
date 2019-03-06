using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.RClone_Commands;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Move.moveFile(@"C:\rclone test\rclone-v1.46-windows-amd64", @"C:\rclone-test", @"HMS:/Archive/Home-Surveillance/Test", "Zip", 3);

        }
    }
}
