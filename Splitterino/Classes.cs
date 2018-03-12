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
        private Category category;
        private string name;
        private Console console;
        private List<Splits> splitList = new List<Splits>();
        public List<Splits> SplitList { get => splitList; set => splitList = value; }

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

        /// <summary>
        /// Gets the category
        /// </summary>
        /// <returns></returns>
        public Category GetCategory()
        {
            return category;
        }

        /// <summary>
        /// Set category
        /// </summary>
        /// <param name="value"></param>
        public void SetCategory(Category value)
        {
            category = value;
        }


    }

    /// <summary>
    /// Run category (example Any% or 100%)
    /// </summary>
    public class Category
    {
        public string Name { get; set; }
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
    public class Splits
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

        public string BestTime { get; set; }
        public string TargetTime { get; set; }
        public string Time { get; set; }


    }


}
