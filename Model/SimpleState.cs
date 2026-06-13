namespace DPAT_Finite_State_Machine.Model;

public class SimpleState : IState
{
    public List<Transition> Transitions { get; }
	public SimpleStateEnum StateType { get; }

    public Action Action { get; }
	public string Name { get; }

    public SimpleState(List<Transition> transitions, SimpleStateEnum stateType, Action action, string name)
    {
        Transitions = transitions;
        StateType = stateType;
        Action = action;
        Name = name;
    }
}
