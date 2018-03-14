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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Game g = new Game(GameName.Text, ConsoleName.Text);
            Category c = new Category(g, CategoryName.Text);
            for (int i = 0; i < SplitContainer.Items.Count; i++)
            {
                c.SplitList.Add(new Split(SplitContainer.Items.GetItemAt(i).ToString()));
            }
            g.CategoryList.Add(c);
            SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games\\", g);
            
        }

        private void AddSplitToList_Click(object sender, RoutedEventArgs e)
        {
            SplitContainer.Items.Add(TitleInput.Text);
        }
    }
}
