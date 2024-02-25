using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Interfaces;

namespace Demonstration
{
    public class Class1
    {
        private Class2 _class2;
        private ITracer _tracer;

        public Class1(ITracer tracer)
        {
            _tracer = tracer;
            _class2 = new Class2(_tracer);
        }

        public void MyMethod(int delay)
        {
            _tracer.StartTrace();

            Thread.Sleep(delay);

            _class2.InnerMethod(delay);

            _tracer.StopTrace();

            _tracer.GetTraceResult();
        }
    }
}
