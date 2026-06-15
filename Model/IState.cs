namespace DPAT_Finite_State_Machine.Model;

public interface IState {
	public string Id { get; }
	public string Name { get; }
	public List<Transition> Transitions { get; }
	public Action? Action { get; }

    void AddTransition(Transition transition);
}
