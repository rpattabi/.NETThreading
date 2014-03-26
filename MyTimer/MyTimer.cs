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
		private ManualResetEvent _manualResetEvent;
        private Thread _thread;

        public int Interval_ms { get; set; }

        public MyTimer()
        {
            this.Interval_ms = 1;
			_manualResetEvent = new ManualResetEvent(initialState: false);
        }

        public void Start()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            _thread = new Thread(() =>
            {
                while (_manualResetEvent.WaitOne(this.Interval_ms) == false)
				{
					if (Tick != null)
					{
						// Attempting to execute callback on the calling thread did not work.
						//dispatcher.BeginInvoke((Action)(() => Tick(this, new EventArgs())));

						Tick(this, new EventArgs()); // Warning: Callback happens on secondary thread
					}
				}
            });

            _thread.Start();
        }

        public void Stop()
        {
			_manualResetEvent.Set();
        }

        public event EventHandler Tick;
    }
}
