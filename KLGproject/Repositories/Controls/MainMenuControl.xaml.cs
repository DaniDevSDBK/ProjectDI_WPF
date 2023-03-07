using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.CustomEvents;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para MainMenuControl.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
        ///Event For LogIn
        public delegate void MyEventHandler(object sender, MenuSelectionEvent e);
        public event MyEventHandler menuSelection;

        /// <summary>
        /// Constructor for <see cref="MainMenuControl"/>
        /// </summary>
        public MainMenuControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks the selection and throws a new <see cref="EventArgs": <see cref="MenuSelectionEvent"/></see>/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckSelection(object sender, RoutedEventArgs e) => menuSelection(this, new MenuSelectionEvent((sender as RadioButton)?.Content.ToString()));
    }
}
