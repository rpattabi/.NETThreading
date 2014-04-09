using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
	class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine(_1_SimplifyThreadState(Thread.CurrentThread.ThreadState));
            //_5_Lock_Deadlock();
			_6_Mutex_ApplicationSingleton(); // ctrl+f5 twice
		}

        public static ThreadState _1_SimplifyThreadState(ThreadState ts)
		{
            // Technique to convert threadstate to one of the 4 most useful values:
            // Unstarted, Running, WaitSleepJoin, and Stopped
            return ts & (ThreadState.Unstarted | 
				         ThreadState.WaitSleepJoin | 
						 ThreadState.Stopped);
        }

        static readonly object _locker = new object();
        static int _x, _y;
        public static void _3_Lock_Atomicity_1()
		{
            lock (_locker) { if (_x!=0) _y/=_x; }
        }

        static decimal _savingsBalance, _checkBalance;
        public static void _3_Lock_Atomicity_2(decimal amount)
		{
           lock (_locker)
		   {
               _savingsBalance += amount;
               _checkBalance -= amount + GetBankFee(); // atomicity between _savingsBalance and _checkBalance lost if GetBankFee() throws exception. Solution: Impl rollback in catch/finally
           }
        }

        private static decimal GetBankFee()
        {
            return default(decimal);
        }

        public static void _4_Lock_Nested()
		{
            lock (_locker)
            {
                lock (_locker)
				{
                    // do something
                    // ...
                } // Isn't the lock exited here on the same object? 

                // We still have the lock - because locks are 'reentrant'.
            }
        }

        static readonly object _locker1 = new object();
        static readonly object _locker2 = new object();
        public static void _5_Lock_Deadlock()
		{
            new Thread(()=> 
			{
                lock (_locker1)
                {
                    Thread.Sleep(1000);
                    lock(_locker2); // Deadlock
                }
            }).Start();

            lock (_locker2)
            {
                Thread.Sleep(1000);
                lock (_locker1); // Deadlock
            }
        }

        public static void _6_Mutex_ApplicationSingleton()
		{
			using (var mutex = new Mutex(initiallyOwned: false, name: "Hexagon.Mutex_007"))
			{
                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), exitContext: false))
				{
					Console.WriteLine("Another app instance is running. Bye!");
					return;
                }

				RunProgram();
			} 
        }

		private static void RunProgram()
		{
			Console.WriteLine("Running. Press Enter to exit");
			Console.ReadLine();
		}
	}

    class _2_Lock_ThreadUnsafe
	{
		static int _val1 = 1, _val2 = 1;

        static void Go()
		{
           if (_val2 != 0)
			   Console.WriteLine(_val1 / _val2); // May throw NullPointerDivision when one thread executes this and the other modifies _val2 (next line)

		   _val2 = 0;
        }

		static int _x;
		static void Increment() { _x++; }
		static void Assign() { _x = 123; }
    }

    class _2_Lock_ThreadSafe
	{
		static int _val1 = 1, _val2 = 1;
		static readonly object _locker = new object();

        static void Go()
		{
			lock (_locker)
			{
				if (_val2 != 0)
					Console.WriteLine(_val1 / _val2);

				_val2 = 0;
			}

            // Equivalent (Expanded) version
			bool lockTaken = false;
			try
			{
				Monitor.Enter(_locker, ref lockTaken);

				if (_val2 != 0)
					Console.WriteLine(_val1 / _val2);

				_val2 = 0;
			}
			finally
			{
				if (lockTaken)
					Monitor.Exit(_locker);
			}

            //
            // [Unclear]
            //

            // Issues with non-exclusive sync object like below. 
			//     Deadlocking 
			//     Excessive blocking. 
            //     Lock on a type may seep through application domain boundaries (within same process). Good or bad?

			//lock (this) { } // non static
			//lock (typeof(Buffer)) { } // static
        }

		static int _x;
		static void Increment() { lock (_locker) _x++; }
		static void Assign() { lock (_locker) _x = 123; }
    }
}
