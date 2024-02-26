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
            var controller = new Switcher<ISwitch>(new TestCallSwitch());
            controller.Switch(state);
            controller.Switch(new TestCallSwitch());

                        
            Assert.That(1,Is.EqualTo(state.EndCallCount));
            Assert.That(1, Is.EqualTo(state.StartCallCount));
        }
       
        [Test]
        public void StateWithEmpty()
        {
            var state = new TestCallState();
            var machine = new StateMachine();
            machine.Switch(state);
            machine.Activer().Update();
            machine.Empty();
            Assert.That(1, Is.EqualTo(state.EndCallCount));
            Assert.That(1, Is.EqualTo(state.StartCallCount));
            Assert.That(1, Is.EqualTo(state.UpdateCallCount));
            
        }
    }
}