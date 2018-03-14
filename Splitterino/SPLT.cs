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
		public static void WriteFile (string path, Game game)
		{
            try
            {
                Stream stream = File.Open(path + "\\" + game.GetName() + ".splg", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, game);
                stream.Close();
            } catch
            {
                Debug.WriteLine("Save failed");
            }
            game = null;

		}

		public static void ReadAndPrint (string path)
		{
            try
            {
                Stream stream = File.Open(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                Game g = (Game)formatter.Deserialize(stream);
                stream.Close();
                MainWindow.instance.UpdateGUI(g, g.CategoryList[0]);
                LoadedGame = g;
            }
            catch
            {
                Debug.WriteLine("Fileread failed");
            }
            //Game g = ReadFile(path);
            /*
			if(g != null)
			{
                /*
				MainWindow.instance.GameTitle.Text = ("New Game Added!");
                MainWindow.instance.GameTitle.Text += ("\tName: " + g.GetName() + "\n");
                MainWindow.instance.GameTitle.Text += ("\tCategory: " + g.CategoryList[0].Name);
                MainWindow.instance.GameTitle.Text += ("\tSplits: ");
				foreach(Split s in g.CategoryList[0].SplitList)
				{
                    MainWindow.instance.GameTitle.Text += ("\t\t" + s.GetTitle());
				}
                

                Debug.WriteLine("New Game Added!");
                Debug.WriteLine("\tName: " + g.GetName() + "\n");
                Debug.WriteLine("\tCategory: " + g.CategoryList[0].Name);
                Debug.WriteLine("\tSplits: ");
                foreach (Split s in g.CategoryList[0].SplitList)
                {
                    Debug.WriteLine("\t\t" + s.GetTitle());
                }
                
            }
        */

        }

        /// <summary>
        /// Reads an .splt file and tries to parse it
        /// </summary>
        /// <param name="path"></param>
        /// <returns>New "Game" object if successful or NULL if failed</returns>
        public static Game ReadFile (string path)
		{
            
			/*
				# example file
				game=Sly 3
				console=Playstation 2
				category=Any%
				splits=tutorial,paris,australia

			*/
			// Game object
			Game g = new Game("", "");

			// Read file
			string[] text = System.IO.File.ReadAllLines(path);
			// State Machine
			int _state = 0;
			/*
				0 = read key
				1 = read value
				2 = read splits
			*/

			// string buffers
			string cur_val = "";
			string cur_key = "";

			List<Split> split_buffer = new List<Split>();

			// Category object
			Category cat = new Category(g, "");

			g.CategoryList.Add(cat);
            Debug.WriteLine("LINES" + text.Length);
            // Loop through lines
            foreach (string row in text)
			{
				// Loop through characters
				foreach (char c in row)
				{

					// TODO: if starts with '#'
					// ignore # 
					if (c == '#')
						break;
					switch(_state)
					{
						case 0:
                            
							if(c == '=')
							{
								if(cur_key.ToUpper() == "SPLITS")
								{
									_state = 2;
									continue;
								}
								_state = 1;
                                Debug.WriteLine("CURRENT STATE" + _state);
								continue;
							}
                            else if (c != ' ')
                            {
                                cur_key += c;
                                continue;
                            }
                            break;
						case 1:
							cur_val += c;
							break;
						case 2:
							if (c == ',')
							{
								split_buffer.Add(new Split(cur_val));
								cur_val = "";
								continue;
							}
							cur_val += c;

							break;
					}
				}
				switch(cur_key.ToUpper())
				{
					case "GAME":
						if(cur_val != "")
						{
							g.SetName(cur_val);
						}
						break;
					case "CONSOLE":
						if (cur_val != "")
						{
							g.SetConsole(cur_val);
						}
						break;
					case "CATEGORY":
						if (cur_val != "")
						{
							cat.Name = cur_val;
						}
						break;
					case "SPLITS":
						// add the last split too
						if(cur_val != "")
						{
							split_buffer.Add(new Split(cur_val));
							cat.SplitList = split_buffer;
						}
						break;
				}

				// empty buffers
				cur_key = "";
				cur_val = "";
				// set default state
				_state = 0;
			}
            MainWindow.instance.UpdateGUI(g, cat);
			//if(g.GetName() != "" && g.GetConsole() != "" && cat.Name != "")
			//{
				return g;
			//}

			//return null;
            
		}
	}
}
