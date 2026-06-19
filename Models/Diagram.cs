namespace DPAT_Finite_State_Machine.Models;

using Validators;

public class Diagram(List<IState> states, List<Transition> transitions)
{
    public List<IState> States { get; } = states;
    private List<Transition> Transitions { get; } = transitions;
    
    public void Accept(IValidationVisitor visitor) {
        foreach (var state in States) {
            state.Accept(visitor);
        }
        foreach (var transition in Transitions) {
            transition.Accept(visitor);
        }
        visitor.Visit(this);
    }
}
