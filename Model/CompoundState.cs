namespace DPAT_Finite_State_Machine.Model;

public class CompoundState : IState {
	public string Id { get; }
	public string Name { get; }
	public Action? Action { get; }

	public List<IState> SubStates { get; private set; } = [];
	public List<Transition> Transitions { get; } = [];

    public CompoundState(string id, string name, Action? action) {
		Id = id;
		Name = name;
		Action = action;
    }

	public void SetSubStates(List<IState> subStates) {
		SubStates = subStates;
	}

	public void AddTransition(Transition transition) {
		Transitions.Add(transition);
	}
}
