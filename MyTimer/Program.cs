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
        static void Main(string[] args)
        {
            var timer_5ms = new MyTimer() { Interval_ms = 5 };
            timer_5ms.Tick += timer_5ms_Tick;
            timer_5ms.Start();

            var stopwatch = Stopwatch.StartNew();
            while (true)
                if (stopwatch.ElapsedMilliseconds >= 10000)
                    break;
                else
                    Thread.Yield();

            stopwatch.Stop();
        }

        static void timer_5ms_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("5ms tick");
        }
    }
}
