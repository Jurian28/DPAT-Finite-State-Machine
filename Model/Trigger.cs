namespace DPAT_Finite_State_Machine.Model;

public class Trigger {
	public string Id { get; }
	public string Description { get; }

	public Trigger(string id, string description) {
		Id = id;
        Description = description;
	}
}
