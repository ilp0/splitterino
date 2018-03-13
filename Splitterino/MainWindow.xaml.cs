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
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        Game g = new Game("Sly 3", "PS2");
        bool runInProgress = false;
        int splitCountBuffer = 0;
        public MainWindow()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            instance = this;
            Debug.WriteLine(Directory.GetCurrentDirectory());
            SPLT.ReadAndPrint(System.IO.Directory.GetCurrentDirectory() + "\\Data\\" + g.GetName() + ".splt");
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
        private void Startbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (!runInProgress)
            {
                Run curRun = new Run();
                curRun.game = g;
                sw.Start();
                dt.Start();
                Startbtn.IsEnabled = false;
            }
            
        }
        /// <summary>
        /// Stop Timer Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stopbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            } else
            {
                sw.Start();
            }
        }
        /// <summary>
        /// Split Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Splitbtn_Click(object sender, RoutedEventArgs e)
        {
            elapsedtimeitem.Items.Add(currentTime);
            ScrollSplitViewToBottom();
            splitCountBuffer++;
            if (splitCountBuffer >= Splititemlist.Items.Count)
            {
                sw.Stop();
                Splitbtn.IsEnabled = false;
                Stopbtn.IsEnabled = false;
                splitCountBuffer = 0;
            }
        }
        /// <summary>
        /// Reset Timer Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Resetbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();
            MainTimerDisplay.Text = "00:00:00";
            Startbtn.IsEnabled = true;
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
            foreach (Split s in cat.SplitList)
            {
                Splititemlist.Items.Add(s.GetTitle());
            }
            
        }

        private void Splititemlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ScrollSplitViewToBottom()
        {
            elapsedtimeitem.SelectedIndex = elapsedtimeitem.Items.Count - 1;
            elapsedtimeitem.ScrollIntoView(elapsedtimeitem.SelectedItem);
            Splititemlist.SelectedIndex = splitCountBuffer + 1;
            Splititemlist.ScrollIntoView(Splititemlist.SelectedItem);

        }
    }
}

