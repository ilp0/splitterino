using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using System.IO;

namespace Splitterino
{
    //main
    public partial class MainWindow : Window
    {
        //Instance for outside use.
        public static MainWindow instance;
        //timer stuff
        public DispatcherTimer dt = new DispatcherTimer();
        public Stopwatch sw = new Stopwatch();
        public string currentTime = string.Empty;
        public Game g = new Game("Sly 3", "PS2");
        //Category category = g.CategoryList[0];
        public bool runInProgress = false;
        public int splitCountBuffer = 0;
        string filename = "";
        public MainWindow()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            instance = this;
            Debug.WriteLine(Directory.GetCurrentDirectory());
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
        }

        /// <summary>
        /// DispatchTimer Tick, for stopwatch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}.{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                MainTimerDisplay.Text = currentTime;
            }
        }
        /// <summary>
        /// Start Timer Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startbtn_Click(object sender, RoutedEventArgs e)
        {
            RunManager.TimerStart();
        }
        /// <summary>
        /// Stop Timer Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stopbtn_Click(object sender, RoutedEventArgs e)
        {
            RunManager.StopButtonClick();
        }
        /// <summary>
        /// Split Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Splitbtn_Click(object sender, RoutedEventArgs e)
        {
            RunManager.Split();
        }
        /// <summary>
        /// Reset Timer Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Resetbtn_Click(object sender, RoutedEventArgs e)
        {
            RunManager.Reset();
        }

        /// <summary>
        /// Updates Game, console, category and splits.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cat"></param>
        public void UpdateGUI(Game g, Category cat)
        {
            
            GameTitle.Text = g.GetName();
            ConsoleTitle.Text = g.GetConsole();
            CategoryTitle.Text = cat.Name;
            if (SPLT.LoadedGame != null)
            {
                SOBTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].SOBTime);
                PBTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].PersonalBest);
                TargetTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].TargetTime);
            }
            
            foreach (Split s in cat.SplitList)
            {
                Splititemlist.Items.Add(s.GetTitle());
            }
            RunManager.WriteTargetTime();
        }

        private void Splititemlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void ScrollSplitViewToBottom()
        {
            elapsedtimeitem.SelectedIndex = elapsedtimeitem.Items.Count - 1;
            elapsedtimeitem.ScrollIntoView(elapsedtimeitem.SelectedItem);
            Splititemlist.SelectedIndex = splitCountBuffer + 1;
            Splititemlist.ScrollIntoView(Splititemlist.SelectedItem);

        }

        public void ScrollSplitViewToTop()
        {
            elapsedtimeitem.SelectedIndex = 0;
            elapsedtimeitem.ScrollIntoView(elapsedtimeitem.SelectedItem);
            Splititemlist.SelectedIndex = 0;
            Splititemlist.ScrollIntoView(Splititemlist.SelectedItem);
        }

        private void LoadSplitBtn_Click(object sender, RoutedEventArgs e)
        {
            RunManager.ClearUI();
            SPLT.ReadAndPrint(filename);
            RunManager.WriteTargetTime();
            //SPLT.ReadAndPrint(System.IO.Directory.GetCurrentDirectory() + "\\Data\\" + g.GetName() + ".splt");
            
        }

        private void SelectSplitBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Data\\";
            // Set filter for file extension and default file extensi on 
            dialog.DefaultExt = ".splt";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dialog.FileName;
                SplitFileName.Text = filename;
            
            }
        }

        private void NewSplitsBtn_Click(object sender, RoutedEventArgs e)
        {
            Window1 SplitWindow = new Window1();
            SplitWindow.Show();
        }
    }
}

