using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomEvents
{
    /// <summary>
    /// MenuSelectionEvent
    /// </summary>
    public class MenuSelectionEvent : EventArgs
    {
        public String ButtonName { get; set; }

        public MenuSelectionEvent(string btnName)
        {
            this.ButtonName = btnName;
        }
    }
}
