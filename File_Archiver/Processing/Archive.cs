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
using File_Manager.Diagnostics;
using RClone_Manager.Diagnostics;

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

        ///NLOG Plugin - Configurable through config
        private static readonly Logger Logger =LogManager.GetCurrentClassLogger();


        public static void archiveFolder(String rCloneDirectory, String gDriveDirectory, String localDropStream, String localArchiverBuffer, String remoteTarget, String remoteArchive, String remoteDriveFolderPath , String fileFormatNameRegex, String fileExtenstion)
        {

            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                
                ///Timer for diagnosing
                 
                String elaspedTimer;

                ///Let's get a temperary name for the temperary folder
                Logger.Info("Getting Temparary Folder...");
                String localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);
                Logger.Info(String.Format("{0} - {1}", "Temparary Folder Retrieved!",localTempFolder));

                ///Where will this zip file be located locally
                Logger.Info("Creating Time-Stamped folders...");
                String localZipDestination = Organizer.createTimestampFolders(localDropStream, localArchiverBuffer, fileFormatNameRegex, fileExtenstion);
                Logger.Info(String.Format("{0}: {1}", "Time-Stamped folders created! Local Zip Destination", localTempFolder));

                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Deleting requested remote folders", localTempFolder, remoteDriveFolderPath));
                elaspedTimer = Delete.deleteFolderContents(rCloneDirectory, remoteDriveFolderPath);
                Logger.Info(String.Format("{0}: {1}", "Successfully deleted Contents! Elapsed time", elaspedTimer));


                ///Due to a bug, the cloud software may not "release" files. Resetting it will fix this.
                Logger.Info("Restarting process");
                Management.restartProcess(Configuration.Config.cloudProcessName, Configuration.Config.cloudProcessPath);
                Logger.Info("Process successully restarted!");


                ///Compress / Remove the folder to be archived
                Logger.Info(String.Format("{0}: {1}", "Compress and removing target folder to the following location", localTempFolder));
                Organizer.compressAndRemoveTargetFolder(localZipDestination);
                Logger.Info("Successfully compressed and removed folder!");

                ///Moving Zipped file to the cloud storage
                Logger.Info(String.Format("{0} - Local Temp Folder: {1} RemoteArchive: {2}", "Moving the compressed file to cloud storage!", localTempFolder, remoteDriveFolderPath));
                elaspedTimer = Move.moveFile(rCloneDirectory, localTempFolder, remoteArchive, Configuration.Config.compressionFormat, Configuration.Config.connectionAttempts);
                Logger.Info(String.Format("{0}: {1}", "Successfully deleted Contents! Elapsed time", elaspedTimer));

                ///Delete the local folder
                Logger.Info(String.Format("{0}: {1}", "Deleting the following local 'Temp Folder' ", localTempFolder));
                Directory.Delete(localTempFolder, true);
                Logger.Info("Successfully deleted the local temp folder!");

                ///Delete the cloud folder
                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Emptying Cloud Folder", localTempFolder,remoteDriveFolderPath));
                Delete.emptyTrashFolder(rCloneDirectory, remoteDriveFolderPath);
                Logger.Info("Successfully emptied cloud recycle bin");

                Logger.Info(String.Format("{0} - Elasped time:{1}", "Archiver has successully been ran!", watch.ElapsedMilliseconds.ToString()));


            }

            catch(OrganizerException e)
            {
                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2} ", "Error while prepping files before transfer", e.Message, watch.Elapsed.ToString()));
            }
            catch(Rclone_Move_Exception e)
            {

                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2}", "Error while transfering file to the cloud", e.Message, watch.Elapsed.ToString()));

            }

            catch(Exception e)
            {
                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2} ", "Error while Archiving", e.Message, watch.Elapsed.ToString()));
            }

        }

    }
}
