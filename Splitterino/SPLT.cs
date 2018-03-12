using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino
{

	/// <summary>
	/// Reads and writes configuration files
	/// </summary>
	public static class SPLT
	{
		/// <summary>
		/// Writes a splt config file
		/// </summary>
		/// <param name="path"></param>
		/// <param name="game"></param>
		/// <returns>Boolean (successful, failed) </returns>
		public static bool WriteFile (string path, Game game)
		{
			/*
				TODO
			*/
			return false;
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
			// TODO: second parameter to "" after "console" is changed to a string
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

			List<string> split_buffer = new List<string>();

			// Category object
			Category cat = new Category(g, "");

			// Loop through lines
			foreach(string row in text)
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
							if(c != ' ')
							{
								cur_key += c;
								continue;
							}
							else if(c == '=')
							{
								if(cur_key.ToUpper() == "SPLITS")
								{
									_state = 2;
									continue;
								}
								_state = 1;
								continue;
							}
							break;
						case 1:
							cur_val += c;
							break;
						case 2:
							if (c == ',')
							{
								split_buffer.Add(cur_val);
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
							// TODO: uncomment sitten kun console on string
							//g.SetConsole(cur_val);
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
							split_buffer.Add(cur_val);
						}
						break;
				}

				// empty buffers
				cur_key = "";
				cur_val = "";
				// set default state
				_state = 0;
			}
			if(g.GetName() != "" && g.GetConsole() != "" && g.GetName() != "")
			{
				return g;
			}

			return null;
		}
	}
}
