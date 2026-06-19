namespace DPAT_Finite_State_Machine.Validators;

using Models;

public class DeterminismValidator : ValidationVisitor
{
	public override string Name => "Determinism";

	public override void Visit(SimpleState simpleState) => CheckOutgoing(simpleState);

	public override void Visit(CompoundState compoundState) => CheckOutgoing(compoundState);

	private void CheckOutgoing(IState state)
	{
		var groups = state.Transitions
			.GroupBy(t => (trigger: t.Trigger?.Id ?? string.Empty, guard: t.Condition ?? string.Empty));

		foreach (var group in groups)
		{
			if (group.Count() <= 1) continue;

			var trigger = string.IsNullOrEmpty(group.Key.trigger)
				? "no trigger"
				: $"trigger '{group.Key.trigger}'";
			var guard = string.IsNullOrEmpty(group.Key.guard)
				? "no guard"
				: $"guard '{group.Key.guard}'";

			AddError(
				$"State '{state.Name}' ({state.Id}) has {group.Count()} outgoing transitions " +
				$"with {trigger} and {guard} (non-deterministic).");
		}
	}
}
