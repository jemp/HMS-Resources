using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.Utilities;

namespace RClone_Manager.Commands
{
    public static class CDelete
    {

        /// <summary>
        /// Will delete a whole folder's contents on the cloud
        /// </summary>
        /// <param name="rCloneDirectory"></param>
        /// <param name="driveTargetDirectory"></param>
        /// <returns></returns>
        public static void deleteDirectory(String rCloneDirectory, String driveTargetDirectory)
        {
            ///Timer, for diagnostics
            Stopwatch watch = Stopwatch.StartNew();

            ///rClone console commands
            String fullParameters = String.Format("\"{0}\"", driveTargetDirectory);
            Shell.runRCloneShell(rCloneDirectory, "delete", fullParameters);

  
        }

        /// <summary>
        /// Empty the "recycle bin" of the cloud storage
        /// </summary>
        /// <param name="rCloneDirectory"></param>
        /// <param name="rCloneConfiguration"></param>
        public static void emptyTrashFolder(String rCloneDirectory, String rCloneConfiguration)
        {
            ///Timer, for diagnostics
            Stopwatch watch = Stopwatch.StartNew();

            String fullParameters = String.Format("{0}: --drive-trashed-only --drive-use-trash=false --verbose=2",rCloneConfiguration);
            Shell.runRCloneShell(rCloneDirectory, "delete", fullParameters);



        }


     


    }
}
