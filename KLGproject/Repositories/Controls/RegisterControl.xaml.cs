using KLGproject.Repositories.CustomEvents;
using KLGproject.Repositories.CustomExceptions;
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

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {

        private SQLite myDb;
        private User _tryUser;
        private ToolTip toolTip = new ToolTip();

        //Event For Register
        public delegate void RegisterGoBackEventHandler(object sender, RegisterGoBack e);
        public event RegisterGoBackEventHandler goBackEvent;

        /// <summary>
        /// Constructor for <see cref="RegisterControl"/>
        /// </summary>
        public RegisterControl()
        {
            InitializeComponent();
            toolTip.Content = "Please accept the terms and conditions to register.";
            RegisterButton.ToolTip = toolTip;
        }

        /// <summary>
        /// If the Checkbox is checked the button will be enabled and the tooltip deleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.RegisterButton.IsEnabled = true;
            this.RegisterButton.ToolTip = null; 
        }

        /// <summary>
        /// If the checkbox is unchecked the tooltip will be <see cref="toolTip"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RegisterButton.ToolTip = toolTip;
        }

        /// <summary>
        /// Calculates the strong of the password in order the length of its self
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private int CalculatePasswordStrength(string password)
        {

            if (password.Length < 4)
            {
                return 0;
            }
            else if (password.Length < 8)
            {
                return 25;
            }
            else if (password.Length < 12)
            {
                return 50;
            }
            else if (password.Length < 16)
            {
                return 75;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// If the password change it recalculates the strenght of it<br/>
        /// <see cref="CalculatePasswordStrength(string)"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

            this.ProgressBar.Value = (int)CalculatePasswordStrength(this.PasswordBox.Password);
        }

        /// <summary>
        /// The logic of the register button<br/>
        /// <see cref="CheckUser(string)"/><br/>
        /// <see cref="UserAlreadyExist"/>br>
        /// <see cref="Register"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

            if(this.ckbTermsConditions.IsChecked == true)
            {
                this.RegisterButton.ToolTip = null;

                bool res=CheckUser(this.txtBUserName.Text)? UserAlreadyExist() : Register();

            }
        }

        /// <summary>
        /// Logic for the button goback, it will redirect the user to the LogIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            this.txtBUserName.Text = "";
            this.txtBEmail.Text = "";
            this.PasswordBox.Password = "";

            goBackEvent(this, new RegisterGoBack(true));
        }

        /// <summary>
        /// Logic for the register
        /// </summary>
        /// <returns></returns>
        private bool Register()
        {

            if(this.txtBUserName.Text!= null && this.txtBEmail.Text!=null && this.PasswordBox.Password!=null) 
            {

                this.txtErrortb.Text = "Registering...";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Username", this.txtBUserName.Text);
                parameters.Add("@Password", BCrypt.Net.BCrypt.HashPassword(this.PasswordBox.Password, 11));
                parameters.Add("@AccessLevel", 0);

                myDb.ExecuteNonQuery("INSERT INTO USER (NAME, PWD, TIPO) VALUES (@Username, @Password, @AccessLevel)", parameters);

                this.txtBUserName.Text = "";
                this.txtBEmail.Text = "";
                this.PasswordBox.Password = "";

                goBackEvent(this, new RegisterGoBack(true));

                return true;

            }
            else
            {

                this.txtErrortb.Text = "Fields are empty or not correct.";
                return false;
            }
        }

        /// <summary>
        /// Gets the user from the database in order to know if already exists<br/>
        /// <see cref="SQLite.GetUserByName(string)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckUser(string name)
        {

            try
            {
                myDb = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");

                _tryUser = myDb.GetUserByName(name);

                return true;

            }
            catch (UserNotFoundException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Stablish the error message if the user already exists
        /// </summary>
        /// <returns></returns>
        private bool UserAlreadyExist()
        {
            this.txtErrortb.Text = "Este Usuario ya existe.";

            return false;
        }
    }
}
