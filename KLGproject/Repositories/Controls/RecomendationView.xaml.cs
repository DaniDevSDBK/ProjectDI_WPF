using KLGproject.Repositories.AppObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
using System.Windows.Media.Animation;
using KLGproject.Resources.Libraries.API.Lib;
using KLGproject.Resources.Libraries.DB.Lib;

namespace KLGproject.Repositories.Controls
{
    /// <summary>
    /// Lógica de interacción para RecomendationView.xaml
    /// </summary>
    public partial class RecomendationView : UserControl
    {
        private List<Game> _gameList = new List<Game>();
        private int _contador = 0;
        private Timer _timer;
        private Storyboard _storyboard;
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");
        private API _API = new API();

        /// <summary>
        /// Constructor for <see cref="RecomendationView"/>
        /// </summary>
        public RecomendationView()
        {
            InitializeComponent();

            _timer = new Timer(10000);
            _timer.Elapsed += OnTimedEvent;
            InitializeUserRecomendationList();

            _timer.Start();
        }

        /// <summary>
        /// Initializes the recomendation list <see cref="List{Game}"/> by creating a temp List by <see cref="SQLite.GetMoreLikedGames()"/><br/>
        /// And getting this games with <see cref="API.GetGameById(int)"/><br/>
        /// Sets the current View by <see cref="SetCurrentGameView(int)"/><br/>
        /// Sarts an active reload by using a <see cref="Task"/> to do the previous code
        /// </summary>
        private async void InitializeUserRecomendationList()
        {

            var tempGamesIdList = db.GetMoreLikedGames();

            if (tempGamesIdList != null)
            {
                foreach (var item in tempGamesIdList)
                {

                    _gameList.Add(await _API.GetGameById(item));

                }
            }

            SetCurrentGameView(_contador);

            //Setting an active reload to know if the more liked games has changed
            await Task.Run(async () =>
            {
                while (true)
                {
                    // Wait for some time before checking again (1 minute in this case)
                    await Task.Delay(TimeSpan.FromMinutes(1));

                    // Get the updated list of most liked games
                    var updatedGamesIdList = db.GetMoreLikedGames();

                    // If the list has changed, update the view
                    if (!tempGamesIdList.SequenceEqual(updatedGamesIdList))
                    {
                        // Clear the current list of games and add the new ones
                        _gameList.Clear();
                        foreach (var item in updatedGamesIdList)
                        {
                            _gameList.Add(await _API.GetGameById(item));
                        }

                        //Set to 0 the count
                        _contador = 0;

                        // Update the current game view
                        SetCurrentGameView(_contador);

                        // Update the temporary list with the new list of games
                        tempGamesIdList = updatedGamesIdList;
                    }
                }
            });

        }

        /// <summary>
        /// Logic for GoBack button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e) => SetCurrentGameView(_contador-- <= 0 ? _contador = _gameList.Count - 1 : _contador--);

        /// <summary>
        /// Logic for GoBack button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoForeward_Click(object sender, RoutedEventArgs e) => SetCurrentGameView(_contador++ % _gameList.Count);

        /// <summary>
        /// Set a timer to change the current view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                SetCurrentGameView(_contador++ % _gameList.Count);

                // Create a storyboard animation to fade out the current game view
                var storyboard = new Storyboard();
                var animation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(2.0)));
                Storyboard.SetTarget(animation, this.BorderGame);
                Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
                storyboard.Children.Add(animation);

                // Hook up a Completed event handler to the storyboard
                storyboard.Completed += (s, args) =>
                {
                    // Update the game view and fade it back in
                    SetCurrentGameView(_contador % _gameList.Count);
                    var storyboard2 = new Storyboard();
                    var animation2 = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromSeconds(2.0)));
                    Storyboard.SetTarget(animation2, this.BorderGame);
                    Storyboard.SetTargetProperty(animation2, new PropertyPath(UIElement.OpacityProperty));
                    storyboard2.Children.Add(animation2);
                    storyboard2.Begin();
                };

                // Start the storyboard animation
                storyboard.Begin();
            });

        }

        /// <summary>
        /// Redirects the user to the oficial page where he can download the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownloadk_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _gameList[_contador].freetogame_profile_url,
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
                this.txtTitle.Text = _gameList[index].title;
                this.txtGenre.Text = _gameList[index].genre;
                this.txtPublisher.Text = _gameList[index].publisher;
                this.txtDeveloper.Text = _gameList[index].developer;
                this.txtDate.Text = _gameList[index].release_date;
                this.txtShortDescription.Text = _gameList[index].short_description;
                this.ThumbnailImg.Source = new BitmapImage(new Uri(_gameList[index].thumbnail));
            });
        }
    }
}
