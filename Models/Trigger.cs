namespace DPAT_Finite_State_Machine.Models;

public class Trigger(string id, string description)
{
	public string Id { get; } = id;
	public string Description { get; } = description;
}
