namespace PinionCyber.StateManagement.Tests
{
    class TestCallSwitch : IActivable
    {
        public int EndCallCount;
        public int StartCallCount;

        void IActivable.Disable()
        {
            EndCallCount++;
        }

        void IActivable.Enable()
        {
            StartCallCount++;
        }
    }
}