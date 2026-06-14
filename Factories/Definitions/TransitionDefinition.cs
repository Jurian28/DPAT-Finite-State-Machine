namespace DPAT_Finite_State_Machine.Factories.Definitions;

public class TransitionDefinition(
	string id,
	string sourceStateId,
	string destinationStateId,
	string? triggerId,
	string guardCondition
) {
	public string Id { get; } = id;
	public string SourceStateId { get; } = sourceStateId;
	public string DestinationStateId { get; } = destinationStateId;
	public string? TriggerId { get; } = triggerId;
	public string GuardCondition { get; } = guardCondition;
}

