using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeIndexTwitter
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Eをクリックすると止まります");
			while (true)
			{
				if (Console.KeyAvailable)
				{
					string inputKey = Console.ReadKey().Key.ToString();
					if (inputKey == "E")
					{
						return;
					}
				}
				//Console.WriteLine("動き続けている！！");
			}
		}
	}
}
