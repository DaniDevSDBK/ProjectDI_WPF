using KLGproject.Repositories.AppObjects;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {

        private User _currentUser;
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");

        /// <summary>
        /// Constroctor for <see cref="SettingsControl"/>
        /// </summary>
        /// <param name="_currentUser"></param>
        public SettingsControl(User _currentUser)
        {
            InitializeComponent();
            this._currentUser = _currentUser;
            this.txtUserName.Text = _currentUser.Name;
            this.txtEmail.Text = _currentUser.Email;
            this.txtPassword.Password = "";
        }

        /// <summary>
        /// Check the database and updates the fields for the current user id checking if the name already exits in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if(this.txtUserName.Text==null||this.txtEmail.Text==null||this.txtPassword.Password==null) 
                {
                    
                    this.txtUserName.Text = _currentUser.Name;
                    this.txtEmail.Text = _currentUser.Email;
                    this.txtPassword.Password = "";
                    this.ErrorLabel.Content = "Fields are Empty. Please, fill the information correctly.";

                }
                else
                {

                    if(!db.UserExists(this.txtUserName.Text))
                    {

                        this.db.UpdateUser(_currentUser.Id, this.txtUserName.Text, this.txtEmail.Text, BCrypt.Net.BCrypt.HashPassword(this.txtPassword.Password, 11));
                        this.ErrorLabel.Content = "";
                        this.txtUserName.Text = _currentUser.Name;
                        this.txtEmail.Text = _currentUser.Email;
                        this.txtPassword.Password = "";

                    }
                    else
                    {

                        this.ErrorLabel.Content = "The User Name Already Exists.";
                        this.ErrorLabel.Content = "";
                        this.txtUserName.Text = _currentUser.Name;
                        this.txtEmail.Text = _currentUser.Email;
                        this.txtPassword.Password = "";
                    }

                }

            }catch(UserNotFoundException ex)
            {
                this.ErrorLabel.Content = "User not Found.";
            }    

        }
    }
}
