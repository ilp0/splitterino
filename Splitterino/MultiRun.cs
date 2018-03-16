using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitterino.Multi
{
	public class MultiRun
	{
		public static MultiRun instance = null;
		
		public static void Initialize ()
		{
			instance = new MultiRun();
		}

	}

	public static class MultiMessage
	{
		/*
		 * Message structure
			#--				| Start tag
			0x01			| Command (BYTE VALUE, not ascii char)
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
		public static Dictionary<string, string> Decode (string message)
		{

			return new Dictionary<string, string>();
		}
	}
}
