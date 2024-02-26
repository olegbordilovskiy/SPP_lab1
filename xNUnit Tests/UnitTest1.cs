
namespace xNUnit_Tests
{
    public class TracerTests
    {
        [Fact]
        public void Tracer_StartStopTrace_CorrectTiming()
        {
            const int DELAY_TIME = 1000;
            const int INACCURACY = 100;

            ResultOutput resultOutput = new ResultOutput();
            Tracer.Interfaces.ITracer tracer = new Tracer.Tracer(resultOutput);

            tracer.StartTrace();
            Thread.Sleep(DELAY_TIME);
            tracer.StopTrace();

            TraceResult traceResult = tracer.GetTraceResult();
            int totalTime = tracer.GetTraceResult().Threads.Select(str => int.Parse(str.Time.TrimEnd("ms".ToCharArray())))
            .Sum();

            Assert.InRange(totalTime, DELAY_TIME, DELAY_TIME + INACCURACY);
        }

        [Fact]
        public void Tracer_MultipleThreads_CorrectTiming()
        {
            const int DELAY_TIME = 1000;
            const int INACCURACY = 500;
            const int THREADS_AMOUNT = 5;

            ResultOutput resultOutput = new ResultOutput();
            Tracer.Interfaces.ITracer tracer = new Tracer.Tracer(resultOutput);
            Class1 foo = new Class1(tracer);

            Thread[] threads = new Thread[THREADS_AMOUNT];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    foo.MyMethod(DELAY_TIME);

                });
                threads[i].Start();

            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            int totalTime = tracer.GetTraceResult().Threads.Select(str => int.Parse(str.Time.TrimEnd("ms".ToCharArray())))
           .Sum();

            Assert.InRange(totalTime, DELAY_TIME * THREADS_AMOUNT * 2, DELAY_TIME * THREADS_AMOUNT * 2 + (INACCURACY * THREADS_AMOUNT * 4));
        }

        [Fact]
        public void Tracer_CorrectTraceResultStructureWithTwoThreads()
        {
            ResultOutput resultOutput = new ResultOutput();
            Tracer.Interfaces.ITracer tracer = new Tracer.Tracer(resultOutput);
            Class1 foo = new Class1(tracer);

            Thread thread1 = new Thread(() =>
            {
                foo.MyMethod(1000);
            });

            Thread thread2 = new Thread(() =>
            {
                foo.MyMethod(2000);
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            Assert.Equal(2, traceResult.Threads.Count);

            foreach (var threadInfo in traceResult.Threads)
            {

                Assert.NotEmpty(threadInfo.Methods);
                Assert.Single(threadInfo.Methods);
                Assert.NotNull(threadInfo.Time);
                Assert.NotEqual(0, threadInfo.Id);

                foreach (var methodInfo in threadInfo.Methods)
                {

                    Assert.Equal("MyMethod", methodInfo.MethodName);
                    Assert.Equal("Class1", methodInfo.ClassName);
                    Assert.NotNull(methodInfo.ExecutionTime);

                    foreach (var innerMethodInfo in methodInfo.Methods)
                    {
                        Assert.Equal("InnerMethod", innerMethodInfo.MethodName);
                        Assert.Equal("Class2", innerMethodInfo.ClassName);
                        Assert.NotNull(methodInfo.ExecutionTime);

                        Assert.Empty(innerMethodInfo.Methods);
                    }
                }
            }
        }
    }
}