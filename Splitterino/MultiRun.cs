using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Splitterino.Multi
{
	public class MultiRun
	{
        public static MultiRun instance = null;
        public static Socket client = null;
		public static void Initialize ()
		{
			instance = new MultiRun();
		}

        public static void Connect(IPAddress address, int port)
        { 
            

            // Connect to a remote device.  
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(address, port);

                // Create a TCP/IP  socket.  
                client = new Socket(address.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    client.Connect(remoteEP);
                    Thread tr = new Thread(ReceiveThread);
                    tr.Start();

                }
                catch (ArgumentNullException ane)
                {
                    Debug.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Debug.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private static void ReceiveThread ()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                if(client.Available > 0)
                {
                    int read = client.Receive(bytes);
                    if(read > 0)
                    {
                        Debug.WriteLine(Encoding.ASCII.GetString(bytes, 0, read));
                    }
                    
                }
                Thread.Sleep(100);
            }
        }

        public static void Send (byte command, Dictionary<string, string> message)
        {
            message.Add(new string((char)0x01, 1), new string((char)command, 1));
            client.Send(Encoding.ASCII.GetBytes(MultiMessage.Encode(message)));
        }

        public static void Disconnect ()
        {
            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

    }



	public static class MultiMessage
	{
		/*
		 * Message structure
			#--				| Start tag
			0x01:0x03		| 0x01:Command (BYTE VALUE, not ascii char)
			testi:aabbccc	| key:value (NOTE: if user sends ':', it has to be escaped!
			arvo:1125.3134	| key:value (Parse numbers)
			--#				| End tag
		*/

		/*
		 * Message Commands
			1				| Auth (key, nickname)
			2				| Clean DC
			3				| Chatter (message)
			4				| Timer countdown begin (ping result) (HOST ONLY)
			5				| Split (split index)
			6				| Share splits (names)
			7				| Share splits (times)

		*/
		public static Dictionary<string, string> Decode(string message)
		{
			string[] rows = message.Split('\n');
			if(!rows[0].Contains("#--") || !rows[rows.Length - 1].Contains("--#")) // Not a good message
			{
				return new Dictionary<string, string>();
			}
			Dictionary<string, string> messages = new Dictionary<string, string>();
			string keybuffer = "";
            string valuebuffer = "";
			bool continueNext = false;

			int state = 0; // 0 = read key, 1 = read value
			int rownum = 0;
			foreach(string row in rows)
			{
				if(rownum == 0 || rownum == rows.Length - 1)
				{
					rownum++;
					continue;
				}
				foreach(char c in row)
				{
					if(continueNext)
					{
						continueNext = false;
						continue;
					}
					if(state == 0)
					{
						if(c == ':')
						{
							state = 1;
							continue;
						}
						else if(c == '\\')
						{
							continueNext = true;
							continue;
						}
						else
						{
							keybuffer += c;
						}
					}
					if (state == 1)
					{
						if (c == '\\')
						{
							continueNext = true;
							continue;
						}
						else
						{
							valuebuffer += c;
						}
					}
				}
				messages.Add(keybuffer, valuebuffer);
                keybuffer = "";
                valuebuffer = "";
				rownum++;
			}
			return messages;
		}
		public static string Encode (Dictionary<string, string> kvs)
		{
			string msg = "#--\n";
			
			foreach(KeyValuePair<string, string> kv in kvs)
			{
				string k = kv.Key;
				string v = kv.Value;
				msg += k.Replace(":", "\\:") + ":" + v.Replace(":", "\\:") + "\n";
			}
			msg += "--#";

			return msg;
		}
	}
}
