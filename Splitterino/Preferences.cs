﻿using System;
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
        public static void Deserialize()
        {
            try
            {
                
                Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniPrefs.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                PreferencesSerializable p = new PreferencesSerializable();

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
                    SPLT.ReadAndPrint(Directory.GetCurrentDirectory() + "//Data//Games//" + DefaultGamePath);
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
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].PBSplits);
                        break;
                    case 2:
                        RunManager.WriteTargetTime(SPLT.LoadedGame.CategoryList[0].TargetSplits);
                        break;
                    case 3:
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

            
            
        }
    }
}
