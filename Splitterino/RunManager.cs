﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Splitterino
{
    public class RunManager
    {

        public static List<Split> CurrentRunSplits = new List<Splitterino.Split>();
        public static int CurrentSplitIndex = 0;
        public static TimeSpan lastTime = TimeSpan.Zero;
        public static void TimerStart ()
        {
            if (!MainWindow.instance.runInProgress)
            {
                MainWindow.instance.CurrentRunCmprListbox.Items.Add(new CompareTimeClass(""));
                /* Run curRun = new Run()
                {
                    game = MainWindow.instance.g
                };
                */
                MainWindow.instance.sw.Start();
                MainWindow.instance.dt.Start();
                lastTime = MainWindow.instance.sw.Elapsed;
                MainWindow.instance.Startbtn.IsEnabled = false;
                //SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games\\", SPLT.LoadedGame);
                MainWindow.instance.runInProgress = true;
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
            if (!MainWindow.instance.sw.IsRunning)
                return;
            //add current time to right list of splits
            MainWindow.instance.elapsedtimeitem.Items.Add(MainWindow.instance.currentTime);
            //save to var
            CurrentRunSplits.Add(new Split(MainWindow.instance.sw.Elapsed - lastTime, CurrentSplitIndex));
            if(SPLT.LoadedGame != null)
            {   
                //Jos Sum of Best splittejä on vähemmän kuin nykyinen splitti indexi
                if(SPLT.LoadedGame.CategoryList[0].SOBSplits.Count > CurrentSplitIndex)
                {
                    //jos vanha SOB splittiaika on suurempi kuin nykyisen splitin aika niin tallenna splitti listaan.
                    if(SPLT.LoadedGame.CategoryList[0].SOBSplits[CurrentSplitIndex].Time > MainWindow.instance.sw.Elapsed - lastTime)
                    {
                        SPLT.LoadedGame.CategoryList[0].SOBSplits[CurrentSplitIndex].Time = MainWindow.instance.sw.Elapsed - lastTime;
                        Debug.WriteLine("Best segment! very nice job dude!");
                    }
                }
                else
                {
                    SPLT.LoadedGame.CategoryList[0].SOBSplits.Add(new Split(MainWindow.instance.sw.Elapsed - lastTime, CurrentSplitIndex));
                    Debug.WriteLine("Best segment! very nice job dude!");
                }
            }
            MainWindow.instance.ScrollSplitViewToBottom();
            MainWindow.instance.splitCountBuffer++;
            if (MainWindow.instance.splitCountBuffer >= MainWindow.instance.Splititemlist.Items.Count)
            {
                if(SPLT.LoadedGame != null)
                {
                    bool save = false;
                    TimeSpan totes = TimeSpan.Zero;
                    foreach(Split t in SPLT.LoadedGame.CategoryList[0].SOBSplits)
                    {
                        totes += t.Time;
                    }
                    if (totes != TimeSpan.Zero)
                    {
                        if (totes != SPLT.LoadedGame.CategoryList[0].SOBTime)
                        {
                            SPLT.LoadedGame.CategoryList[0].SOBTime = totes;
                            save = true;
                        }
                    }
                    if (SPLT.LoadedGame.CategoryList[0].PersonalBest > MainWindow.instance.sw.Elapsed || SPLT.LoadedGame.CategoryList[0].PersonalBest == TimeSpan.Zero)
                    {
                        Debug.WriteLine("PB !!!!!!!!!\n\n");
                        SPLT.LoadedGame.CategoryList[0].PersonalBest = MainWindow.instance.sw.Elapsed;
                        SPLT.LoadedGame.CategoryList[0].PBSplits = CurrentRunSplits;
                        
                        save = true;
                    }

                    if(save)
                    {
                        SPLT.WriteFile(Directory.GetCurrentDirectory() + "\\Data\\Games\\", SPLT.LoadedGame);
                    }
                }
                MainWindow.instance.sw.Stop();
                MainWindow.instance.Splitbtn.IsEnabled = false;
                MainWindow.instance.Stopbtn.IsEnabled = false;
                MainWindow.instance.splitCountBuffer = 0;
                if (SPLT.LoadedGame != null)
                {
                    MainWindow.instance.UpdateGUI(SPLT.LoadedGame, SPLT.LoadedGame.CategoryList[0]);
                }
            }
            lastTime = MainWindow.instance.sw.Elapsed;
            /*for (int i = 0; i < CurrentSplitIndex; i++)
            {
                compareTime += SPLT.LoadedGame.CategoryList[0].SOBSplits[i].Time;
            }
            */
            //toivottavasti toimii!
            if(SPLT.LoadedGame.CategoryList[0].PBSplits.Count > CurrentSplitIndex)
            {
                TimeSpan cmpr = SPLT.CountTotalTime(new List<Split>(SPLT.LoadedGame.CategoryList[0].PBSplits.GetRange(0, CurrentSplitIndex + 1))) - SPLT.CountTotalTime(CurrentRunSplits);
                string cmprTime = "";
                if (cmpr.Ticks < 0)
                {
                    cmpr.Negate();
                    cmprTime = "+";
                }
                else
                {
                    cmprTime = "-";
                }
                cmprTime += SPLT.TimeSpanToString(cmpr, true);
                //MainWindow.instance.CurrentRunCmprListbox.Items.Add(cmprTime);
                //if(SPLT.LoadedGame.CategoryList[0].PBSplits[CurrentSplitIndex].Time < SPLT.LoadedGame.CategoryList[0].PBSplits[CurrentSplitIndex].Time)
                //{
                    
                //}
                cmpr = TimeSpan.Zero;
                cmprTime = "";
            }

            MainWindow.instance.CurrentRunCmprListbox.Items.Add(new CompareTimeClass(""));
            CurrentSplitIndex++;


        }

        public static void Reset ()
        {
            MainWindow.instance.sw.Reset();
            MainWindow.instance.MainTimerDisplay.Text = "00:00:00";
            MainWindow.instance.Startbtn.IsEnabled = true;
            MainWindow.instance.Splitbtn.IsEnabled = true;
            MainWindow.instance.Stopbtn.IsEnabled = true;
            MainWindow.instance.splitCountBuffer = 0;
            lastTime = MainWindow.instance.sw.Elapsed;
            CurrentSplitIndex = 0;
            CurrentRunSplits = new List<Split>();
            ClearUI();
            MainWindow.instance.UpdateGUI(SPLT.LoadedGame, SPLT.LoadedGame.CategoryList[0]);
            MainWindow.instance.runInProgress = false;
        }
        /// <summary>
        /// Writes target time to UI. Gets the targetsplits from passed on property
        /// </summary>
        /// <param name="sl"></param>
        public static void WriteTargetTime(List<Split> sl)
        {
            /*if(SPLT.LoadedGame.CategoryList[0].TargetSplits == null)
            {
                SPLT.LoadedGame.CategoryList[0].TargetSplits = SPLT.LoadedGame.CategoryList[0].PBSplits;
            }
            */
            //MainWindow.instance.TargetTimeContainer.Items.Clear();
            if(SPLT.LoadedGame != null)
            {
                TimeSpan tts = new TimeSpan();
                for (int i = 0; i < sl.Count; i++)
                {
                    tts += sl[i].Time;
                    string a = SPLT.TimeSpanToString(tts, false);
                    //string a = SPLT.TimeSpanToString(SPLT.LoadedGame.CategoryList[0].TargetSplits[i].Time);
                    //string a = String.Format("{0:00}:{1:00}.{2:00}",
                    //SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Minutes, SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Seconds, SPLT.LoadedGame.CategoryList[0].PBSplits[i].Time.Milliseconds / 10);
                    //MainWindow.instance.TargetTimeContainer.Items.Add(a);
                }

            } else
            {
                Debug.WriteLine("Loaded game is null");
                
            }
            
        }

        public static void ClearUI()
        {
            //MainWindow.instance.TargetTimeContainer.Items.Clear();
            MainWindow.instance.elapsedtimeitem.Items.Clear();
            MainWindow.instance.Splititemlist.Items.Clear();
            MainWindow.instance.CurrentRunCmprListbox.Items.Clear();

        }
    }
}
