using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomEvents
{
    /// <summary>
    /// UserControlChangedEvent
    /// </summary>
    public class UserControlChanged
    {
        public bool Changed { get; set; }

        public UserControlChanged(bool loggedIn)
        {
            Changed = loggedIn;
        }
    }
}
