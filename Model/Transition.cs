namespace DPAT_Finite_State_Machine.Model;

public class Transition
{
	public Action Action { get; }
	public Trigger Trigger { get; }
	public IState ToState { get; }
	public string Condition { get; }

    public Transition(Action action, Trigger trigger, IState toState, string condition)
    {
        Action = action;
        Trigger = trigger;
        ToState = toState;
        Condition = condition;
    }
}
