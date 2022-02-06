using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedTrust
{
    public class StringList
    {
        public List<string> Queue { get; set; }

        public StringList(List<string> queue)
        {
            Queue = queue;
        }

        public void Enqueue(string item)
        {
            this.Queue.Add(item);

            Thread t1 = new Thread(new ThreadStart(() => this.Consume()));
            Thread t2 = new Thread(new ThreadStart(() => this.Consume()));

            OnItemEnqueuedEventArgs args = new OnItemEnqueuedEventArgs();
            args.T1 = new Thread(new ThreadStart(() => this.Consume()));
            args.T1.Name = "T1 item enqueued";

            args.T2 = new Thread(new ThreadStart(() => this.Consume()));
            args.T2.Name = "T2 item enqueued";

            ItemEnqueuedEventHandler(this, args);
        }

        public string Consume()
        {
            while (true)
            {
                Monitor.Enter(Queue);

                if (this.Count() == 0)
                {
                    Monitor.Exit(Queue);
                    Thread.Sleep(600000);
                }

                var value = this.Dequeue();
                Console.WriteLine($"{Thread.CurrentThread.Name}: print value --> {value}");

                Monitor.Exit(Queue);
                Thread.Sleep(500);
            }
        }

        public string Dequeue()
        {
            var value = Queue.First();
            Queue.Remove(value);
            return value;
        }

        public int Count() => Queue.Count();

        protected virtual void OnItemEnqueued(OnItemEnqueuedEventArgs e)
        {
            EventHandler<OnItemEnqueuedEventArgs> handler = ItemEnqueuedEventHandler;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<OnItemEnqueuedEventArgs> ItemEnqueuedEventHandler;
    }


}
