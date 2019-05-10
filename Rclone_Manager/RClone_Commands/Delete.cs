using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.Utilities;

namespace RClone_Manager.RClone_Commands
{
    public static class Delete
    {

        public static void deleteFolderContents(String rCloneDirectory, String driveTargetDirectory)
        {
            String fullParameters = String.Format("\"{0}\"", driveTargetDirectory);
            Shell.runRCloneShell(rCloneDirectory, "delete", fullParameters);

        }

        public static void emptyTrashFolder(String rCloneDirectory, String rCloneConfiguration)
        {
            String fullParameters = String.Format("delete {0}: --drive-trashed-only --drive-use-trash=false --verbose=2",rCloneConfiguration);
            Shell.runRCloneShell(rCloneDirectory, "delete", fullParameters);

        }


    }
}
