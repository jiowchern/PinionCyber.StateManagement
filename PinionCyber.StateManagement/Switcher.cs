

using System;

namespace PinionCyber.StateManagement
{
    public class Switcher<T> : System.IDisposable where T : ISwitch 
    {
        T _Switch;

        public Switcher(T first)
        {
            _Switch = first;
            _Switch.Start();
        }
        public void Switch(T state)
        {
            _Switch.End();
            _Switch = state;
            _Switch.Start();
        }

        void IDisposable.Dispose()
        {
            _Switch.End();
        }

        public T Activer()
        {
            return _Switch;
        }
    }
}
