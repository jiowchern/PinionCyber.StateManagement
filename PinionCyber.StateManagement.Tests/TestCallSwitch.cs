namespace PinionCyber.StateManagement.Tests
{
    class TestCallSwitch : ISwitch
    {
        public int EndCallCount;
        public int StartCallCount;

        void ISwitch.End()
        {
            EndCallCount++;
        }

        void ISwitch.Start()
        {
            StartCallCount++;
        }
    }
}