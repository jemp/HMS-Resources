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

        public static async void moveFile(String rCloneDirectory, String sourceDirectory, String driveTargetDirectory, String fileExtenstion, int maxAttempts)
        {
            int moveAttempts = 0;


            DirectoryInfo d = new DirectoryInfo(sourceDirectory);
            FileInfo[] files = d.GetFiles(String.Format("*.{0}", fileExtenstion)); //Getting Text files

            if (Status.CheckForInternetConnection())
            {
                if (moveAttempts != 0) { await Task.Delay(3000); }

                while (files.Any() && moveAttempts != maxAttempts)
                {

   
                    foreach (FileInfo file in files)
                    {
                        Shell.runRCloneShell("move", rCloneDirectory, file.FullName, driveTargetDirectory);

                    }

                 
                   
                    moveAttempts += 1;
                    files = d.GetFiles(fileExtenstion);

                }
                
                }

            }




        }



    }

