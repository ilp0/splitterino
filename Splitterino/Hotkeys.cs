using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace Splitterino
{
	[System.Serializable]
	public class KeyConfig
	{
		public uint SplitHK, StartHK, ResetHK;

		[NonSerialized]
		static KeyConfig _config = null;
		public static KeyConfig Config {
			get
			{
				if(_config == null)
				{
					try
					{
						if (File.Exists(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat"))
						{
							Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat", FileMode.Open);

							BinaryFormatter formatter = new BinaryFormatter();
							KeyConfig p = (KeyConfig)formatter.Deserialize(stream);
							_config = p;
							stream.Close();
							return p;
						}
						else
						{
							KeyConfig p = new KeyConfig() {
								SplitHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad0),
								StartHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad1),
								ResetHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad2)
							};
							Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat", FileMode.Create);
							BinaryFormatter formatter = new BinaryFormatter();
							formatter.Serialize(stream, p);
							stream.Close();
							_config = p;
							return p;
						}
					}
					catch
					{
						KeyConfig p = new KeyConfig()
						{
							SplitHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad0),
							StartHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad1),
							ResetHK = (uint)KeyInterop.VirtualKeyFromKey(Key.NumPad2)
						};
						Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat", FileMode.Create);
						BinaryFormatter formatter = new BinaryFormatter();
						formatter.Serialize(stream, p);
						stream.Close();
						_config = p;
						return p;
					}
					// Load
					
					
				}
				else
				{
					return _config;
				}
			}

		}

		
		public void Update ()
		{
			Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat", FileMode.Open);

			BinaryFormatter formatter = new BinaryFormatter();
			KeyConfig p = (KeyConfig)formatter.Deserialize(stream);
			this.ResetHK = p.ResetHK;
			this.StartHK = p.StartHK;
			this.SplitHK = p.SplitHK;
			stream.Close();
		}

		public void Save ()
		{
			Stream stream = File.Open(Directory.GetCurrentDirectory() + "\\SplitteroniHotkeys.dat", FileMode.Create);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, this);
			stream.Close();
		}

	}

    public static class Hotkeys
    {
		public static uint SplitHK = 0x60; //numpad0
		public static uint StartHK = 0x61; //numpad1
        // public static uint PauseHK = 0x62; //numpad2
        public static uint ResetHK = 0x63; //numpad3
        //public static uint SkipHK = 0x64; //numpad4

		public const int SplitID = 9000;
		public const int StartID = 9001;
		// public const int PauseID = 9002;
		public const int ResetID = 9003;
		//public const int SkipID = 9004;

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
