using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleThread
{
    class WatchDog
    {
        Thread t;

        public WatchDog()
        {
            t = new Thread(Watch);
            t.Start();
        }

        public void Stop()
        {
            t.Abort();
        }

        void Watch()
        {
            while (true)
            {
                Console.Write('.');
                Thread.Sleep(1000);
            }
        }
    }
}
