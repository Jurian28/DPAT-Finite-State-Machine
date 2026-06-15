namespace DPAT_Finite_State_Machine.Model;

public class Diagram
{
    public List<IState> States { get; }
	public List<Transition> Transitions { get; }

    public Diagram(List<IState> states, List<Transition> transitions) {
        States = states;
		Transitions = transitions;
    }
}
