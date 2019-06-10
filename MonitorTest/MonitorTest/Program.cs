using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MonitorSample obj = new MonitorSample();
            Thread tProduce = new Thread(new ThreadStart(obj.Produce));
            Thread tConsume = new Thread(new ThreadStart(obj.Consumer));
            //Start threads.  
            tProduce.Start();
            tConsume.Start();

            Console.ReadLine();
        }
    }


    class MonitorSample
    {
        private int n = 1;
        private int max = 1000;
        private object monitor = new object();
        public void Produce()
        {
            lock (monitor)
            {
                while (n < max)
                {
                    Console.WriteLine("妈妈：第" + n.ToString() + "块蛋糕做好了");


                    Monitor.Wait(monitor);
                    n++;
                }
            }
        }


        public void Consumer()
        {
            lock (monitor)
            {
                while (true)
                {
                    Monitor.Pulse(monitor);

                    Monitor.Wait(monitor, 1000);

                    Console.WriteLine("孩子：开始吃第" + n.ToString() + "块蛋糕");
                }
            }
        }
    }
}
