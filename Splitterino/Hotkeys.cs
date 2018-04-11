using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace Splitterino
{
    [Serializable]
    public static class Hotkeys
    {
        public static uint StartHK = 0x61; //numpad1
        public static uint PauseHK = 0x62; //numpad2
        public static uint SplitHK = 0x60; //numpad0
        public static uint ResetHK = 0x63; //numpad3
        public static uint SkipHK = 0x64; //numpad4

        public static Key IsKeyDown()
        {
            var values = Enum.GetValues(typeof(Key));
            foreach (var v in values)
            {
                if (((Key)v) != Key.None)
                {
                    if (Keyboard.IsKeyDown((Key)v))
                    {
                        return (Key)v;
                    }
                }
            }

            return Key.None;
        }

    }
}
