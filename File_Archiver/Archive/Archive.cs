using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RClone_Manager.RClone_Commands;
using File_Manager.General;
using ToolKit.Applications;
using System.IO;
using System.Diagnostics;

namespace File_Archiver.Archive
{
    public static class Archive
    {
        /// <summary>
        /// This function will:
        /// 1.Select a target Directory
        /// 2. Please each file in respected folder by date
        /// 3. ZIP all files in question
        /// 4.Send to cloud drive
        /// </summary>
        /// <param name="rCloneLocation">Location of rClone Executable</param>
        /// <param name="gDriveDirectory">The directory in gDrive in which we are performing the call</param>
        /// <param name="localDropStream">Where are the files being dumped on HMS?</param>
        /// <param name="localArchiverBuffer">Were are we performing this job?</param>
        /// <param name="remoteTarget">On the cloud, where are we sending the finished product to?</param>
        /// <param name="remoteArchive">Where is the zip going after compelted?</param>
        /// <param name="fileNameRegex">The Regex of what the file name looks like</param>
        /// <param name="fileExtenstion">Extension of file that we are looking for</param>
        /// <param name="gDriveName">Name of the g-Drive?</param>
        public static void archiveFolder(String rCloneLocation, String gDriveDirectory, String localDropStream, String localArchiverBuffer, String remoteTarget, String remoteArchive, String fileNameRegex, String fileExtenstion, String gDriveName)
        {

            String localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);

            String localZipDestination = Organizer.createTimestampFolders(localDropStream, localArchiverBuffer, fileNameRegex, fileExtenstion);

            Delete.deleteFolderContents(rCloneLocation, remoteTarget);

            Management.killProcess(Configuration.Config.cloudProcessName);
         
            Organizer.compressAndRemoveTargetFolder(localZipDestination);

            Directory.Delete(localTempFolder, true);

            Delete.emptyTrashFolder(rCloneLocation, gDriveName);

        }

    }
}
