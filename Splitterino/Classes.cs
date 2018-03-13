using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino
{
    /// <summary>
    /// Game
    /// </summary>
    [Serializable]
    public class Game
    {
        public Game(string n, string c)
        {
            SetName(n);
            SetConsole(c);
            
        }
		public Game ()
		{
			// important merge, delete this comment
		}
        private List<Category> categoryList = new List<Category>();
        public List<Category> CategoryList { get => categoryList; set => categoryList = value; }
        private string name;
        private string console;

        //NAME
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }
  
        /// <summary>
        /// Gets the game console
        /// </summary>
        /// <returns></returns>
        public string GetConsole()
        {
            return console;
        }

        /// <summary>
        /// Sets the game console
        /// </summary>
        /// <param name="value"></param>
        public void SetConsole(string value)
        {
            console = value;
        }


    }

    /// <summary>
    /// Run category (example Any% or 100%)
    /// </summary>
    [Serializable]
    public class Category
    {
        public Category(Game g, string n)
        {
            game = g;
            Name = n;
        }
        public Game game;
        public string Name { get; set; }
        private List<Split> splitList = new List<Split>();
        public List<Split> SplitList { get => splitList; set => splitList = value; }
        //Total run pb
        public TimeSpan PersonalBest { get; set; }
    }
    
    /// <summary>
    /// Single split
    /// </summary>
    [Serializable]
    public class Split
    {
        private string title;

        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string value)
        {
            title = value;
        }

		public Split (string name)
		{
			title = name;
		}
        //Best time all time
        public TimeSpan BestTime { get; set; }
        //Split time from PB
        public TimeSpan PersonalBestTime { get; set; }
        //optional target time
        public TimeSpan TargetTime { get; set; }
        //current run time
        public TimeSpan Time { get; set; }

    }

    public class Run
    {
        public Run()
        {
            TimeStamp = DateTime.Now;
        }
        private uint runID { get; set; }
        public Game game { get; set; }
        public DateTime TimeStamp { get; protected set; }
    }


}
