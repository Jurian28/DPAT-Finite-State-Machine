namespace DPAT_Finite_State_Machine.Models;

using Validators;

public class SimpleState(string id, string name, SimpleStateEnum stateType, List<Action> actions) 
	: IState
{
	public string Id { get; } = id;
	public string Name { get; } = name;
	public SimpleStateEnum StateType { get; } = stateType;
	public List<Action> Actions { get; } = actions;

	public List<Transition> Transitions { get; } = [];

	public void AddTransition(Transition transition) {
		Transitions.Add(transition);
    }
	
	public void Accept(IValidationVisitor visitor) {
		visitor.Visit(this);
	}
}
