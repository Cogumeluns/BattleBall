using System;

namespace BattleBall.Scripts.Interfaces
{
    public interface IBaseDisposable : IDisposable
    {
        public bool isDisposed { get; }
    }
}