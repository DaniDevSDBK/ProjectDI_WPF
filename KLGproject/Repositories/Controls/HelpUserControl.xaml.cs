using KLGproject.Repositories.AppObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para HelpUserControl.xaml
    /// </summary>
    public partial class HelpUserControl : UserControl
    {
        public HelpUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Redirects the user to the link, using his default system browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

            Hyperlink hp = (Hyperlink)sender;

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                
                FileName = hp.NavigateUri.ToString(),
                UseShellExecute = true
            });
        }

    }
}
