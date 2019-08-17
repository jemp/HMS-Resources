using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Archiver.Processing;
using NLog;

namespace File_Archiver
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {

           
            Archive.archiveFolder(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[0], args[0]);

            }
            catch(IndexOutOfRangeException e)
            {
                Logger.Error(e, String.Format("{0} : {1}", "Error while assigning archiver parameters", e.Message));
            }

        }
    }
}
