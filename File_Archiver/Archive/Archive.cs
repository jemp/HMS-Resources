using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.RClone_Commands;
using File_Manager.General;

namespace File_Archiver.Archive
{
    public static class Archive
    {

        public static void archiveFolder(String rCloneLocation, String gDriveDirectory, String localDropStream, String localArchiverBuffer, String remoteTarget, String remoteArcive)
        {

            String localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);


        }

    }
}
