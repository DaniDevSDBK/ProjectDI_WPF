using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomEvents
{
    /// <summary>
    /// RegisteredEvent
    /// </summary>
    public class Registered:EventArgs
    {
        public bool Register { get; set; }

        public Registered(bool registered)
        {
            Register = registered;
        }
    }
}
