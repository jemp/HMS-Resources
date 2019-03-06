using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Network_Tools;
using RClone_Manager.Utilities;

namespace RClone_Manager.RClone_Commands
{
    public static class Move
    {

        public static  void moveFile(String rCloneDirectory, String sourceDirectory, String driveTargetDirectory, String fileExtenstion, int maxAttempts)
        {
            int attempts = 0;
            DirectoryInfo d = new DirectoryInfo(sourceDirectory);
            FileInfo[] files = d.GetFiles(String.Format("*.{0}", fileExtenstion)); //Getting Text files


            foreach (FileInfo file in files)
            {
                while (attempts != maxAttempts)
                {
                    if (Status.CheckForInternetConnection())
                    {
                        Shell.runRCloneShell("move", rCloneDirectory, file.FullName, driveTargetDirectory);
                        break;
                    }
                    else
                    {
                        attempts += 1;
                        System.Threading.Thread.Sleep(300000);
                    }

                }

            }

        }

    }
}





