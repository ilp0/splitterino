using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino
{
    /// <summary>
    /// Class to serialize
    /// </summary>
    [Serializable]
    public class PreferencesSerializable
    {
        public bool WindowAlwaysOnTop { get; set; }
        public bool SavePrefsOnQuit { get; set; }
        public string DefaultGamePath { get; set; }
        public int DefaultComparisonSplits { get; set; }
        public TimeSpan DefaultTargetTime { get; set; }
    }
    /// <summary>
    /// static values for easier use
    /// </summary>
    public static class Preferences
    {
        public static bool WindowAlwaysOnTop { get; set; }
        public static bool SavePrefsOnQuit { get; set; }
        public static string DefaultGamePath { get; set; }
        public static int DefaultComparisonSplits { get; set; }
        public static TimeSpan DefaultTargetTime { get; set; }
        public static void SetDefaultTargetTime(TimeSpan ts)
        {
            DefaultTargetTime = ts;
        }
        /// <summary>
        /// Serialize into SplitteroniPrefs.dat
        /// </summary>
        public static void Serialize()
        {
            try
            {
                PreferencesSerializable p = new PreferencesSerializable();
                p.WindowAlwaysOnTop = WindowAlwaysOnTop;
                p.SavePrefsOnQuit = SavePrefsOnQuit;
                p.DefaultGamePath = DefaultGamePath;
                p.DefaultComparisonSplits = DefaultComparisonSplits;
                p.DefaultTargetTime = DefaultTargetTime;

                Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniPrefs.dat", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, p);
                stream.Close();
            } catch
            {
                Debug.WriteLine("Failed to serialize");
            }

        }
        /// <summary>
        /// Deserialize SplitteroniPrefs.dat
        /// </summary>
        public static void Deserialize()
        {
            try
            {
                
                Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniPrefs.dat", FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();
                PreferencesSerializable p = (PreferencesSerializable)formatter.Deserialize(stream);
                WindowAlwaysOnTop = p.WindowAlwaysOnTop;
                SavePrefsOnQuit = p.SavePrefsOnQuit;
                DefaultGamePath = p.DefaultGamePath;
                DefaultComparisonSplits = p.DefaultComparisonSplits;
                DefaultTargetTime = p.DefaultTargetTime;
            } catch
            {
                Debug.WriteLine("Failed to deserialize");
            }


        }
        /// <summary>
        /// Loads preferences and updates the preferences tab with correct info.
        /// </summary>
        public static void LoadPreferences()
        {


            if (WindowAlwaysOnTop)
            {
                MainWindow.instance.Topmost = true;
                MainWindow.instance.WindowAlwaysOnTopCheckBox.IsChecked = true;
            } else
            {
                MainWindow.instance.Topmost = false;
                MainWindow.instance.WindowAlwaysOnTopCheckBox.IsChecked = false;

            }

            try
            {
                if (DefaultGamePath != null)
                {
                    SPLT.ReadAndPrint(DefaultGamePath);
                }
            } catch
            {
                Debug.WriteLine("failed to set defaultgame");
            }
            try
            {
                switch (DefaultComparisonSplits)
                {
                    case 1:
                        MainWindow.instance.ComparisonTimeComboBox.SelectedIndex = 1;
                        MainWindow.instance.CurrentComparisonTime.SelectedIndex = 0;
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].PBSplits);
                        break;
                    case 2:
                        MainWindow.instance.ComparisonTimeComboBox.SelectedIndex = 2;
                        MainWindow.instance.CurrentComparisonTime.SelectedIndex = 1;
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].TargetSplits);
                        break;
                    case 3:
                        MainWindow.instance.ComparisonTimeComboBox.SelectedIndex = 3;
                        MainWindow.instance.CurrentComparisonTime.SelectedIndex = 2;
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].SOBSplits);
                        break;
                }
            } catch
            {
                Debug.WriteLine("Failed to set defaultComparisonSplits");
            }
            try
            {
                MainWindow.instance.TargetTimeText.Text = SPLT.TimeSpanToString(DefaultTargetTime);
            } catch
            {
                Debug.WriteLine("Failed to set defaultTargetTime");
            }

            if (SavePrefsOnQuit) MainWindow.instance.SavePreferencesOnQuitChkBox.IsChecked = true;
            else MainWindow.instance.SavePreferencesOnQuitChkBox.IsChecked = false;
            
            
        }
    }
}
