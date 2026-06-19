namespace DPAT_Finite_State_Machine.Models;

using Validators;

public class Transition(Action? action, Trigger? trigger, IState toState, string condition)
{
	public Action? Action { get; } = action;
    public Trigger? Trigger { get; } = trigger;
    public IState ToState { get; } = toState;
    public string Condition { get; } = condition;

    public void Accept(IValidationVisitor visitor) {
        visitor.Visit(this);
    }
}
