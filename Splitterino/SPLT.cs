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
	/// Reads and writes configuration files
	/// </summary>
	public static class SPLT
	{

        public static Game LoadedGame = null;
        /// <summary>
        /// Writes a splt config file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="game"></param>
        /// <returns>Boolean (successful, failed) </returns>
        public static void WriteFile(string path, Game game)
        {
                try
                {
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "//Data//Games//"))
                    {
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "//Data//Games//");
                        Debug.WriteLine("created directory at " + Directory.GetCurrentDirectory() + "//Data//Games//");
                    }
                    Stream stream = File.Open(path + "\\" + game.GetName() + ".splg", FileMode.Create);
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, game);
                    stream.Close();
                }
                catch
                {
                    Debug.WriteLine("save failed");
                }
        }
        /// <summary>
        /// Reads and loads .splg file
        /// </summary>
        /// <param name="file"></param>
		public static void ReadAndPrint(string file)
        {
            
            
                try
                {
                    Stream stream = File.Open(file, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    Game g = (Game)formatter.Deserialize(stream);
                    stream.Close();
                
                LoadedGame = g;

                    MainWindow.instance.UpdateGUI(g, g.CategoryList[0]);

                }
                catch
                {

                    Debug.WriteLine("Fileread failed");
                }
        }
            

        /// <summary>
        /// Converts timespan to string (apu)
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TimeSpanToString(TimeSpan ts)
        {
            string s = "";
            s = "";
            if(ts.Hours != 0)
            {
                s += String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
            } else
            {
                s += String.Format("{0:00}:{1:00}",
                ts.Minutes, ts.Seconds);
            }
            if (Preferences.ShowMS)
            {
                s += "." + (ts.Milliseconds / 10).ToString();
            }
            return s;
            
            
        }
        /// <summary>
        /// Compares ts1 to ts2
        /// </summary>
        /// <param name="ts1"></param>
        /// <param name="ts2"></param>
        /// <returns></returns>
        public static TimeSpan CompareTS(TimeSpan ts1, TimeSpan ts2)
        {
            return ts1 - ts2;
            
        }

        public static TimeSpan CountTotalTime(List<Split> sL)
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (Split s in sL)
            {
                total += s.Time;
            }
            return total;
        }
    }
}
