using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomEvents
{
    /// <summary>
    /// RegisterGoBackEvent
    /// </summary>
    public class RegisterGoBack:EventArgs
    {
        public bool GoBack{ get; set; }

        public RegisterGoBack(bool GoBack)
        {
            this.GoBack = GoBack;
        }
    }
}
