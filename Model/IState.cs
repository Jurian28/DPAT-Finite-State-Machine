namespace DPAT_Finite_State_Machine.Model;

public interface IState
{
	public List<Transition> Transitions { get; }
	public Action Action { get; }
}
