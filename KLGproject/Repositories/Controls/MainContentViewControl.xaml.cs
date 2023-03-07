using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.CustomExceptions;
using KLGproject.Resources.Libraries.API.Lib;
using KLGproject.Resources.Libraries.DB.Lib;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para MainContentViewControl.xaml
    /// </summary>
    public partial class MainContentViewControl : UserControl {

        private List<Game> _carouselItems = new List<Game>();
        private API _API = new API();
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");
        private User _currentUser = new User();
        private Dictionary<Int32, bool> liked_disliked = new Dictionary<int, bool>();

        /// <summary>
        /// Constructor for <see cref="MainContentViewControl"/>
        /// </summary>
        /// <param name="_currentUser"></param>
        public MainContentViewControl(User _currentUser)
        {

            InitializeComponent();
            this._currentUser = _currentUser;
            InitializeGameList();
            
        }

        /// <summary>
        /// Initialize the game List using the method <see cref="API.GetGames()"/>
        /// </summary>
        private async void InitializeGameList()
        {

            _carouselItems = await _API.GetGames();
            
            if(_currentUser.Library!=null) 
            {

                this.List.ItemsSource = _carouselItems.Where(game => !_currentUser.Library.Contains(game.id)).ToList();

            }
            else
            {

                this.List.ItemsSource = _carouselItems;

            }
        }

        /// <summary>
        /// Adds a new Item to the users Library <see cref="UserLibrary"/> and removed it from the <see cref="MainContentViewControl"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLibraryItem_Click(object sender, RoutedEventArgs e)
        {

            var _currentGame = ((Game)this.List.SelectedItem);

            this._currentUser.Library.Add(_currentGame.id);
            this.db.AddToUserLibrary(_currentUser.Id, _currentGame.id);
            this.List.ItemsSource = _carouselItems.Where(game => !_currentUser.Library.Contains(game.id)).ToList();

        }

        /// <summary>
        /// Redirects the user to the current game url using the System Default Browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadItem_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = ((Game)this.List.SelectedItem).freetogame_profile_url,
                UseShellExecute = true
            });

        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToMyWishesList_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //Future Implementation
        }

        /// <summary>
        /// Sets a like to this game from this user, if the users had already liked the user the next like would not affect to the game rating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LikeItem_Click(object sender, RoutedEventArgs e)
        {

            if (this.List.SelectedItem != null)
            {
                var game = (Game)this.List.SelectedItem;

                var (liked, _) = db.GetUserGameStatus(_currentUser.Id, game.id);
                if (!liked)
                {
                    game.Likes++;
                    game.Liked = true;
                    game.Disliked = false;
                    this.UpadateGameRating(game);
                    this.db.SetUserGame(_currentUser.Id, game.id, game.Liked, game.Disliked);
                    this.List.Items.Refresh();

                }
                else
                {

                    game.DisLikes--;

                }

            }

        }

        /// <summary>
        /// Sets a dislike to this game from this user, if the users had already disliked the user the next like would not affect to the game rating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DislikeItem_Click(object sender, RoutedEventArgs e)
        {

            if (this.List.SelectedItem != null)
            {
                var game = (Game)this.List.SelectedItem;

                var (disliked, _) = db.GetUserGameStatus(_currentUser.Id, game.id);
                if (!disliked)
                {

                    game.DisLikes++;
                    game.Liked = false;
                    game.Disliked = true;
                    this.UpadateGameRating(game);
                    this.db.SetUserGame(_currentUser.Id, game.id, game.Liked, game.Disliked);
                    this.List.Items.Refresh();

                }
                else
                {

                    game.Likes--;

                }

            }

        }

        /// <summary>
        /// Updates the game rating <see cref="SQLite.GetGameIfExists(int)"/>
        /// </summary>
        /// <param name="game"></param>
        private void UpadateGameRating(Game game)
        {
            try
            {

                var gameToUpdate = db.GetGameIfExists(game.id);
                var parameters = new Dictionary<string, object>();
                parameters.Add("@Likes", gameToUpdate.Likes);
                parameters.Add("@DisLikes", gameToUpdate.DisLikes);
                parameters.Add("@GameId", gameToUpdate.id);

                db.ExecuteNonQuery("UPDATE GAMESRATING SET LIKES=@Likes, DISLIKES=@DisLikes WHERE GAMEID=@GameId", parameters);

            }
            catch(GameNotFoundException ex)
            {


                var parameters = new Dictionary<string, object>();
                parameters.Add("@Likes", game.Likes);
                parameters.Add("@DisLikes", game.DisLikes);
                parameters.Add("@GameId", game.id);

                db.ExecuteNonQuery("INSERT INTO GAMESRATING VALUES(@GameId, @Likes, @DisLikes)", parameters);

            }

        }

    }

}
