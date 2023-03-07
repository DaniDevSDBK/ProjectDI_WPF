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
    /// Lógica de interacción para FindGame.xaml
    /// </summary>
    public partial class FindGame : UserControl
    {
        private API _API = new API();
        private List<Game> _filteredGames = new List<Game>();
        private int _contador = 0; 

        /// <summary>
        /// Constructor
        /// </summary>
        public FindGame()
        {
            InitializeComponent();
            this.cbCategory.SelectedIndex = 0;
            this.cbPlatform.SelectedIndex = 0;
        }

        /// <summary>
        /// Button to search the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem cbiCategory = (ComboBoxItem)this.cbCategory.SelectedItem;
            ComboBoxItem cbiPlatform = (ComboBoxItem)this.cbPlatform.SelectedItem;

            string category = cbiCategory.Content.ToString().ToLower();
            string platform = cbiPlatform.Content.ToString().ToLower();

            if ( category!= "none" &&  platform!="none")
            {

               _filteredGames = await _API.GetGamesByPlatformAndCategory(platform, category);

            }else if(category!="none" && platform == "none")
            {

                _filteredGames = await _API.GetGamesByCategory(category);

            }else if(category == "none" && platform != "none")
            {

                _filteredGames = await _API.GetGamesByPlatform(platform);

            }
            else
            {

                this.txtShortDescription.Text = "You must select some option in order to search the content you want";

            }

            this.SetCurrentGameView(_contador);
            
        }

        /// <summary>
        /// Button to navigate throught the game list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e) => SetCurrentGameView(_contador-- <= 0 ? _contador = _filteredGames.Count - 1 : _contador--);

        /// <summary>
        /// Button to navigate throught the game list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoForeward_Click(object sender, RoutedEventArgs e) => SetCurrentGameView(_contador++ % _filteredGames.Count);

        /// <summary>
        /// Button to download the current game in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownloadk_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _filteredGames[_contador].freetogame_profile_url,
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Sets the view to the current item in the index
        /// </summary>
        /// <param name="index"></param>
        private void SetCurrentGameView(int index)
        {
            //If we dont use Dispatcher.Invoke() will give us a subprocess error, cause the owner is the process that use this method first.
            Dispatcher.Invoke(() =>
            {
                this.txtTitle.Text = _filteredGames[index].title;
                this.txtGenre.Text = _filteredGames[index].genre;
                this.txtPublisher.Text = _filteredGames[index].publisher;
                this.txtDeveloper.Text = _filteredGames[index].developer;
                this.txtDate.Text = _filteredGames[index].release_date;
                this.txtShortDescription.Text = _filteredGames[index].short_description;
                this.ThumbnailImg.Source = new BitmapImage(new Uri(_filteredGames[index].thumbnail));
            });
        }
    }
}
