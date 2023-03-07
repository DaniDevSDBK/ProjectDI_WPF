using KLGproject.Repositories.AppObjects;
using KLGproject.Repositories.CustomExceptions;
using KLGproject.Resources.Libraries.API.Lib;
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

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para NewComersControl.xaml
    /// </summary>
    public partial class NewComersControl : UserControl
    {

        private List<Game> _games= new List<Game>();
        private API _API = new API();
        private User _currentUser = new User();
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");

        /// <summary>
        /// Constructor for <see cref="NewComersControl"/>
        /// </summary>
        /// <param name="_currentUser"></param>
        public NewComersControl(User _currentUser)
        {
            InitializeComponent();
            InitializeGameList();
            this._currentUser = _currentUser;
        }

        /// <summary>
        /// Initialize the Game List <see cref="API.GetGames()"/> and filters it in a query getting the 10 newest games
        /// </summary>
        private async void InitializeGameList()
        {

            _games = await _API.GetGames();
            this.List.ItemsSource = _games.Where(game => !_currentUser.Library.Contains(game.id)).ToList().OrderByDescending(x => x.release_date).Take(10);
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
            this.List.ItemsSource = _games.Where(x => !_currentUser.Library.Contains(x.id)).ToList().OrderByDescending(x => x.release_date).Take(10);
            
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
                    this.UpadateGameRating(game);
                    db.UpdateUserGame(_currentUser.Id, game.id, true, false);
                    this.List.Items.Refresh();

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
                    this.UpadateGameRating(game);
                    db.UpdateUserGame(_currentUser.Id, game.id, false, true);
                    this.List.Items.Refresh();

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

                this.db.ExecuteNonQuery("UPDATE GAMESRATING SET LIKES=@Likes, DISLIKES=@DisLikes WHERE GAMEID=@GameId", parameters);

            }
            catch (GameNotFoundException ex)
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
