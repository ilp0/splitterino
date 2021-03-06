﻿using System;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Runtime.Serialization.Formatters.Binary;

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

        public int setHotkeyState = 0;

        public MainWindow()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            instance = this;
            Preferences.Deserialize();
            Preferences.LoadPreferences();
            Debug.WriteLine(Directory.GetCurrentDirectory());
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);

			set_split_key.Content = KeyInterop.KeyFromVirtualKey((int)KeyConfig.Config.SplitHK).ToString();
			set_start_key.Content = KeyInterop.KeyFromVirtualKey((int)KeyConfig.Config.StartHK).ToString();
			set_reset_key.Content = KeyInterop.KeyFromVirtualKey((int)KeyConfig.Config.ResetHK).ToString();


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
                currentTime = SPLT.TimeSpanToString(ts, false);
                //TODO
                //mitä vittua
                //CurrentRunCmprListbox.Items.Add(SPLT.TimeSpanToString(SPLT.CompareTS(sw.Elapsed, SPLT.CountTotalTime(SPLT.LoadedGame.CategoryList[0].PBSplits.GetRange(0, RunManager.CurrentSplitIndex + 1))), true));
                if (SPLT.LoadedGame.CategoryList[0].PBSplits.Count > RunManager.CurrentSplitIndex)
                {
                    List<Split> a = SPLT.LoadedGame.CategoryList[0].PBSplits.GetRange(0, RunManager.CurrentSplitIndex + 1);
                    if (RunManager.CurrentSplitIndex < SPLT.LoadedGame.CategoryList[0].PBSplits.Count)
                    {
                        CurrentRunCmprListbox.Items[RunManager.CurrentSplitIndex] = SPLT.TimeSpanToString(SPLT.CompareTS(sw.Elapsed, SPLT.CountTotalTime(a)), true);
                    }
                }
                //CurrentRunCmprListbox.Items[RunManager.CurrentSplitIndex] = CurrentRunCmprListbox.Items[RunManager.CurrentSplitIndex];
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
            MainWindow.instance.Startbtn.IsEnabled = true;
            MainWindow.instance.Resetbtn.IsEnabled = true;
            MainWindow.instance.Splitbtn.IsEnabled = true;
            MainWindow.instance.Stopbtn.IsEnabled = true;
            GameTitle.Text = g.GetName();
            ConsoleTitle.Text = g.GetConsole();
            CategoryTitle.Text = cat.Name;
            if (SPLT.LoadedGame != null)
            {
                SOBTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].SOBTime, false);
                PBTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].PersonalBest, false);
                TargetTimeText.Text = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].TargetTime, false);
            }
            Splititemlist.Items.Clear();
            foreach (Split s in cat.SplitList)
            {
                Splititemlist.Items.Add(s.GetTitle());
            }
            switch (Preferences.DefaultComparisonSplits)
            {
                case 1:
                    RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].PBSplits);
                    break;
                case 2:
                    RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].TargetSplits);
                    break;
                case 3:
                    RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].SOBSplits);
                    break;
            }
        }

        private void Splititemlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void ScrollSplitViewToBottom()
        {
            /*elapsedtimeitem.SelectedIndex = elapsedtimeitem.Items.Count - 1;
            elapsedtimeitem.ScrollIntoView(elapsedtimeitem.SelectedItem);
            Splititemlist.SelectedIndex = splitCountBuffer + 1;
            Splititemlist.ScrollIntoView(Splititemlist.SelectedItem);
            */
        }

        public void ScrollSplitViewToTop()
        {
            /*
            elapsedtimeitem.SelectedIndex = 0;
            elapsedtimeitem.ScrollIntoView(elapsedtimeitem.SelectedItem);
            Splititemlist.SelectedIndex = 0;
            Splititemlist.ScrollIntoView(Splititemlist.SelectedItem);
            */
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
                Debug.WriteLine(filename);
                RunManager.ClearUI();
                SPLT.ReadAndPrint(filename);
                //SPLT.ReadAndPrint(System.IO.Directory.GetCurrentDirectory() + "\\Data\\" + g.GetName() + ".splt");
            }
            
        }

        private void NewSplitsBtn_Click(object sender, RoutedEventArgs e)
        {
            Window1 SplitWindow = new Window1(new Game(), true);
            SplitWindow.Show();
        }

        //Hotkey Stuff
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
      [In] IntPtr hWnd,
      [In] int id,
      [In] uint fsModifiers,
      [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            //RegisterHotKey(Hotkeys.StartHK);
            RegisterHotKey(KeyConfig.Config.SplitHK, 9000);
			RegisterHotKey(KeyConfig.Config.StartHK, 9001);
			RegisterHotKey(KeyConfig.Config.ResetHK, 9002);
		}

        protected override void OnClosed(EventArgs e)
        {
            Preferences.Serialize();
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey(9000);
			UnregisterHotKey(9001);
			UnregisterHotKey(9002);
			KeyConfig.Config.Save();
			base.OnClosed(e);
        }

        private void RegisterHotKey(uint Key, int hkey_id)
        {
            var helper = new WindowInteropHelper(this);
            //const uint VK_F10 = 0x79;
            if (!RegisterHotKey(helper.Handle, hkey_id, 0, Key))
            {
                // handle error
            }
        }

        private void UnregisterHotKey(int hotkey_id)
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, hotkey_id);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (setHotkeyState != 0)
            {
                Key k = Hotkeys.IsKeyDown();
				if(k == Key.Escape)
				{
					switch (setHotkeyState)
					{
						case 1:
							set_split_key.Content = "Not set";
							break;
						case 2:
							set_start_key.Content = "Not set";
							break;
						case 3:
							set_reset_key.Content = "Not set";
							break;
					}
					setHotkeyState = 0;
				}
                if (k != Key.None)
                {
                    Debug.WriteLine("Pressed key: " + k.ToString());

                    switch (setHotkeyState)
                    {
                        case 1:
							UnregisterHotKey(Hotkeys.SplitID);
							KeyConfig.Config.SplitHK = (uint)KeyInterop.VirtualKeyFromKey(k);
                            RegisterHotKey(KeyConfig.Config.SplitHK, Hotkeys.SplitID);
							set_split_key.Content = k.ToString();
                            break;
						case 2:
							UnregisterHotKey(Hotkeys.StartID);
							KeyConfig.Config.StartHK = (uint)KeyInterop.VirtualKeyFromKey(k);
							RegisterHotKey(KeyConfig.Config.StartHK, Hotkeys.StartID);
							set_start_key.Content = k.ToString();
							break;
						case 3:
							UnregisterHotKey(Hotkeys.ResetID);
							KeyConfig.Config.ResetHK = (uint)KeyInterop.VirtualKeyFromKey(k);
							RegisterHotKey(KeyConfig.Config.ResetHK, Hotkeys.ResetID);
							set_reset_key.Content = k.ToString();
							break;
					}

                    setHotkeyState = 0;

                    
                }
                
            }
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case 9000:

							if (SPLT.LoadedGame != null)
							{
								if (!runInProgress)
								{
									RunManager.TimerStart();
								}
								else
								{
									RunManager.Split();
								}
							}
                            
                            
                            handled = true;
                            break;
						case 9001:
							if (SPLT.LoadedGame != null)
							{
								if (!runInProgress)
								{
									RunManager.TimerStart();
								}
								else
								{
									RunManager.StopButtonClick();
								}
							}
							
							break;
						case 9002:
							if (SPLT.LoadedGame != null)
							{
								RunManager.Reset();
							}

							break;
					}
                    break;
            }
            return IntPtr.Zero;
        }

        private void WindowAlwaysOnTopCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Preferences.WindowAlwaysOnTop = true;
            instance.Topmost = true;
        }

        private void WindowAlwaysOnTopCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Preferences.WindowAlwaysOnTop = false;
            instance.Topmost = false;
        }

        private void SetCurrentGameDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SPLT.LoadedGame != null)
            {
                Preferences.DefaultGamePath = Directory.GetCurrentDirectory() + "//Data//Games//" + SPLT.LoadedGame.GetName() + ".splg";
            }
        }

        private void SelectDefaultGameBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Data\\";
            // Set filter for file extension and default file extensi on 
            dialog.DefaultExt = ".splg";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 

                string f = dialog.FileName;
                Preferences.DefaultGamePath = f;
                Debug.WriteLine(f + " set as default game");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxBestSplit_Selected(object sender, RoutedEventArgs e)
        {
            Preferences.DefaultComparisonSplits = 3;
            //if (SPLT.LoadedGame != null) RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].SOBSplits);
        }

        private void ComboBoxCustomTarget_Selected(object sender, RoutedEventArgs e)
        {
            Preferences.DefaultComparisonSplits = 2;
        }

        private void CombBoxPB_Selected(object sender, RoutedEventArgs e)
        {
            Preferences.DefaultComparisonSplits = 1;
            //if(SPLT.LoadedGame != null) RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].PBSplits);

        }

        private void SavePreferencesOnQuitChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Preferences.SavePrefsOnQuit = false;
        }



        private void SavePrefsBtn_Click(object sender, RoutedEventArgs e)
        {
            Preferences.Serialize();
            MessageBox.Show("Splitterino", "Preferences saved succesfully!", System.Windows.MessageBoxButton.OK);
            
        }

        private void SavePreferencesOnQuitChkBox_Checked(object sender, RoutedEventArgs e)
        {
            Preferences.SavePrefsOnQuit = true;
        }
        /*
        private void CurrentComparisonTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SPLT.LoadedGame != null)
            {
                switch (CurrentComparisonTime.SelectedIndex)
                {
                    case 0:
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].PBSplits);
                        break;
                    case 1:
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].TargetSplits);
                        break;
                    case 2:
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].SOBSplits);
                        break;


                }
            }
        }
        */

        private void EditSplitsbtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\Data\\";
            // Set filter for file extension and default file extensi on 
            dialog.DefaultExt = ".splg";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                Stream stream = File.Open(dialog.FileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                Game g = (Game)formatter.Deserialize(stream);
                Window1 w = new Window1(g, false);
                w.Show();

            }
        }

        private void ShowMSChkBox_Checked(object sender, RoutedEventArgs e)
        {
            Preferences.ShowMS = true;
        }

        private void ShowMSChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Preferences.ShowMS = false;
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            
        }
        private void set_split_key_Click(object sender, RoutedEventArgs e)
        {
            setHotkeyState = 1;
			set_split_key.Content = "Press any key";

		}

		private void set_start_key_Click(object sender, RoutedEventArgs e)
		{
			setHotkeyState = 2;
			set_start_key.Content = "Press any key";
		}

		private void set_reset_key_Click(object sender, RoutedEventArgs e)
		{
			setHotkeyState = 3;
			set_reset_key.Content = "Press any key";
		}
	}
}

