# PinionCyber.StateManagement
�@��²�檺 c# ���A���u��.
## ²��
�o�ӱM�ץD�u�O���F�y�z���A���Ҧ��o�@�]�p�Ҧ�, �ä��O�@�ӸU�ΥB�j�j�����A���M��, �L�O�ΨӸѨM�C���}�o�����󪬺A�Ҧ��ݨD��²���]�p�Ҧ��u��.  
�L�S���̿�����Ҳթβ��~, ²�����N�X���Ω����M��.

## �p��ϥ�
**A.�w�q���A**  
�~�� ```PinionCyber.StateManagement.IState``` 
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    class StateA : PinionCyber.StateManagement.IState
    {                
        void IActivable.Disable()
        {
            // ���A����
        }

        void IActivable.Enable()
        {
            // ���A�_�l
        }

        void IUpdate.Update()
        {
            // ���A��s
        }
    }      
}

```
**B.�إߪ��A��**
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    public static void Main()
    {
        
        var machine = new PinionCyber.StateManagement.StateMachine();

        // ���A����
        var stateA = new StateA();
        machine.Change(stateA);
        var stateB = new StateB();  // �t�@�Ӫ��A
        machine.Change(stateB);

        // ���A��s
        machine.Activer().Update();

        // �M�Ū��A
        machine.Empty();
    }
}
```
## Cookbook 
���A���O�@�س]�p�Ҧ�, �W���N�X�B�z���O���A�����֤߳���, �ӹ�ڤW�����@�ӱM�׻ݨD�ٻݭn�@�Ǳ���B�z,  �H�U�����A���ϥΤ覡.  

### ���A����
�ϥΨƥ��X���A�����G, �H�U��StateA�PStateB������.
```csharp
class StateA : IState
{
    // ��@ IState �P�����{���X ... 

    // ���A�����ƥ�
    public event System.Action DoneEvent;
}

class StateB : IState
{
    // ��@ IState �P�����{���X ... 

    // StateB �P A �P
    public event System.Action DoneEvent;
}
```
�z�L ```DoneEvent``` ������Ӫ��A��������.
```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;
    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        // ���������� StateA
        _ToStateA();
    }
    
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // �� StateA �����h������ StateB
        _Machine.Change(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA;// �� StateB �����h������ StateA
        _Machine.Change(state);
    }
}
```
### ���A�s��
���A���q�`�O�@�����O�̪��p������, ��~���ݭn�s�����A�άO�ݭn���D�ثe�O���Ӫ��A�i�H�z�L�H�U��k.

�� StateA �� StateB �]�p�s����k.
```csharp
// �� StateA
interface IAccessible
{
    void Set(int val);
    int Get();
}

// �� StateB
interface IGetter
{
    string Get();
}

class StateA : IState , IAccessible
{
    // ��@ IState, IAccessible �P�����{���X ... 

    // ���A�����ƥ�
    public event System.Action DoneEvent;
}

class StateB : IState , IGetter
{
    // ��@ IState, IGetter �P�����{���X ... 

    // StateB �P A �P
    public event System.Action DoneEvent;
}
```
Sample ���ѥ~���ƥ� IAccessible �P IGetter
```csharp
class Sample
{
    readonly PinionCyber.StateManagement.StateMachine _Machine;

    // ���ѵ��~�����s������
    public event System.Action<IAccessible> AccessibleEvent;
    public event System.Action<IGetter> GetterEvent;

    public Sample()
    {
        _Machine = new PinionCyber.StateManagement.StateMachine();
        
    }
    // �Ыسo�ۤ�k��K�ƥ���U����Ұʪ�l���A
    // �Ϊ� class Sample �����]�i�H�O�Ӫ��A.
    public void Enable()
    {
        // ���������� StateA
        _ToStateA();
    }
    void _ToStateA()
    {
        var state = new StateA();       
        state.DoneEvent += _ToStateB; // �� StateA �����h������ StateB
        _Machine.Change(state);
        // ���A�q��
        AccessibleEvent(state);
    }    

    void _ToStateB()
    {
        var state = new StateB();
        state.DoneEvent += _ToStateA;// �� StateB �����h������ StateA
        _Machine.Change(state);
        // ���A�q��
        GetterEvent(state);
    }
}
```
��~�����O�ϥ� Sample �i�H�p�U�o�� Sample ���A.
```csharp

// �~�����O
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
    // ���o���A
    public SampleState GetSampleState()
    {
        return _SampleState;
    }
    // �B��R�O, ���� Sample ���A
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