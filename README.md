# PinionCyber.StateManagement
一個簡單的 c# 狀態機工具.
## 簡介
這個專案主只是為了描述狀態機模式這一設計模式, 並不是一個萬用且強大的狀態機套件, 他是用來解決遊戲開發中任何狀態模式需求的簡易設計模式工具.  
他沒有依賴於任何模組或產品, 簡易的代碼易用於任何專案.

## 如何使用
**A.定義狀態**
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    class StateA :  PinionCyber.StateManagement.IState
    {                
        void ISwitch.End()
        {
            // 狀態結束
        }

        void ISwitch.Start()
        {
            // 狀態起始
        }

        void IUpdate.Update()
        {
            // 狀態更新
        }
    }    

    class StateB :  PinionCyber.StateManagement.IState
    {                
        void ISwitch.End()
        {
            // 狀態結束
        }

        void ISwitch.Start()
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
        machine.Switch(stateA);
        var stateB = new StateB();  
        machine.Switch(stateB);

        // 狀態更新
        machine.Activer().Update();

        // 清空狀態
        machine.Empty();
    }
}
```

<!-- 
## 完整的使用情境
狀態機是一種設計模式, 上面代碼處理的是狀態機的核心部份, 而實際上完成一個專案需求還需要一些控制處理,  以下為一個遊戲角色的狀態機使用情境.  

假如你有一個類叫做```Player``` 且定義了如下屬性
```csharp
namespace PinionCyber.StateManagement.SamplePlayer
{
    public class Player
    {
        public string Name;
        public int Hp;
        public int Atk;
        public int Def;
        public int Money;
    }
}
```
-->