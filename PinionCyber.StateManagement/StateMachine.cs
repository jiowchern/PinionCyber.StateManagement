

namespace PinionCyber.StateManagement
{
    
    public class StateMachine : ActiverChanger<IState>         
    {
        class EmptyState : IState
        {
            void IActivable.Disable()
            {
                
            }

            void IActivable.Enable()
            {
                
            }

            void IUpdate.Update()
            {
                
            }
        }
        public StateMachine() : base(new EmptyState())
        {

        }
        public StateMachine(IState first) : base(first)
        {

        }
        public void Empty()
        {
            Change(new EmptyState());
        }
    }


}
