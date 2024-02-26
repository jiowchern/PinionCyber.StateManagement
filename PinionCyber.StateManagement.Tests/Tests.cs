using NUnit.Framework;

namespace PinionCyber.StateManagement.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Switch()
        {
            var state = new TestCallSwitch();
            {
                using var controller = new Switcher<ISwitch>(new TestCallSwitch());
                controller.Switch(state);
            }
            Assert.AreEqual(1, state.EndCallCount);
            Assert.AreEqual(1, state.StartCallCount);
        }
        [Test]
        public void StateWithDispose()
        {
            var state = new TestCallState();
            {
                using var machine = new StateMachine();
                machine.Switch(state);
                machine.Activer().Update();
                
            }
            Assert.AreEqual(1, state.EndCallCount);
            Assert.AreEqual(1, state.StartCallCount);
            Assert.AreEqual(1, state.UpdateCallCount);
        }
        [Test]
        public void StateWithEmpty()
        {
            var state = new TestCallState();
            var machine = new StateMachine();
            machine.Switch(state);
            machine.Activer().Update();
            machine.Empty();
            Assert.AreEqual(1, state.EndCallCount);
            Assert.AreEqual(1, state.StartCallCount);
            Assert.AreEqual(1, state.UpdateCallCount);
        }
    }
}