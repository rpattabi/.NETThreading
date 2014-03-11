﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading
{
    class Program
    {
        /// <summary>
        ///
        /// Threading Examples in .NET
        ///
        /// Courtesy: 
        /// "Threading in C#" by Joseph Albahari
        /// http://www.albahari.com/threading/
        ///
        /// </summary>
        static void Main()
        {
            //_1_ThreadClass();
            //_2_LocalVariable();
            //_3_CommonReference();
            //_4_StaticField();
            //_5_LackOfThreadSafety();
            //_6_Lock();
            //_7_Join();
            //_8_Sleep();
            //_9_Construction();
            //_10_PassArguments();
            //_11_LambdaGotcha();
            //_11a_LambdaGotcha_Workaround();
            _11b_LambdaGotcha();
        }

        #region 1

        static void _1_ThreadClass()
        {
            Thread t = new Thread(WriteY_1);
            t.Start();

            for (int i = 0; i < 1000; i++)
            {
                Console.Write("x");
            }
        }
        static void WriteY_1()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write("y");
            }
        }

        #endregion

        #region 2

        static void _2_LocalVariable()
        {
            new Thread(Go_2).Start();
            Go_2();
        }

        static void Go_2()
        {
            for (int cycles = 0; cycles < 5; cycles++)
            {
                Console.Write('?');
            }
        }

        #endregion

        #region 3

        bool done_3;
        static void _3_CommonReference()
        {
            Program p = new Program();
            new Thread(p.Go_3).Start();
            p.Go_3();
        }
        void Go_3()
        {
            if (!done_3) { done_3 = true; Console.WriteLine("Done"); }
        }

        #endregion

        #region 4

        static bool done_4;

        static void _4_StaticField()
        {
            new Thread(Go_4).Start();
            Go_4();
        }

        static void Go_4()
        {
            if (!done_4) { done_4 = true; Console.WriteLine("Done"); }
        }

        #endregion

        #region 5

        static bool done_5;

        static void _5_LackOfThreadSafety()
        {
            new Thread(Go_5).Start();
            Go_5();
        }

        static void Go_5()
        {
            if (!done_5) { Console.WriteLine("Done"); done_5 = true; }
        }

        #endregion

        #region 6

        static bool done_6;
        static readonly object locker = new object();

        static void _6_Lock()
        {
            new Thread(Go_6).Start();
            Go_6();
        }

        static void Go_6()
        {
            lock (locker)
            {
                if (!done_6) { Console.WriteLine("Done"); done_6 = true; }
            }
        }

        #endregion

        #region 7

        static void _7_Join()
        {
            Thread t = new Thread(Go_7);
            t.Start();
            t.Join(millisecondsTimeout: 500); // Timeout is optional.

            Console.WriteLine("Thread t has ended!");
        }

        static void Go_7()
        {
            for (int i = 0; i < 1000; i++)
                Console.Write("y");
        }

        #endregion

        #region 8

        static void _8_Sleep()
        {
            Thread.Sleep(millisecondsTimeout: 500);
            Thread.Sleep(TimeSpan.FromMilliseconds(500)); // Alternative

            // Sleep(0) and Yield
            //
            // 1. Useful for advanced performance tweaks.
            // 2. Diagnostic tool to uncover thread safety issues.
            //
            // "If inserting Thread.Yield() makes or breaks program, you almost certainly have a bug."

            Thread.Sleep(0); // Relinquishes thread's current time slice immediately. Voluntarily give back CPU to other threads.
            Thread.Yield();  // Relinquishes only to threads running on the _same_ processor.
        }

        #endregion

        #region 9

        static void _9_Construction()
        {
            Thread t = new Thread(Go_9);
            Thread t2 = new Thread(new ThreadStart(Go_9));
            Thread t3 = new Thread(_ => Console.WriteLine("Hello!"));

            t.Start(); t2.Start(); t3.Start();
            Go_9();
        }

        static void Go_9()
        {
            Console.WriteLine("hello!");
        }

        #endregion

        #region 10

        static void _10_PassArguments()
        {
            Thread t = new Thread(Print_10);
            t.Start("Hello from t!");

            Thread t2 = new Thread(() => Print_10("Hello from t2!"));
            t2.Start();
        }

        static void Print_10(object messageObj)
        {
            string message = (string)messageObj;
            Console.WriteLine(message);
        }

        #endregion

        #region 11

        static void _11_LambdaGotcha()
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(() => Console.Write(i))
                            .Start();
            }
        }

        static void _11a_LambdaGotcha_Workaround()
        {
            for (int i = 0; i < 10; i++)
            {
                int temp = i;
                new Thread(() => Console.Write(temp))
                            .Start();
            }
        }

        static void _11b_LambdaGotcha()
        {
            string text = "t1";
            Thread t1 = new Thread(_ => Console.WriteLine(text));

            text = "t2";
            Thread t2 = new Thread(_ => Console.WriteLine(text));

            t1.Start(); t2.Start();
        }

        #endregion
    }
}
