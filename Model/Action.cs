namespace DPAT_Finite_State_Machine.Model;

public class Action {
	public string Description { get; }
	public ActionTypeEnum ActionType { get; }

    public Action(string description, ActionTypeEnum actionType) {
        Description = description;
		ActionType = actionType;
    }
}
