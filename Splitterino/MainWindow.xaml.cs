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

namespace Splitterino
{
    public partial class MainWindow : Window
    {

        //timer
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

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

        private void Startbtn_Click_1(object sender, RoutedEventArgs e)
        {
            sw.Start();
            dt.Start();
        }

        private void Stopbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
            elapsedtimeitem.Items.Add(currentTime);
        }

        private void Resetbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();
            MainTimerDisplay.Text = "00:00:00";
        }
    }
}

