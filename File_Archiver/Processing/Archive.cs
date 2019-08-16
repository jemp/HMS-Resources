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
using NLog;

namespace File_Archiver.Processing
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
        /// 

        private static readonly Logger Logger =LogManager.GetCurrentClassLogger();


        public static void archiveFolder(String rCloneLocation, String gDriveDirectory, String localDropStream, String localArchiverBuffer, String remoteTarget, String remoteArchive, String fileNameRegex, String fileExtenstion, String gDriveName)
        {
            try
            {



                ///Timer for diagnosing
                 Stopwatch watch = Stopwatch.StartNew();
                String elaspedTimer;

                ///Let's get a temperary name for the temperary folder
                Logger.Info("Getting Temparary Folder...");
                String localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);
                Logger.Info(String.Format("{0} - {1}", "Temparary Folder Retrieved!",localTempFolder));

                ///Where will this zip file be located locally
                Logger.Info("Creating Time-Stamped folders...");
                String localZipDestination = Organizer.createTimestampFolders(localDropStream, localArchiverBuffer, fileNameRegex, fileExtenstion);
                Logger.Info(String.Format("{0}: {1}", "Time-Stamped folders created! Local Zip Destination", localTempFolder));

                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Deleting requested remote folders", localTempFolder, gDriveName));
                elaspedTimer = Delete.deleteFolderContents(rCloneLocation, remoteTarget);
                Logger.Info(String.Format("{0}: {1}", "Successfully deleted Contents! Elapsed time", elaspedTimer));


                ///Due to a bug, the cloud software may not "release" files. Resetting it will fix this.
                Logger.Info("Killing cloud process...");
                Management.killProcess(Configuration.Config.cloudProcessName);
                Logger.Info("Process successully killed!");


                ///Compress / Remove the folder to be archived
                Logger.Info(String.Format("{0}: {1}", "Compress and removing target folder to the following location", localTempFolder));
                Organizer.compressAndRemoveTargetFolder(localZipDestination);
                Logger.Info("Successfully compressed and removed folder!");

                ///Delete the local folder
                Logger.Info(String.Format("{0}:{1}", "Deleting the following local 'Temp Folder' ", localTempFolder));
                Directory.Delete(localTempFolder, true);
                Logger.Info("Successfully deleted the local temp folder!");

                ///Delete the cloud folder
                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Emptying Cloud Folder", localTempFolder,gDriveName));
                Delete.emptyTrashFolder(rCloneLocation, gDriveName);
                Logger.Info("Successfully emptied cloud recycle bin");

                Logger.Info(String.Format("{0} - Elasped time:{1}", "Archiver has successully been ran!", localTempFolder));


            }

            catch(Exception e)
            {
                Logger.Error(e, String.Format("{0} - {1}","Error while Archiving",e.Message));
            }

        }

    }
}
