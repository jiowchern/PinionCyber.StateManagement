# PinionCyber.StateManagement
一個簡單的 c# 狀態機工具.
## 簡介
這個專案主只是為了描述狀態機模式這一設計模式, 並不是一個萬用且強大的狀態機套件, 他是用來解決遊戲開發中任何狀態模式需求的簡易設計模式工具.  
他沒有依賴於任何模組或產品, 簡易的代碼易用於任何專案.

## 如何使用
**A.定義狀態**  
繼承 ```PinionCyber.StateManagement.IState``` 
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    class StateA : PinionCyber.StateManagement.IState
    {                
        void IActivable.Disable()
        {
            // 狀態結束
        }

        void IActivable.Enable()
        {
            // 狀態起始
        }

        void IUpdate.Update()
        {
            // 狀態更新
        }
    }      
}

```
**B.建立狀態機**
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    public static void Main()
    {
        
        var machine = new PinionCyber.StateManagement.StateMachine();

        // 狀態切換
        var stateA = new StateA();
        machine.Change(stateA);
        var stateB = new StateB();  // 另一個狀態
        machine.Change(stateB);

        // 狀態更新
        machine.Activer().Update();

        // 清空狀態
        machine.Empty();
    }
}
```
## Cookbook 
狀態機是一種設計模式, 上面代碼處理的是狀態機的核心部份, 而實際上完成一個專案需求還需要一些控制處理,  以下為狀態機使用方式.  

### 狀態切換
使用事件輸出狀態的結果, 以下為StateA與StateB的切換.
```csharp
class StateA : IState
{
    // 實作 IState 與相關程式碼 ... 

    // 狀態完成事件
    public event System.Action DoneEvent;
}

class StateB : IState
{
    // 實作 IState 與相關程式碼 ... 

    // StateB 與 A 同
    public event System.Action DoneEvent;
}
```
透過 ```DoneEvent``` 完成兩個狀態間的切換.
```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;
    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        // 首先先切到 StateA
        _ToStateA();
    }
    
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // 當 StateA 完成則切換到 StateB
        _Machine.Change(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA;// 當 StateB 完成則切換到 StateA
        _Machine.Change(state);
    }
}
```
### 狀態存取
狀態機通常是一個類別裡的私有物件, 當外部需要存取狀態或是需要知道目前是哪個狀態可以透過以下方法.

為 StateA 跟 StateB 設計存取方法.
```csharp
// 給 StateA
interface IAccessible
{
    void Set(int val);
    int Get();
}

// 給 StateB
interface IGetter
{
    string Get();
}

class StateA : IState , IAccessible
{
    // 實作 IState, IAccessible 與相關程式碼 ... 

    // 狀態完成事件
    public event System.Action DoneEvent;
}

class StateB : IState , IGetter
{
    // 實作 IState, IGetter 與相關程式碼 ... 

    // StateB 與 A 同
    public event System.Action DoneEvent;
}
```
Sample 提供外部事件 IAccessible 與 IGetter
```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;

    // 提供給外部的存取物件
    public event System.Action<IAccessible> AccessibleEvent;
    public event System.Action<IGetter> GetterEvent;

    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        
    }
    // 創建這著方法方便事件註冊完後啟動初始狀態
    // 或者 class Sample 本身也可以是個狀態.
    public void Enable()
    {
        // 首先先切到 StateA
        _ToStateA();
    }
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // 當 StateA 完成則切換到 StateB
        _Machine.Change(state);
        // 狀態通知
        AccessibleEvent(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA;// 當 StateB 完成則切換到 StateA
        _Machine.Change(state);
        // 狀態通知
        GetterEvent(state);
    }
}
```
當外部類別使用 Sample 可以如下得知 Sample 狀態.
```csharp

// 外部類別
class Controller
{
    public enum SampleState { AccessState , GetState }
    SampleState _SampleState;
    delegate System.Func<string,string> _CommandHandler;
    readonly Sample _Sample;
    public Controller()
    {
        _Sample = new Sample();
        _Sample.AccessibleEvent += (access) =>{
            _SampleState = SampleState.AccessState;
            _CommandHandler = _CreateHandler(access);
        };
        _Sample.GetterEvent += (getter) =>{
            _SampleState = SampleState.GetState;
            _CommandHandler = _CreateHandler(getter);
        };
    }
    public void Enable()
    {
        _Sample.Enable();
    }
    // 取得狀態
    public SampleState GetSampleState()
    {
        return _SampleState;
    }
    // 運行命令, 控制 Sample 狀態
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