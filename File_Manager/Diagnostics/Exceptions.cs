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
    public class OrganizerException : Exception
    {
        public OrganizerException()
        {
        }

        public OrganizerException(string message)
            : base(message)
        {
        }

        public OrganizerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
