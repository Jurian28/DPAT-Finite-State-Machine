namespace DPAT_Finite_State_Machine.Models;

public class Action(string description, ActionTypeEnum actionType)
{
	public string Description { get; } = description;
	public ActionTypeEnum ActionType { get; } = actionType;
}
