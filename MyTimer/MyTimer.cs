using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MyTimer
{
    class MyTimer
    {
        private Stopwatch _stopwatch;
        private Thread _thread;

        public int Interval_ms { get; set; }

        public MyTimer()
        {
            this.Interval_ms = 1;
        }

        public void Start()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            _thread = new Thread(() =>
            {
                _stopwatch = Stopwatch.StartNew();

                try
                {
                    while (_stopwatch.IsRunning)
                    {
                        if (_stopwatch.ElapsedMilliseconds == 0)
                            continue;

                        if (_stopwatch.ElapsedMilliseconds >= this.Interval_ms)
                            break;

                        if (this.Interval_ms % _stopwatch.ElapsedMilliseconds == 0)
                            if (Tick != null)
                                dispatcher.BeginInvoke((Action) (()=>Tick(this, new EventArgs())));
                    }
                }
                finally
                {
                    _stopwatch.Stop();
                }
            });

            _thread.Start();
        }

        public void Stop()
        {
            _thread.Abort();
        }

        public event EventHandler Tick;
    }
}
