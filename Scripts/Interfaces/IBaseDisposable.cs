using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleBall.Scripts.Interfaces
{
    public interface IBaseDisposable : IDisposable
    {
        public bool isDisposed { get; }
    }
}