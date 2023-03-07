using KLGproject.Resources.Libraries.DB.Lib;
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
using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.CustomEvents;
using KLGproject.Repositories.CustomExceptions;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para LogInControl.xaml
    /// </summary>
    public partial class LogInControl : UserControl
    {

        private SQLite myDb;
        private User _currentUser;
        private User _tryUser;
        private const string ERROR_STRING = "Fields are empty or the user name/password provided are not correct.";

        ///Event For LogIn
        public delegate void MyEventHandler(object sender, LoggedIn e);
        public event MyEventHandler userLoginEvent;

        ///Event For Register
        public delegate void RegisteredEventHandler(object sender, Registered e);
        public event RegisteredEventHandler userRegisterEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogInControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button logic to LogIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {

            if(this.txtBUserName.Text!=null && this.txtBUserPassword.Password!=null)
            {
                CheckUser(this.txtBUserName.Text.ToString());
            }
        }

        /// <summary>
        /// Check if the user exists in the data base if not throws new
        /// <see cref="UserNotFoundException"/>
        /// </summary>
        /// <param name="name"></param>
        private void CheckUser(string name)
        {

            try
            {
                myDb = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");

                _tryUser = myDb.GetUserByName(name);

                //BCrypt.Net.BCrypt.HasPassword(string,salt)
                //.verify(contraseña_textbox, contraseña_db
                if (_tryUser.Password.Equals(this.txtBUserPassword.Password))
                {

                    userLoginEvent(this, new LoggedIn(true, _tryUser));

                }
                else
                {
                    this.lblMsgError.Content = ERROR_STRING;
                }

            }catch(UserNotFoundException ex)
            {
                this.lblMsgError.Content = ERROR_STRING;
            }
        }

        /// <summary>
        /// Logic for Close Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// Event for reset the UserPasswordBox/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBUserPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.lblMsgError.Content="";
        }

        /// <summary>
        /// Logic for register button. throws a new event:
        /// <see cref="Registered"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            userRegisterEvent(this, new Registered(true));
        }

    }
}
