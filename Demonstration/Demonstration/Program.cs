using Tracer;

namespace Demonstration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ResultOutput resultOutput = new ResultOutput();
            Tracer.Tracer tracer = new Tracer.Tracer(resultOutput);

            Class1 foo = new Class1(tracer);

            Thread[] threads = new Thread[5];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => foo.MyMethod(1000)); 
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            resultOutput.ConsoleOutput(tracer.GetTraceResult().SerializeToJSON());

            resultOutput.FileOutput(tracer.GetTraceResult().SerializeToJSON(), "test.txt"); 
           
            //test
        }

    }
}
