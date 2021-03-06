﻿using System;
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
        public TimeSpan PersonalBest = TimeSpan.Zero;
        // PB splits
        public List<Split> PBSplits = new List<Split>();
        // Target time
        public TimeSpan TargetTime = TimeSpan.Zero;
        // Target splits
        public List<Split> TargetSplits = new List<Split>();
        // Sum of best segments
        public TimeSpan SOBTime = TimeSpan.Zero;
        // Sum of best segments splits
        public List<Split> SOBSplits = new List<Split>();

    }

    /// <summary>
    /// Single split
    /// </summary>
    [Serializable]
    public class Split
    {
        private string title;
        public string mediaFile { get; set; }
        public int splitIndex = 0;
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

        /// <summary>
        /// Constructor for saving the splits (Name not needed!)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="index"></param>
        public Split (TimeSpan time, int index)
        {
            this.Time = time;
            this.splitIndex = index;

        }

        /// <summary>
        /// Compares two split's times. Useful if you want to compare current time to personal best time.
        /// Returns true if a's time is less than b's time otherwise returns false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if a's time is less than b's time otherwise returns false</returns>
        public static bool Compare (Split a, Split b)
        {
            return a.Time < b.Time;
        }
        public TimeSpan Time { get; set; }



    }

    [Serializable]
    public class Run
    {
        public Run()
        {
            TimeStamp = DateTime.Now;
        }
        private uint RunID { get; set; }
        public Game Game { get; set; }
        public DateTime TimeStamp { get; protected set; }
        // Total Run time
        public TimeSpan Time = TimeSpan.Zero;
        // Splits
        public List<Split> Splits = new List<Split>();
    }

    public class CompareTimeClass
    {
        public string TimeString { get; set; }
        public CompareTimeClass(string t)
        {
            TimeString = t;
        }
        public override string ToString()
        {
            return TimeString;
        }
    }
}
