using System;
using System.Collections.Generic;
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
    }
}
