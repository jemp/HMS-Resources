using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Manager.General;
using ToolKit.Applications;
using System.IO;
using System.Diagnostics;
using NLog;
using File_Manager.Diagnostics;
using RClone_Manager.Diagnostics;
using File_Archiver.Configuration;
using RClone_Manager.Commands;
using RClone_Manager.Utilities.Serialize;
using RClone_Manager.Utilities.Serialize.Expressions;
using static RClone_Manager.Utilities.Serialize.Expressions.CloudDirectoryInfo;

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


        ///NLOG Plugin - Configurable through config
        private static readonly Logger Logger =LogManager.GetCurrentClassLogger();


        public static void archiveFolder(
            String rCloneDirectory,  
            String localDropStream,
            String localArchiverBuffer, 
            String remoteDropStreamTarget,
            String remoteArchive, 
            String fileFormatNameRegex, 
            String fileExtenstion)
        {

            Stopwatch watch = Stopwatch.StartNew();

            try
            {
            

                ///Timer for diagnosing

                String elaspedTimer;

                ///Let's get a temperary name for the temperary folder
                Logger.Info("Getting Temparary Folder... ");
                String localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);
                Logger.Info(String.Format("{0} - {1}", "Temparary Folder Retrieved!",localTempFolder));
                
                ///Where will this zip file be located locally
                Logger.Info("Creating Time-Stamped folders...");
                String localZipDestination = Organizer.createTimestampFolders(localDropStream, localArchiverBuffer, fileFormatNameRegex, fileExtenstion);
                Logger.Info(String.Format("{0}: {1}", "Time-Stamped folders created! Local Zip Destination", localTempFolder));

                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Deleting requested remote folders", localTempFolder, remoteDropStreamTarget));
                elaspedTimer = CDelete.deleteDirectory(rCloneDirectory, remoteDropStreamTarget);
                Logger.Info(String.Format("{0}: {1}", "Successfully deleted Contents! Elapsed time", elaspedTimer));


                ///Due to a bug, the cloud software may not "release" files. Resetting it will fix this.

                Logger.Info(String.Format("{0} - cloudProcessName: {1} cloudProcessPath: {2}", "Restarting Process", Config.cloudProcessName, Config.cloudProcessPath));
                Management.restartProcess(Config.cloudProcessName, Config.cloudProcessPath);
                Logger.Info("Process successully restarted!");


                ///Compress / Remove the folder to be archived
                Logger.Info(String.Format("{0}: {1}", "Compress and removing target folder to the following location", localTempFolder));
                Organizer.compressAndRemoveTargetFolder(localZipDestination);
                Logger.Info("Successfully compressed and removed folder!");

                FileInfo info = new FileInfo(localTempFolder);


                ///Delete any files in cloud over threshold
                Logger.Info(String.Format("Removing any files over: {0} At remote Location: {1} Utilizing", rCloneDirectory, remoteArchive, info.Name));
                List<FileCloudInfo> filesToRemove =    Containment.getFIlesInDirectoryOverThreshold(rCloneDirectory, remoteArchive, info);
                Logger.Info("Now removing a total of {0} files from cloud directory: {1}", filesToRemove.Count(),remoteArchive) ;
                filesToRemove.ForEach(i => CDelete.deleteDirectory(i.FilePath,remoteArchive));
                Logger.Info("Successfully removed files over threshold! Files removed: {0} Memory Free'd up: {1}",filesToRemove.Count);

                ///Moving Zipped file to the cloud storage
                Logger.Info(String.Format("{0} - Local Temp Folder: {1} RemoteArchive: {2}", "Moving the compressed file to cloud storage!", localTempFolder, remoteArchive));
                elaspedTimer = CMove.moveFile(rCloneDirectory, localTempFolder, remoteArchive, Config.compressionFormat, Config.connectionAttempts);
                Logger.Info(String.Format("{0}: {1}", "Successfully deleted Contents! Elapsed time", elaspedTimer));

                ///Delete the local folder
                Logger.Info(String.Format("{0}: {1}", "Deleting the following local 'Temp Folder' ", localTempFolder));
                System.IO.Directory.Delete(localTempFolder, true);
                Logger.Info("Successfully deleted the local temp folder!");

                ///Delete the cloud folder
                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Emptying Cloud Folder", localTempFolder, Config.driveProfileName));
                CDelete.emptyTrashFolder(rCloneDirectory, Config.driveProfileName);
                Logger.Info("Successfully emptied cloud recycle bin");

                Logger.Info(String.Format("{0} - Elasped time:{1}", "Archiver has successully been ran!", watch.ElapsedMilliseconds.ToString()));


            }

            catch(OrganizerException e)
            {
                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2} ", "Error while prepping files before transfer", e.Message, watch.Elapsed.ToString()));
                Logger.Trace(e.StackTrace);
            }
            catch(Rclone_Move_Exception e)
            {

                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2}", "Error while transfering file to the cloud", e.Message, watch.Elapsed.ToString()));
                Logger.Trace(e.StackTrace);
            }

            catch(Exception e)
            {
               
                Logger.Error(e, String.Format("{0} - {1} (Elapsed time before error: {2} ", "Error while Archiving", e.Message, watch.Elapsed.ToString()));
                Logger.Trace(e.StackTrace);
            }

        }

    }
}
