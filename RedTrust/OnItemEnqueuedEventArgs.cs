using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedTrust
{
    public class OnItemEnqueuedEventArgs : EventArgs
    {
        public Thread T1 { get; set; }
        public Thread T2 { get; set; }
    }
}
