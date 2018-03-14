﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino
{
    public class RunManager
    {

        public static List<Split> CurrentRunSplits = new List<Splitterino.Split>();
        public static int CurrentSplitIndex = 0;
        public static void TimerStart ()
        {
            if (!MainWindow.instance.runInProgress)
            {
                Run curRun = new Run();
                curRun.game = MainWindow.instance.g;
                MainWindow.instance.sw.Start();
                MainWindow.instance.dt.Start();
                MainWindow.instance.Startbtn.IsEnabled = false;
            }
        }

        public static void StopButtonClick ()
        {
            if (MainWindow.instance.sw.IsRunning)
            {
                MainWindow.instance.sw.Stop();
            }
            else
            {
                MainWindow.instance.sw.Start();
            }
        }

        public static void Split ()
        {
            MainWindow.instance.elapsedtimeitem.Items.Add(MainWindow.instance.currentTime);
            CurrentRunSplits.Add(new Splitterino.Split(MainWindow.instance.sw.Elapsed, CurrentSplitIndex++));

            MainWindow.instance.ScrollSplitViewToBottom();
            MainWindow.instance.splitCountBuffer++;
            if (MainWindow.instance.splitCountBuffer >= MainWindow.instance.Splititemlist.Items.Count)
            {
                if(SPLT.LoadedGame != null)
                {
                    if (SPLT.LoadedGame.CategoryList[0].PersonalBest > MainWindow.instance.sw.Elapsed || SPLT.LoadedGame.CategoryList[0].PersonalBest == TimeSpan.Zero)
                    {
                        Debug.WriteLine("PB !!!!!!!!!\n\n");
                        SPLT.LoadedGame.CategoryList[0].PersonalBest = MainWindow.instance.sw.Elapsed;
                        SPLT.LoadedGame.CategoryList[0].PBSplits = CurrentRunSplits;
                        SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games\\", SPLT.LoadedGame);
                    }
                }
                MainWindow.instance.sw.Stop();
                MainWindow.instance.Splitbtn.IsEnabled = false;
                MainWindow.instance.Stopbtn.IsEnabled = false;
                MainWindow.instance.splitCountBuffer = 0;
            }
        }

        public static void Reset ()
        {
            MainWindow.instance.sw.Reset();
            MainWindow.instance.MainTimerDisplay.Text = "00:00:00";
            MainWindow.instance.Startbtn.IsEnabled = true;
            MainWindow.instance.Splitbtn.IsEnabled = true;
            MainWindow.instance.Stopbtn.IsEnabled = true;
            CurrentSplitIndex = 0;
            CurrentRunSplits = new List<Splitterino.Split>();
            for (int i = 0; i <= MainWindow.instance.elapsedtimeitem.Items.Count; i++)
            {
                MainWindow.instance.elapsedtimeitem.Items.Clear();
            }
            MainWindow.instance.ScrollSplitViewToTop();
            WriteTargetTime();
        }

        public static void WriteTargetTime()
        {
            MainWindow.instance.TargetTimeContainer.Items.Clear();
            if(SPLT.LoadedGame != null)
            {
                for (int i = 0; i < SPLT.LoadedGame.CategoryList[0].PBSplits.Count; i++)
                {
                    string a = String.Format("{0:00}:{1:00}.{2:00}",
                    SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Minutes, SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Seconds, SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Milliseconds / 10);
                    MainWindow.instance.TargetTimeContainer.Items.Add(a);
                }

            } else
            {
                Debug.WriteLine("Loaded game is null");
            }
            
        }

        public static void ClearUI()
        {
            MainWindow.instance.TargetTimeContainer.Items.Clear();
            MainWindow.instance.elapsedtimeitem.Items.Clear();
            MainWindow.instance.Splititemlist.Items.Clear();

        }
    }
}
