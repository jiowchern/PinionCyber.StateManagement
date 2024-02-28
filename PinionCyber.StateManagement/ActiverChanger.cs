using System;

namespace PinionCyber.StateManagement
{
    public class ActiverChanger<T> where T : IActivable
    {
        T _Activable;
        /// <summary>
        /// An empty implementation is usually passed here.
        /// This constructor calls the IActivable.Enable on the parameter.
        /// </summary>
        /// <param name="first"></param>
        public ActiverChanger(T first)
        {
            _Activable = first;
            _Activable.Enable();
        }

        /// <summary>
        /// Changes the current activable object to the provided one and manages their active states.
        /// It disables the current activable object before switching to the new one, which is then enabled.
        /// </summary>
        /// <param name="activer">The new activable object to switch to.</param>
        public void Change(T activer)
        {
            _Activable.Disable();
            _Activable = activer;
            _Activable.Enable();
        }

        /// <summary>
        /// Returns the current activable object.
        /// </summary>
        /// <returns>The current activable object.</returns>
        public T Activer()
        {
            return _Activable;
        }
    }
}
