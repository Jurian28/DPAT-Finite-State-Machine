namespace DPAT_Finite_State_Machine.Model;

public class Diagram
{
    public List<IState> States { get; }

    public Diagram(List<IState> states)
    {
        States = states;
    }
}
