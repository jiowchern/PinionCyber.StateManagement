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
        public void ActiverChanger()
        {
            var state = new TestCallSwitch();
            var controller = new ActiverChanger<IActivable>(new TestCallSwitch());
            controller.Change(state);
            controller.Change(new TestCallSwitch());

                        
            Assert.That(1,Is.EqualTo(state.EndCallCount));
            Assert.That(1, Is.EqualTo(state.StartCallCount));
        }
       
        [Test]
        public void StateMachine()
        {
            var state = new TestCallState();
            var machine = new StateMachine();
            machine.Change(state);
            machine.Activer().Update();
            machine.Empty();
            Assert.That(1, Is.EqualTo(state.EndCallCount));
            Assert.That(1, Is.EqualTo(state.StartCallCount));
            Assert.That(1, Is.EqualTo(state.UpdateCallCount));
            
        }
    }
}