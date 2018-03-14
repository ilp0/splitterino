using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Splitterino
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Save button click function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Game g = new Game(GameName.Text, ConsoleName.Text);
            Category c = new Category(g, CategoryName.Text);
            for (int i = 0; i < SplitContainer.Items.Count; i++)
            {
                Split s = new Split(SplitContainer.Items.GetItemAt(i).ToString());
                s.splitIndex = i;
                c.SplitList.Add(s);
            }
            g.CategoryList.Add(c);
            SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games\\", g);
            
        }

        private void AddSplitToList_Click(object sender, RoutedEventArgs e)
        {
            SplitContainer.Items.Add(TitleInput.Text);
            TitleInput.Text = "";
        }

        /// <summary>
        /// Move Items up on the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SplitContainer.SelectedIndex > 0)
            {
                // add a duplicate item up in the listbox
                SplitContainer.Items.Insert(SplitContainer.SelectedIndex - 1, SplitContainer.SelectedItem);
                // make it the current item
                SplitContainer.SelectedIndex = (SplitContainer.SelectedIndex - 2);
                // delete the old occurrence of this item
                SplitContainer.Items.RemoveAt(SplitContainer.SelectedIndex + 2);
            }
        }
        /// <summary>
        /// move items down on the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityDownBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((SplitContainer.SelectedIndex != -1) && (SplitContainer.SelectedIndex < SplitContainer.Items.Count - 1))
            {
                // add a duplicate item down in the listbox
                int IndexToRemove = SplitContainer.SelectedIndex;
                SplitContainer.Items.Insert(SplitContainer.SelectedIndex + 2, SplitContainer.SelectedItem);
                // make it the current item
                SplitContainer.SelectedIndex = (SplitContainer.SelectedIndex + 2);
                // delete the old occurrence of this item
                SplitContainer.Items.RemoveAt(IndexToRemove);
            }
        }
    }
}
