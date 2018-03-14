using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Splitterino
{
    [Serializable]
    public static class Hotkeys
    {
        public const uint StartHK = 0x61; //numpad1
        public const uint PauseHK = 0x62; //numpad2
        public const uint SplitHK = 0x60; //numpad0
        public const uint ResetHK = 0x63; //numpad3
        public const uint SkipHK = 0x64; //numpad4


    }
}
