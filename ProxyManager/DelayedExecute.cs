using System;
using Timer = System.Windows.Forms.Timer;

namespace ProxyManager
{
    /// <summary>Delays the execution of a method by a given amount of time.</summary>
    static class DelayedExecute
    {
        /// <summary>
        /// Enqueues the specified method whose execution will be delayed by the specified amount of time.
        /// </summary>
        /// <param name="msToDelay">The number of milliseconds to delay execution.</param>
        /// <param name="method">The method to be invoked after the delay.</param>
        static void Enqueue(int msToDelay, Action method)
        {
            // create the timer which will invoke the method 
            var tmr = new Timer() { Interval = msToDelay };

            // add timer's event handler
            tmr.Tick += (o, e) => {
                var t = (Timer)o;
                t.Stop();
                method.Invoke();
                t.Dispose();
            };

            // start the timer
            tmr.Start();
        }
    }
}
