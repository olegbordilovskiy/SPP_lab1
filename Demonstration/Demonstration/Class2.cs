using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Interfaces;

namespace Demonstration
{
    internal class Class2
    {
        private ITracer _tracer;

        internal Class2(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod(int delay)
        {
            _tracer.StartTrace();

            Thread.Sleep(delay); //

            _tracer.StopTrace();
        }
    }
}
