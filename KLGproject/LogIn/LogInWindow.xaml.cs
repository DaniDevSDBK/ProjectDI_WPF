using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.Controls;
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
using System.Windows.Shapes;

namespace KLGproject.LogIn
{
    /// <summary>
    /// Lógica de interacción para LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public User CurrentUser { get; set; }

        public LogInWindow()
        {
            InitializeComponent();
            this.registerControl.IsEnabled = false;
            this.registerControl.Visibility= Visibility.Hidden;

            this.myUserControl.userLoginEvent += LogIn;
            this.myUserControl.userRegisterEvent += Register;
            this.registerControl.goBackEvent += GoBack;

        }

        private void LogIn(object sender, LoggedIn logged)
        {

            this.CurrentUser = logged.CurrentUser;
           this.Visibility=Visibility.Hidden;

        }

        private void Register(object sender, Registered logged)
        {

            this.myUserControl.IsEnabled = false;
            this.myUserControl.Visibility = Visibility.Hidden;

            this.registerControl.IsEnabled = true;
            this.registerControl.Visibility = Visibility.Visible;

        }

        private void GoBack(object sender, RegisterGoBack e)
        {

            this.myUserControl.IsEnabled = true;
            this.myUserControl.Visibility = Visibility.Visible;

            this.registerControl.IsEnabled = true;
            this.registerControl.Visibility = Visibility.Hidden;

        }

    }
}
