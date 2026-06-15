namespace DPAT_Finite_State_Machine.Model;

public class SimpleState : IState {
	public string Id { get; }
	public string Name { get; }
	public SimpleStateEnum StateType { get; }
	public Action? Action { get; }

	public List<Transition> Transitions { get; } = [];

	public SimpleState(string id, string name, SimpleStateEnum stateType, Action? action) {
        Id = id;
		Name = name;
		StateType = stateType;
		Action = action;
    }

    public void AddTransition(Transition transition) {
		Transitions.Add(transition);
    }
}
