namespace DPAT_Finite_State_Machine.Model;

public class SimpleState : IState
{
    public List<Transition> Transitions { get; }
	public SimpleStateEnum StateType { get; }

    public Action Action { get; }

	public string Id { get; }
	public string Name { get; }

    public SimpleState(SimpleStateEnum stateType, string name)
    {
		StateType = stateType;
		Name = name;
    }
}
