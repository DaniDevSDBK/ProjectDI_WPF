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
using KLGproject.Resources.Libraries.DB.Lib;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : UserControl
    {
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");
        private User _currentUser = new User();
        public AdminPanel(User _currentUser)
        {
            InitializeComponent();
            this._currentUser = _currentUser;
            UpdateUsersList();
        }
        private void EliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User) this.Users.SelectedItem;
            if (selectedUser != null)
            {
                db.DeleteUserFromDataBase(selectedUser);
                UpdateUsersList();
            }
        }

        /// <summary>
        /// Update the user list
        /// </summary>
        private void UpdateUsersList() => this.Users.ItemsSource = db.GetAllUsers().Where(x => x.Id != _currentUser.Id).ToList();

    }
}
