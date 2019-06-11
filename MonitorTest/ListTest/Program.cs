using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListTest
{
    class Program
    {
        static object QueueLock = new object();
        static List<int> Values = new List<int>();
        static void Main(string[] args)
        {
            Thread tProduce = new Thread(new ThreadStart(Producer));
            Thread tConsume = new Thread(new ThreadStart(Consumer));
            //Start threads.  
            tProduce.Start();
            tConsume.Start();
            tProduce.Join();
            tConsume.Join();
            Console.ReadLine();

        }



        static void Producer()
        {
            int i = 0;
            lock (QueueLock)
            {
                while (i < 1000)
                {

                
                        
                    Monitor.Pulse(QueueLock);
                    Values.Add(i);
                    i++;
                    Monitor.Wait(QueueLock);
                }
            }
        }

        static void Consumer()
        {
            lock (QueueLock)
            {
                while (true)
                {
                    Monitor.Pulse(QueueLock);
                    if (Monitor.Wait(QueueLock, 1000))
                    {
                        while (Values.Count > 20)
                        {
                            Console.WriteLine($"数量策略 开始消费数据 Count:{Values.Count}");
                            Values = new List<int>();
                        }
                    }
                    else
                    {
                        if (Values.Count > 0)
                        {
                            Console.WriteLine($"时间策略 开始消费数据 Count:{Values.Count}");
                            Values = new List<int>();
                        }
                    }
                }
            }
        }
    }
}
