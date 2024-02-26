namespace PinionCyber.StateManagement.Tests
{
    class TestCallState : TestCallSwitch, IState
    {
        public int UpdateCallCount;

        void IUpdate.Update()
        {
            UpdateCallCount++;
        }
    }
}