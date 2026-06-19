namespace DPAT_Finite_State_Machine.Models;

using Validators;

public class CompoundState(string id, string name, List<Action> actions) : IState
{
	public string Id { get; } = id;
	public string Name { get; } = name;
	public List<Action> Actions { get; } = actions;

	public List<IState> SubStates { get; private set; } = [];
	public List<Transition> Transitions { get; } = [];

	public void SetSubStates(List<IState> subStates) {
		SubStates = subStates;
	}

	public void AddTransition(Transition transition) {
		Transitions.Add(transition);
	}
	
	public void Accept(IValidationVisitor visitor) {
		visitor.Visit(this);
	}
}
