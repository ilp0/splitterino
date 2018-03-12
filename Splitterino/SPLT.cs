using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino.SPLT
{


	public static class SPLT
	{
		/// <summary>
		/// Reads an .splt file and tries to parse it
		/// </summary>
		/// <param name="path"></param>
		/// <returns>New "Game" object if successful or NULL if failed</returns>
		public static Game ReadFile (string path)
		{
			Game g = new Game();

			string text = System.IO.File.ReadAllText(path);

			foreach(char c in text)
			{

			}

			return null;
		}
	}
}
