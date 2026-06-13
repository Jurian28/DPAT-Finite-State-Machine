namespace DPAT_Finite_State_Machine.Model;

public class CompoundState : IState
{
	public List<Transition> Transitions { get; }

    public Action Action { get;}
	public List<IState> SubStates { get; }

    public CompoundState(List<Transition> transitions, Action action, List<IState> subStates)
    {
        Transitions = transitions;
        Action = action;
        SubStates = subStates;
    }
}
