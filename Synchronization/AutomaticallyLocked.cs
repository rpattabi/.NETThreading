using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace Synchronization
{
    [Synchronization]
    public class AutomaticallyLocked : ContextBoundObject
    {
        public void Demo()
        {
            Console.Write("Start...");
            Thread.Sleep(1000);
            Console.WriteLine("end");
        }
    }

    public class Test
    {
        public static void Main()
        {
            AutomaticallyLocked safeInstance = new AutomaticallyLocked(); // Returns a proxy! 

            // Call Demo three times concurrently
            new Thread(safeInstance.Demo).Start();
            new Thread(safeInstance.Demo).Start();
            safeInstance.Demo();
        }
    }
}

