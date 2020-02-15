using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProcessing
{
    class Program
    {
        IList<Task> taskList = new List<Task>();
        static void Main(string[] args)
        {

            Program p = new Program();
            Console.WriteLine("1");
            Task t1 = p.GoAsync(1);
            Console.WriteLine("2");
            Task t2 = p.GoAsync(2);
            Console.WriteLine("3");
            Task t3 = p.GoAsync(3);
            Console.WriteLine("4");

            Console.WriteLine("Work for Success! 1");

            Task.WaitAll(Task.Delay(10000));

            Console.WriteLine("Work for Success! 2");

            Task.WaitAll(t1, t2, t3);

            Console.WriteLine("Work for Success! 3");

            //t4.Result this code make below code sequential. It waits to get data from GetInnerText function
            var t4 = p.GetInnerText("http://www.google.com/");
            Console.WriteLine(t4.Result);

            Console.WriteLine("Work for Google!");

            //Exception Handling with Threads
            Task.WaitAll(p.Caller());

            Console.WriteLine("Work for Throw!");
        }

        public async Task GoAsync(int i)
        {
            Console.WriteLine("Starting Slept " + i + ".");

            await Task.Delay(1000);

            await Sleep(5000, i);

            await Task.Delay(1000);

            await Sleep(3000, i);

            //int[] result = await Task.WhenAll(task1, task2);

            Console.WriteLine("Finished Slept for a total of " + i + ".");
        }

        public async Task GoWithThrowAsync(int i)
        {
            Console.WriteLine("Starting Slept " + i + ".");

            await Task.Delay(1000);

            await Sleep(5000, i);

            throw new Exception("My Trial");

            await Task.Delay(1000);

            await Sleep(3000, i);

            //int[] result = await Task.WhenAll(task1, task2);

            Console.WriteLine("Finisched Slept for a total of " + i + ".");
        }

        private async Task<int> Sleep(int ms, int i)
        {
            Console.WriteLine(i + ".-" + "Sleeping for {0} at {1}", ms, Environment.TickCount);

            await Task.Delay(1);

            Console.WriteLine(i + ".-" + "Sleeping for {0} finished at {1}", ms, Environment.TickCount);
            return ms;
        }

        async Task<string> GetInnerText(string URL)
        {
            HttpClient c = new HttpClient();
            var t = await c.GetStringAsync(URL);
            return t;
        }

        async Task Caller()
        {
            try
            {
                await GoWithThrowAsync(5);
                await GoWithThrowAsync(6);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
