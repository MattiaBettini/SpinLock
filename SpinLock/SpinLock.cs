using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpinLock
{
    public abstract class SpinLock
    {
        private int TokenCounter;
        private volatile int ServedToken;

        public SpinLock()
        {
            ServedToken = TokenCounter + 1;
        }

        protected void SpinWaitFor(Action action)
        {
            int ThreadToken = Interlocked.Increment(ref TokenCounter);

            while (true)
            {
                if (Interlocked.CompareExchange(ref ThreadToken, ServedToken + 1, ServedToken) != ThreadToken)
                {
                    action.Invoke();
                    ServedToken = ThreadToken;
                    break;
                }
            }
        }
    }
}