using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_Server_Forms_0._9._1._1
{
    public class User : IDisposable
    {
        string username;
        string id;
        string match_code;
        Socket socket;

        public User()
        {
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        public string Code
        {
            get { return match_code; }
            set { match_code = value; }
        }

        #region Dispose object

        // To detect redundant calls
        private bool isDisposed = false;

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Code to dispose the managed resources of the class
                if (username != null)
                    username = null;
                if (socket != null)
                    socket = null;

            }
            // Code to dispose the un-managed resources of the class
            isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
