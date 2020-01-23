using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Network_Tools
{
    /// <summary>
    /// This class provides functionality for network checks, and other network diagnostics
    /// </summary>
    /// 

    public static class Status
    {
        private const string connectionURL = "http://clients3.google.com/generate_204";

        /// <summary>
        /// Check for an active internet connection
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(connectionURL))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}
