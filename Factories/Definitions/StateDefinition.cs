using DPAT_Finite_State_Machine.Models;

namespace DPAT_Finite_State_Machine.Factories.Definitions;

public class StateDefinition(
	string id, 
	string name, 
	SimpleStateEnum? stateType, // null when its a compoundState
	string? parentId
) {
	public string Id { get; } = id;
	public string Name { get; } = name;
	public SimpleStateEnum? StateType { get; } = stateType;
	public string? ParentId { get; } = parentId;
}
