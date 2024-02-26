# PinionCyber.StateManagement
A simple C# state machine tool.


## Introduction
This project is designed to describe the state machine pattern, not as a universal and powerful state machine package. It's a simple design pattern tool aimed at solving any state pattern needs in game development. It does not depend on any modules or products, making it easy to use in any project.

## How to Use
**A. Defining States**  
Inherit ```PinionCyber.StateManagement.IState```
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    class StateA : PinionCyber.StateManagement.IState
    {                
        void ISwitch.End()
        {
             // State ends
        }

        void ISwitch.Start()
        {
            // State starts
        }

        void IUpdate.Update()
        {
            // State update
        }
    }      
}

```
**B. Creating a State Machine**
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    public static void Main()
    {
        
        var machine = new PinionCyber.StateManagement.StateMachine();

        // Switching states
        var stateA = new StateA();
        machine.Switch(stateA);
        var stateB = new StateB();  // Another state
        machine.Switch(stateB);

        // Updating state
        machine.Active().Update();

        // Clearing states
        machine.Empty();
    }
}
```
## Cookbook 
State machines are a design pattern. The above code handles the core part of a state machine. To fulfill a project's requirements, some control handling is needed. Below are the ways to use a state machine.

### State Switching
Use events to output the result of a state. Below is the switching between StateA and StateB.

```csharp
class StateA : IState
{
    // Implement IState and related code... 

    // State completion event
    public event System.Action DoneEvent;
}

class StateB : IState
{
    // Implement IState and related code... 

    // Same as StateB with A
    public event System.Action DoneEvent;
}
```
Switch between two states using ```DoneEvent```.

```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;
    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        // First switch to StateA
        _ToStateA();
    }
    
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // When StateA completes, switch to StateB
        _Machine.Switch(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA; // When StateB completes, switch back to StateA
        _Machine.Switch(state);
    }
}
```
### State Access

When the state machine is a private object within a class and external access to the state or knowledge of the current state is needed, use the following methods.
Design access methods for StateA and StateB.

```csharp
// For StateA
interface IAccessible
{
    void Set(int val);
    int Get();
}

// For StateB
interface IGetter
{
    string Get();
}

class StateA : IState, IAccessible
{
    // Implement IState, IAccessible and related code... 

    // State completion event
    public event System.Action DoneEvent;
}

class StateB : IState, IGetter
{
    // Implement IState, IGetter and related code... 

    // Same as StateB with A
    public event System.Action DoneEvent;
}
```
Sample provides external events IAccessible and IGetter.

```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;

    // Access objects provided to the outside
    public event System.Action<IAccessible> AccessibleEvent;
    public event System.Action<IGetter> GetterEvent;

    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        
    }
    // This method makes it easy to start the initial state after event registration
    // Or the class Sample itself can also be a state.
    public void Start()
    {
        // First switch to StateA
        _ToStateA();
    }
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // When StateA completes, switch to StateB
        _Machine.Switch(state);
        // State notification
        AccessibleEvent?.Invoke(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA; // When StateB completes, switch back to StateA
        _Machine.Switch(state);
        // State notification
        GetterEvent?.Invoke(state);
    }
}
```
When an external class uses Sample, it can determine the Sample state as follows.

```csharp

// External class
class Controller
{
    public enum SampleState { AccessState, GetState }
    SampleState _SampleState;
    delegate System.Func<string,string> _CommandHandler;
    public void Start()
    {
        var sample = new Sample();
        sample.AccessibleEvent += (access) =>{
            _SampleState = SampleState.AccessState;
            _CommandHandler = _CreateHandler(access);
        };
        sample.GetterEvent += (getter) =>{
            _SampleState = SampleState.GetState;
            _CommandHandler = _CreateHandler(getter);
        };
        sample.Start();
    }
    // Get state
    public SampleState GetSampleState()
    {
        return _SampleState;
    }
    // Execute command, control Sample state
    public string RunCommand(string cmd)
    {
        return _CommandHandler(cmd);
    }

    System.Func<string,string> _CreateHandler(IAccessible access)
    {
        return (cmd)=>{
            if(cmd == "set1")
            {
                access.Set(1);
                return "done";
            }
            else if(cmd == "get")
            {                
                return access.Get().ToString();  
            }
            return "";
        };
    }

    System.Func<string,string> _CreateHandler(IGetter getter)
    {
        return (cmd)=>{            
            if(cmd == "get")
            {                
                return getter.Get();  
            }
            return "";
        };
    }
}
```