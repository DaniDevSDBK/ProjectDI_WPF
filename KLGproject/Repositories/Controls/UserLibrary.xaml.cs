using KLGproject.Repositories.AppObjects;
using KLGproject.Resources.Libraries.API.Lib;
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
    /// Lógica de interacción para UserLibrary.xaml
    /// </summary>
    public partial class UserLibrary : UserControl
    {

        private API _API = new API();
        private User _currentUser = new User();

        /// <summary>
        /// Constructor for <see cref="UserLibrary"/>
        /// </summary>
        /// <param name="_currentUser"></param>
        public UserLibrary(User _currentUser)
        {
            InitializeComponent();
            this._currentUser = _currentUser;
            this._currentUser.LibraryChanged += OnLibraryChanged;
            SetGameList();
        }

        /// <summary>
        /// Set the game users list
        /// </summary>
        public async void SetGameList()
        {
            List<Game> _updatedList = new List<Game>();

            if (_currentUser.Library != null)
            {
                foreach (var item in _currentUser.Library)
                {

                    _updatedList.Add(await _API.GetGameById(item));

                }
            }

            this.List.ItemsSource = _updatedList;

        }

        /// <summary>
        /// If the library change if update it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLibraryChanged(object sender, EventArgs e)
        {
            SetGameList();
        }
    }
}
