namespace DPAT_Finite_State_Machine.Models;

using Validators;

public interface IState {
	public string Id { get; }
	public string Name { get; }
	public List<Transition> Transitions { get; }

    void AddTransition(Transition transition);
    void Accept(IValidationVisitor visitor);
}
