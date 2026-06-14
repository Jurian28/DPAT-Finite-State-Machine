namespace DPAT_Finite_State_Machine.Model;

public class CompoundState : IState
{
	public List<Transition> Transitions { get; }

    public Action Action { get; }
	public List<IState> SubStates { get; }

	public string Id { get; }
	public string Name { get; }

    public CompoundState(string name)
    {
		Name = name;
    }
}
