

namespace PinionCyber.StateManagement
{
    
    public class StateMachine : Switcher<IState>         
    {
        class EmptyState : IState
        {
            void ISwitch.End()
            {
                
            }

            void ISwitch.Start()
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
            Switch(new EmptyState());
        }
    }


}
