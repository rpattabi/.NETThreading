using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTimer
{
    class Program
    {
		private const int Interval_ms = 500;

        static void Main(string[] args)
        {
            var timer = new MyTimer() { Interval_ms = Interval_ms };
            timer.Tick += timer_Tick;
            timer.Start();

			var manualResetEvent = new ManualResetEvent(initialState: false);
            while (manualResetEvent.WaitOne(Interval_ms) == false)
                Thread.Yield();
        }

        static void timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm::ss.ffffzzz") + " " + Interval_ms + "ms tick");
        }
    }
}
