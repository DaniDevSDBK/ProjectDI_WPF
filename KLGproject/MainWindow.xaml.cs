using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
using KLGproject.Repositories.Controls;
using KLGproject;
using Newtonsoft.Json;
using KLGproject.Repositories.CustomEvents;
using Google.Protobuf.WellKnownTypes;
using System.Windows.Forms;
using KLGproject.Resources.Libraries.DB.Lib;

namespace KLGproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TabItem> tabs = new List<TabItem>();
        private List<Game> _gameList = new List<Game>();
        protected User _currentUser = new User();
        private SQLite db = new SQLite("Data Source = ../../../Resources/DB/DB.db; Version = 3;");

        /// <summary>
        /// MainWindow Constructor
        /// </summary>
        /// <param name="currentUser"></param>
        public MainWindow(User currentUser)
        {

            InitializeComponent();

            this.Menu.menuSelection += SelectOption;

            this._currentUser = currentUser;
            this.lblCurrentUserName.Content = currentUser.Name;
            CheckUserStatus();
        }

        private void CheckUserStatus()
        {

            if (this._currentUser.Status == 1)
            {
                this.Menu.AdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                this.Menu.AdminPanel.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Logic for the closession
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Logic to allow the user to move the window when holding the left mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                DragMove();
            }
        }

        /// <summary>
        /// Selects the option of the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="opc"></param>
        private void SelectOption(object sender, MenuSelectionEvent opc)
        {

            switch (opc.ButtonName)
            {
                case "Home":

                    if(!tabs.Any(x => x.Name.Equals("tb"+ opc.ButtonName)))
                    {
                        this.WindowState= WindowState.Maximized;
                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new MainContentViewControl(_currentUser));

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName)).IsSelected = true;

                    }

                    break;
                
                case "Newcomers":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName)))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new NewComersControl(_currentUser));

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName)).IsSelected = true;

                    }

                    break;
                
                case "Find My Game":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new FindGame());

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))).IsSelected = true;

                    }

                    break;
                
                case "My Library":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new UserLibrary(_currentUser));

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))).IsSelected = true;

                    }

                    break;
                
                case "Settings":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName)))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new SettingsControl(_currentUser));

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName)).IsSelected = true;

                    }

                    break;
                
                case "Help":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName)))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new HelpUserControl());

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName)).IsSelected = true;

                    }

                    break;
                
                case "Admin Panel":

                    if (!tabs.Any(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))))
                    {

                        CreateTabItem("tb" + opc.ButtonName, opc.ButtonName, new AdminPanel(_currentUser));

                    }
                    else
                    {

                        tabs.FirstOrDefault(x => x.Name.Equals("tb" + opc.ButtonName.Replace(' ', '_'))).IsSelected = true;

                    }

                    break;
            }

        }

        /// <summary>
        /// Creates a tabItem
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="tabHeader"></param>
        /// <param name="object"></param>
        private void CreateTabItem(String tabName, String tabHeader, object @object)
        {

            //creamos la ventana
            TabItem tb = new TabItem();
            tb.Name = tabName.Replace(' ','_');
            tb.Header = tabHeader;
            tb.Content = @object;
            tb.IsSelected = true;

            //addList
            this.tabs.Add(tb);
            this.MyTabControl.Items.Add(tabs.Last());

        }

        /// <summary>
        /// Minimize the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMinimize_Click(object sender, RoutedEventArgs e)=> this.WindowState = WindowState.Minimized;

        /// <summary>
        /// Maximized the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.btnMaximize.ToolTip = "Maximize this Window.";
                this.WindowState = WindowState.Normal;
            }
            else
            {

                this.btnMaximize.ToolTip = "Return this Window its Normal Satate.";
                this.WindowState = WindowState.Maximized;

            }
        }

        /// <summary>
        /// Shut down the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)=> App.Current.Shutdown();
        /// <summary>
        /// Expands or compress the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandMenu(object sender, ExecutedRoutedEventArgs e) => this.Menu.ExpanderMenu.IsExpanded = this.Menu.ExpanderMenu.IsExpanded ? false : true;
        /// <summary>
        /// GoForeward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoForeward(object sender, ExecutedRoutedEventArgs e)=> _ = (this.MyTabControl.SelectedIndex < this.MyTabControl.Items.Count - 1) ? this.MyTabControl.SelectedIndex++ : this.MyTabControl.SelectedIndex;
        /// <summary>
        /// Goes Backward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackward(object sender, ExecutedRoutedEventArgs e)=> _ = (this.MyTabControl.SelectedIndex > 0) ? this.MyTabControl.SelectedIndex-- : this.MyTabControl.SelectedIndex;

        /// <summary>
        /// Goes Home or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home(object sender, ExecutedRoutedEventArgs e)
        {

            if (!tabs.Any(x => x.Name.Equals("tbHome")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbHome", "Home", new MainContentViewControl(_currentUser));

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbHome")).IsSelected = true;

            }

        }

        /// <summary>
        /// Goes Newcomers or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewComers(object sender, ExecutedRoutedEventArgs e)
        {

            if (!tabs.Any(x => x.Name.Equals("tbNewcomers")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbNewcomers", "Newcomers", new NewComersControl(_currentUser));

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbNewcomers")).IsSelected = true;

            }

        }

        /// <summary>
        /// Goes Users Library or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyLibrary(object sender, ExecutedRoutedEventArgs e)
        {

            if (!tabs.Any(x => x.Name.Equals("tbMy_Library")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbMy_Library", "My Library", new UserLibrary(_currentUser));

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbMy_Library")).IsSelected = true;

            }

        }

        /// <summary>
        /// Goes FindMyGmae menu option or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindMyGame(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            if (!tabs.Any(x => x.Name.Equals("tbFind_My_Game")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbFind_My_Game", "Find My Game", new FindGame());

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbFind_My_Game")).IsSelected = true;

            }

        }

        /// <summary>
        /// Goes Settings or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings(object sender, ExecutedRoutedEventArgs e)
        {

            if (!tabs.Any(x => x.Name.Equals("tbSettings")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbSettings", "Settings", new SettingsControl(_currentUser));

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbSettings")).IsSelected = true;

            }

        }

        /// <summary>
        /// Goes Help or creates if it doesnt exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help(object sender, ExecutedRoutedEventArgs e)
        {

            if (!tabs.Any(x => x.Name.Equals("tbHelp")))
            {
                this.WindowState = WindowState.Maximized;
                CreateTabItem("tbHelp", "Help", new HelpUserControl());

            }
            else
            {

                tabs.FirstOrDefault(x => x.Name.Equals("tbHelp")).IsSelected = true;

            }

        }


    }

}
