
# PinionCyber.StateManagement
[![Maintainability](https://api.codeclimate.com/v1/badges/9d9186f471d28c542b17/maintainability)](https://codeclimate.com/github/jiowchern/PinionCyber.StateManagement/maintainability)[![codecov](https://codecov.io/gh/jiowchern/PinionCyber.StateManagement/graph/badge.svg?token=957MXN7E9S)](https://codecov.io/gh/jiowchern/PinionCyber.StateManagement)  


## Introduction
The state pattern is a very easy to understand design pattern.  
Taking an RPG character as an example, it can be divided into the following two states: Exploring and Fighting.  
![Sample](https://www.plantuml.com/plantuml/svg/SoWkIImgAStDuSh8J4bLICqjAAbKI4ajJYxAB2Z9pC_Z0igNf2e4v2HMfXOfL7CfA2Y0i89hHK5EVb5cNhgkdO9RPdf62P29Ac6bu9jVbWeIWoW0gqL8eqWeM2aubfGa9cTprN9nSJcavgK0NGG0)  
Usually we can design a class called `Player` as follows, and use an enumeration to record the current state.
```csharp
class Player
{
    public enum State
    {
        Exploring,
        Fighting,
    }
    // Use State to record the current state of the Player.
    State _state;       
}
```
Usually it is controlled in `Player.ChangeState` whether to switch the state or not.
```csharp
public void ChangeState(State state)
{
    _state = state;
}
```
Then it is necessary to provide control methods, and in this case it is necessary to design the methods of movement and battle.  
```csharp
public void Move(int x, int y)
{
    if (_state != State.Exploring)
    {
        throw new InvalidOperationException("Cannot move while not exploring.");
    }

    // Move the player to the specified location.
}

public void Attack(Enemy enemy)
{
    if (_state != State.Fighting)
    {
        throw new InvalidOperationException("Cannot attack while not fighting.");
    }

    // Deal damage to the enemy.
}
```
Since the owner of the Player needs to know the current state of the Player in order to know whether to call `Battle` or `Move`, it needs to provide a method to get the state.  
```csharp
public State GetState()
{
    return _state;
}
```
Here's an example of using `Player`  
```csharp
var player = new Player();

// The player is initially in the exploring state.
Console.WriteLine(player.GetState()); // Exploring

// The player can move while exploring.
player.Move(10, 20);

// The player can change their state to fighting.
player.ChangeState(Player.State.Fighting);

// The player can attack enemies while fighting.
player.Attack(new Enemy());

```  
There are a few drawbacks to the above implementation.
1. The control method needs to know the current state before it can be used, and this is a problem that can only be solved by implementing code that understands the relationship between the state and the control method, except for a well-written development document for the class.  
2. If the design architecture needs to become a nested structure, it will need to add a new `_stateXX` to manage the sub-states, which will increase the complexity of the code.  

## Use `PinionCyber.StateManagement`
**PinionCyber.StateManagement** provides a very simple state design pattern, it is not a powerful and all-encompassing suite of state machines, but only provides some simple modules for developers to build their own state patterns.  

### Implementation status
The two states of the implementation are typed as follows.  
```csharp
class ExploringState
{
    public void Move(int x,int y)
    {
        if(_HasMonster(x,y))
        {
            EnemyEvent();
        }
    }

    public event Syste.Action EnemyEvent;
}

class FightingState
{
    public void Attack(Enemy enemy)
    {
        if(enemy.IsDead())
        {
            VictoryEvent();
        }
    }

    public event Syste.Action VictoryEvent;
}
```
Inherit the state class from `PinionCyber.StateManagement.IState`.  
The state class needs to implement the `Enable` `Disable` and `Update` methods.  

```csharp
class ExploringState : PinionCyber.StateManagement.IState
{
    public void Move(int x,int y)
    {
        if(_HasMonster(x,y))
        {
            EnemyEvent();
        }
    }

    public event Syste.Action EnemyEvent;

    void IActivable.Disable()
    {
        // Call on status release.
    }

    void IActivable.Enable()
    {
        // Initialize the state.
    }

    void IUpdate.Update()
    {
        // Update the state.
    }
}

class FightingState : PinionCyber.StateManagement.IState
{
    public void Attack(Enemy enemy)
    {
        if(enemy.IsDead())
        {
            VictoryEvent();
        }
    }

    public event Syste.Action VictoryEvent;

    void IActivable.Disable()
    {
        // Call on status release.
    }

    void IActivable.Enable()
    {
        // Initialize the state.
    }

    void IUpdate.Update()
    {
        // Update the state.
    }
}
```
### Use `PinionCyber.StateManagement.StateMachine`  
`PinionCyber.StateManagement.StateMachine` is used to manage the state switching class in the following way.
```csharp
class Player
{
    readonly PinionCyber.StateManagement.StateMachine _machine;
    
    public void Enable()
    {   
        // Initialize the first state of the state machine   
        _toExploring();
    }

    
    void _toExploring()
    {        
        var state = new ExploringState();
        // If an enemy is encountered, switch to the fighting state.
        state.EnemyEvent += _toFighting;
        _machine.Change(state);
    }

    void _toFighting()
    {
        var state = new FightingState();
        // End the fight and switch to the exploring state.
        state.VictoryEvent += _toExploring;
        _machine.Change(state);
    }
}
```
It is used as follows  
```csharp
var player = new Player();
player.Enable();// Exploring
```
Obviously we need to be able to control the player state.  
The way to do this is very simple, just create events and hang them before `Player.Enable` is called.  
This works like this.  
```csharp
var player = new Player();
player.ExploringEvent += (state)=>{
    // in Exploring
    state.Move(...);
};
player.FightingEvent += (state)=>{
    // in Exploring
    state.Attack(...);
};
player.Enable();// Exploring
```
`Player` is implemented as follows.   
```csharp
class Player
{
    readonly PinionCyber.StateManagement.StateMachine _machine;
    public System.Action<ExploringState> ExploringEvent;
    public System.Action<FightingState> FightingEvent;
    public void Enable()
    {   
        // Initialize the first state of the state machine   
        _toExploring();
    }

    
    void _toExploring()
    {        
        var state = new ExploringState();
        // If an enemy is encountered, switch to the fighting state.
        state.EnemyEvent += _toFighting;
        _machine.Change(state);
        ExploringEvent(state);
    }

    void _toFighting()
    {
        var state = new FightingState();
        // End the fight and switch to the exploring state.
        state.VictoryEvent += _toExploring;
        _machine.Change(state);
        FightingEvent(state);
    }

    //  If the project needs to keep updating the state then you can use the following method to call `Update`.
    public void Update()
    {
        _machine.Activer().Update();
    }
}
```
This way the state will be encapsulated and the code can be easily maintained and expanded without the need for enumeration.

## Install
**Download** [![nuget](https://buildstats.info/nuget/PinionCyber.StateManagement)  ](https://www.nuget.org/packages/PinionCyber.StateManagement)

