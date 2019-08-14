using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager.Diagnostics
{
    /// <summary>
    /// Custom Exception for archiveFolder Functionality
    /// </summary>
    public class ArchiveFolderException : Exception
    {
        public ArchiveFolderException()
        {
        }

        public ArchiveFolderException(string message)
            : base(message)
        {
        }

        public ArchiveFolderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
