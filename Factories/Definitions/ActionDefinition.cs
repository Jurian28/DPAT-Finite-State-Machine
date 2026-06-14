using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories.Definitions;

public class ActionDefinition(
	string ownerId, 
	string description, 
	ActionTypeEnum actionType
) {
	public string OwnerId { get; } = ownerId;
	public string Description { get; } = description;
	public ActionTypeEnum ActionType { get; } = actionType;
}
