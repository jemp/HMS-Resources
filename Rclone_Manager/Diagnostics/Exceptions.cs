using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RClone_Manager.Diagnostics
{
    /// <summary>
    /// Custom Exception for rClone Moving
    /// </summary>
    public class Rclone_Move_Exception : Exception
    {
        public Rclone_Move_Exception()
        {
        }

        public Rclone_Move_Exception(string message)
            : base(message)
        {
        }

        public Rclone_Move_Exception(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
