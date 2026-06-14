namespace DPAT_Finite_State_Machine.Model;

public class Action
{
	public string Id { get; }
	public string Description { get; }

    public Action(string id, string description)
    {
		Id = id;
        Description = description;
    }
}
