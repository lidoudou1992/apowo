using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyModel
{
    public abstract class DisposeObject:IDisposable
    {
        protected bool IsDisposed = false;
        public abstract void Dispose();        
    }
}
