using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading
{
	class Program
	{
		static void Main(string[] args)
		{
			_1_Basic();
		}

        static void _1_Basic()
		{
			Thread t = new Thread(WriteY);
			t.Start();

			for (int i = 0; i < 1000; i++)
			{
				Console.Write("x");
			}
        }

        static void WriteY()
		{
			for (int i = 0; i < 1000; i++)
			{
				Console.Write("y");
			}
        }
	}
}
