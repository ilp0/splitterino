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
    public class Game
    {
        public Game(string n, Console c, Category cat)
        {
            SetName(n);
            SetConsole(c);
            SetCategory(cat);
        }
        private List<Category> categoryList = new List<Category>();
        public List<Category> CategoryList { get => categoryList; set => categoryList = value; }
        private string name;
        private Console console;

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
        public Console GetConsole()
        {
            return console;
        }

        /// <summary>
        /// Sets the game console
        /// </summary>
        /// <param name="value"></param>
        public void SetConsole(Console value)
        {
            console = value;
        }


    }

    /// <summary>
    /// Run category (example Any% or 100%)
    /// </summary>
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
    }
    /// <summary>
    /// Game Console (eg. Nintendo Switch or PS4)
    /// </summary>
    public class Console
    {
        public string Name { get; set; }
    }
    
    /// <summary>
    /// Single split
    /// </summary>
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

        public TimeSpan BestTime { get; set; }
        public TimeSpan TargetTime { get; set; }
        public TimeSpan WRTime { get; set; }
        public TimeSpan Time { get; set; }


    }

    public class Run
    {
        public Run()
        {
            date = DateTime.Now;
        }
        private uint runID { get; set; }
        private Game game { get; set; }
        DateTime date { get; set; }
    }


}
