# PinionCyber.StateManagement
�@��²�檺 c# ���A���u��.
## ²��
�o�ӱM�ץD�u�O���F�y�z���A���Ҧ��o�@�]�p�Ҧ�, �ä��O�@�ӸU�ΥB�j�j�����A���M��, �L�O�ΨӸѨM�C���}�o�����󪬺A�Ҧ��ݨD��²���]�p�Ҧ��u��.  
�L�S���̿�����Ҳթβ��~, ²�����N�X���Ω����M��.

## �p��ϥ�
**A.�w�q���A**
```csharp
namespace PinionCyber.StateManagement.Sample1
{
    class StateA :  PinionCyber.StateManagement.IState
    {                
        void ISwitch.End()
        {
            // ���A����
        }

        void ISwitch.Start()
        {
            // ���A�_�l
        }

        void IUpdate.Update()
        {
            // ���A��s
        }
    }    

    class StateB :  PinionCyber.StateManagement.IState
    {                
        void ISwitch.End()
        {
            // ���A����
        }

        void ISwitch.Start()
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
        machine.Switch(stateA);
        var stateB = new StateB();  
        machine.Switch(stateB);

        // ���A��s
        machine.Activer().Update();

        // �M�Ū��A
        machine.Empty();
    }
}
```

<!-- 
## ���㪺�ϥα���
���A���O�@�س]�p�Ҧ�, �W���N�X�B�z���O���A�����֤߳���, �ӹ�ڤW�����@�ӱM�׻ݨD�ٻݭn�@�Ǳ���B�z,  �H�U���@�ӹC�����⪺���A���ϥα���.  

���p�A���@�����s��```Player``` �B�w�q�F�p�U�ݩ�
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