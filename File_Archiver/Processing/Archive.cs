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
            String fileExtenstion,
            String thesholdInGigabytes)
        {

            Stopwatch watch = Stopwatch.StartNew();

            String localTempFolder = String.Empty;
            String localZipDestination =  String.Empty;
            List<String> cmdListOutput = new List<String>(); //Output for listed group items

            try
            {
            

                ///Timer for diagnosing

                ///Let's get a temperary name for the temperary folder
                Logger.Info("Getting Temparary Folder... ");
                 localTempFolder = Organizer.getTempFolderPath(localDropStream, localArchiverBuffer);
                Logger.Info(String.Format("{0} - {1}", "Temparary Folder Retrieved!",localTempFolder));
                
                ///Where will this zip file be located locally
                Logger.Info("Creating Time-Stamped folders...");
                localZipDestination = Organizer.createTimestampFolders(localDropStream, localArchiverBuffer, fileFormatNameRegex, fileExtenstion);
                Logger.Info(String.Format("{0}: {1}", "Time-Stamped folders created! Local Zip Destination", localZipDestination));

                ///Compress / Remove the folder to be archived
                Logger.Info(String.Format("{0}: {1}", "Compress and removing target folder to the following location", localTempFolder));
                Organizer.compressAndRemoveTargetFolder(localZipDestination);
                Logger.Info("Successfully compressed and removed folder!");


                ///To make the threshold process a little easier, we need to rename any duplicated file names
                Logger.Info(String.Format("{0}: {1}", "Renaming any duplicated files for removal", localTempFolder));
                CDirectory.renameDuplicatedFiles(rCloneDirectory, remoteArchive);
                Logger.Info("Duplicates renamed / removed!");

                ///Serialize localzipdesitination file for parsing
                FileInfo info = new FileInfo(localZipDestination);
                ///Get a list of all of the existing files in target archive
                var existingFiles = CloudDirectory.serializeDirectory(CDirectory.getFilesStatsInDirectory(rCloneDirectory, remoteArchive));
                ///Delete any files in cloud over threshold
                Logger.Info(String.Format("Removing any files over: {0} (GB) At remote Location: {1} Utilizing: {2}", thesholdInGigabytes, remoteArchive, info.Name));
                List<FileCloudInfo> filesToRemove =    Containment.getFIlesInDirectoryOverThreshold(existingFiles,info, Double.Parse(thesholdInGigabytes));
                Logger.Info("Now removing a total of {0} files from cloud directory: {1}", filesToRemove.Count(),remoteArchive) ;
                Logger.Debug("Target Files: {0}", String.Concat(filesToRemove.Select(o => String.Format("{0}\n ", o.FilePath)))); //Print out all of the files to remove
                
                ///Run Command to Delete *any* target files
                filesToRemove.ForEach(i => cmdListOutput.Add( CDelete.deleteDirectory(rCloneDirectory, String.Format(@"{0}/{1}",remoteArchive,i.FilePath))));
                ///Lots of logging, information regarding deleting items
                Logger.Debug("Command Ouput for deletion: {0}", String.Concat(cmdListOutput.Select(o => String.Format("{0}\n ", o))));
                Logger.Info("Ran command to removed files over threshold! Files *removed*: {0} | Memory *Free'd up*: {1} (GB) ",filesToRemove.Count, 
                ByteSizeLib.ByteSize.FromBytes(filesToRemove.Sum(i => i.Length)).GigaBytes, (filesToRemove.Sum(i=>i.Length)));

                ///Moving Zipped file to the cloud storage
                Logger.Info(String.Format("{0} - Local Temp Folder: {1} RemoteArchive: {2}", "Moving the compressed file to cloud storage!", localTempFolder, remoteArchive));
               CMove.moveFile(rCloneDirectory, localTempFolder, remoteArchive, Config.compressionFormat, Config.connectionAttempts);
                Logger.Info(String.Format("{0}", "Successfully deleted Contents!"));

                ///Delete the local folder
                Logger.Info(String.Format("{0}: {1}", "Deleting the following local 'Temp Folder' ", localTempFolder));
                System.IO.Directory.Delete(localTempFolder, true);
                Logger.Info("Successfully deleted the local temp folder!");

                ///TODO: Remove this to a later process...
                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Deleting requested remote folders", rCloneDirectory, remoteDropStreamTarget));
                CDelete.deleteDirectory(rCloneDirectory, remoteDropStreamTarget);
                Logger.Info(String.Format("{0}", "Deletion of contents command has been ran!" ));


                ///Due to a bug, the cloud software may not "release" files. Resetting it will fix this.

                Logger.Info(String.Format("{0} - cloudProcessName: {1} cloudProcessPath: {2}", "Restarting Process", Config.cloudProcessName, Config.cloudProcessPath));
                Management.restartProcess(Config.cloudProcessName, Config.cloudProcessPath);
                Logger.Info("Process successully restarted!");



                ///Delete the cloud folder
                Logger.Info(String.Format("{0} - rCloneLocation: {1} gDriveName: {2}", "Emptying Cloud Folder", rCloneDirectory, Config.driveProfileName));
                CDelete.emptyTrashFolder(rCloneDirectory, Config.driveProfileName);
                Logger.Info("Successfully emptied cloud recycle bin");

                Logger.Info(String.Format("{0} - Elasped time:{1}", "Archiver has successully been ran!", watch.Elapsed.ToString()));


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

           finally
            {
                ///If the process fails, remove the temperary directory!
                if (Directory.Exists(localTempFolder)) { Directory.Delete(localTempFolder, true); }
                

            }

        }

    }
}
