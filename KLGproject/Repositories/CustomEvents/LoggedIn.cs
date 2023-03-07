using KLGproject.Repositories.AppObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLGproject.Repositories.CustomEvents
{
    /// <summary>
    /// LoggedInEvent
    /// </summary>
    public class LoggedIn : EventArgs
    {
        public bool Logged { get; set; }
        public User CurrentUser { get; set; }

        public LoggedIn(bool loggedIn)
        {
            this.Logged = loggedIn;
        }
        
        public LoggedIn(User currentUser)
        {
            this.CurrentUser = currentUser;
        }

        public LoggedIn(bool loggedIn, User currentUser)
        {
            this.Logged = loggedIn;
            this.CurrentUser = currentUser;
        }
    }
}
