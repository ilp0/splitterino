﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        List<Split> SplitList = new List<Split>();
        List<Split> TargetSplitList = new List<Split>();
        public Window1(Game g, bool isNew)
        {
            InitializeComponent();
            
            if (!isNew)
            {
                SplitList = g.CategoryList[0].SplitList;
                TargetSplitList = g.CategoryList[0].TargetSplits;
                GameName.Text = g.GetName();
                CategoryName.Text = g.CategoryList[0].Name;
                ConsoleName.Text = g.GetConsole();
                for (int i = 0; i < SplitList.Count; i++)
                {
                    SplitContainer.Items.Add(SplitList[i].GetTitle());
                }
            }

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

            c.SplitList = SplitList;
            c.TargetSplits = TargetSplitList;
            g.CategoryList.Add(c);
            SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games", g);
            MessageBox.Show("Saved split file to " + Directory.GetCurrentDirectory() + "\\Data\\Games", "Splitterino" + g.GetName() + ".splg", System.Windows.MessageBoxButton.OK);
            Close();
        }

        private void SaveGame()
        {

        }

        private void AddSplitToList_Click(object sender, RoutedEventArgs e)
        {
            Split s = new Split(TitleInput.Text);
            SplitContainer.Items.Add(s.GetTitle());
            TitleInput.Focus();
            s.splitIndex = SplitContainer.Items.Count;
            //juu eihän tässä
            try
            {
                if (TargetHour.Text != null || TargetMin.Text != null || TargetSec.Text != null)
                {
                    TargetSplitList.Add(new Split(s.GetTitle())
                    {
                        splitIndex = s.splitIndex,
                        Time = new TimeSpan(
                            int.Parse(TargetHour.Text),
                            int.Parse(TargetMin.Text),
                            int.Parse(TargetSec.Text))
                    });
                }
            }
            catch
            {
                Debug.WriteLine("TargetTime setting failed");
            }
            TitleInput.Text = "";
            TargetHour.Text = "0";
            TargetMin.Text = "0";
            TargetSec.Text = "0";
            SplitList.Add(s);

        }

        /// <summary>
        /// Move Items up on the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SplitContainer.SelectedIndex > 0 && SplitContainer.SelectedIndex != SplitContainer.Items.Count)
            {
                TargetSplitList[SplitContainer.SelectedIndex].splitIndex -= 1;
                TargetSplitList[SplitContainer.SelectedIndex - 1].splitIndex += 1;
                SplitList[SplitContainer.SelectedIndex].splitIndex -= 1;
                SplitList[SplitContainer.SelectedIndex - 1].splitIndex += 1;
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
                TargetSplitList[SplitContainer.SelectedIndex].splitIndex += 1;
                TargetSplitList[SplitContainer.SelectedIndex + 1].splitIndex -= 1;
                SplitList[SplitContainer.SelectedIndex].splitIndex += 1;
                SplitList[SplitContainer.SelectedIndex + 1].splitIndex -= 1;
                // add a duplicate item down in the listbox
                int IndexToRemove = SplitContainer.SelectedIndex;
                SplitContainer.Items.Insert(SplitContainer.SelectedIndex + 2, SplitContainer.SelectedItem);
                // make it the current item
                SplitContainer.SelectedIndex = (SplitContainer.SelectedIndex + 2);
                // delete the old occurrence of this item
                SplitContainer.Items.RemoveAt(IndexToRemove);
            }
        }

        private void RemoveSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove split?","Are you sure?", System.Windows.MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SplitList.RemoveAt(SplitContainer.SelectedIndex - 1);
                TargetSplitList.RemoveAt(SplitContainer.SelectedIndex - 1);
                SplitContainer.Items.RemoveAt(SplitContainer.SelectedIndex);
            }

        }

        private void UpdateSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            int sI = SplitContainer.SelectedIndex;
            SplitList[sI].SetTitle(TitleInput.Text);
            TimeSpan s = new TimeSpan(int.Parse(TargetHour.Text), int.Parse(TargetMin.Text), int.Parse(TargetSec.Text));
            TargetSplitList[sI].SetTitle(TitleInput.Text);
            TargetSplitList[sI].Time = s;
            SplitContainer.Items.Insert(sI + 1, TitleInput.Text);
            SplitContainer.Items.RemoveAt(sI);
            SplitContainer.SelectedIndex = sI;
            sI = 0;
        }

        private void SplitContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SplitContainer.SelectedIndex > -1)
            {
                TitleInput.Text = SplitList[SplitContainer.SelectedIndex].GetTitle();
                TargetHour.Text = TargetSplitList[SplitContainer.SelectedIndex].Time.Hours.ToString();
                TargetMin.Text = TargetSplitList[SplitContainer.SelectedIndex].Time.Minutes.ToString();
                TargetSec.Text = TargetSplitList[SplitContainer.SelectedIndex].Time.Seconds.ToString();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
