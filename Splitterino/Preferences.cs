using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino
{
    [Serializable]
    public static class Preferences
    {
        public static bool WindowAlwaysOnTop { get; set; }
        public static Game DefaultGame { get; set; }
        public static List<Split> DefaultComparisonSplits { get; set; }
        public static TimeSpan DefaultTargetTime { get; set; }
        public static void SetDefaultTargetTime(TimeSpan ts)
        {
            DefaultTargetTime = ts;
        }
        public static void SetDefaultComparisonSplits (List<Split> sL)
        {
            DefaultComparisonSplits = sL;
        }

        public static void LoadPreferences()
        {
            if (WindowAlwaysOnTop)
            {
                MainWindow.instance.Topmost = true;
            } else
            {
                MainWindow.instance.Topmost = false;
            }

            try
            {
                if (DefaultGame != null)
                {
                    SPLT.ReadAndPrint(Directory.GetCurrentDirectory() + "//Data//Games//" + DefaultGame.GetName());
                }
            } catch
            {
                Debug.WriteLine("failed to set defaultgame");
            }
            try
            {
                if (DefaultComparisonSplits != null) {
                    for (int i = 0; i < DefaultComparisonSplits.Count; i++)
                    {
                        MainWindow.instance.TargetTimeContainer.Items.Add(SPLT.TimeSpanToString(DefaultComparisonSplits[i].Time));
                    }
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
