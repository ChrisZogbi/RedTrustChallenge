using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RedTrust
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                StringList stringList = new StringList(Utilities.MockList());
                stringList.ItemEnqueuedEventHandler += c_OnItemEnqueued;

                Thread t1 = new Thread(new ThreadStart(() => stringList.Consume()));
                t1.Name = "t1";

                Thread t2 = new Thread(new ThreadStart(() => stringList.Consume()));
                t2.Name = "t2";

                Thread t3 = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(10000);
                    stringList.Enqueue("Add");
                }));

                t1.Start();
                t2.Start();
                t3.Start();
            }
            catch (AggregateException e)
            {
                foreach (var ie in e.InnerExceptions)
                    Console.WriteLine("{0}: {1}", ie.GetType().Name, ie.Message);
            }
        }

        static void c_OnItemEnqueued(object sender, OnItemEnqueuedEventArgs e)
        {
            e.T1.Start();
            e.T2.Start();

            Environment.Exit(0);
        }
    }
}
