namespace DPAT_Finite_State_Machine.Validators;

using Models;

public class ReachableValidator : ValidationVisitor
{
	public override string Name => "Reachability";

	public override void Visit(SimpleState simpleState) => CheckShadowedTransitions(simpleState);

	public override void Visit(CompoundState compoundState) => CheckShadowedTransitions(compoundState);
	
	
	private void CheckShadowedTransitions(IState state)
	{
		var hasUnconditional = state.Transitions
			.Any(t => t.Trigger is null && string.IsNullOrEmpty(t.Condition));

		if (!hasUnconditional || state.Transitions.Count <= 1) return;

		foreach (var transition in state.Transitions)
		{
			var isUnconditional = transition.Trigger is null && string.IsNullOrEmpty(transition.Condition);

			if (!isUnconditional)
				AddError(
					$"A transition from state '{state.Name}' ({state.Id}) to '{transition.ToState.Id}' " +
					"is unreachable because the state has an unconditional transition.");
		}
	}

	public override void Visit(Diagram diagram)
	{
		var byId = diagram.States.ToDictionary(s => s.Id);
		var parentOf = BuildParentMap(diagram);

		HashSet<string> reachable = [];
		Queue<IState> queue = new();

		foreach (var state in diagram.States)
		{
			if (state is SimpleState { StateType: SimpleStateEnum.Initial } && reachable.Add(state.Id))
				queue.Enqueue(state);
		}

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();

			foreach (var transition in current.Transitions)
				Discover(transition.ToState.Id);

			if (current is CompoundState compound)
			{
				foreach (var sub in compound.SubStates)
					Discover(sub.Id);
			}

			if (parentOf.TryGetValue(current.Id, out var parentId))
				Discover(parentId);
		}

		foreach (var state in diagram.States)
		{
			if (!reachable.Contains(state.Id))
				AddError($"State '{state.Name}' ({state.Id}) is not reachable from an initial state.");
		}

		void Discover(string id)
		{
			if (reachable.Add(id) && byId.TryGetValue(id, out var next))
				queue.Enqueue(next);
		}
	}

	private static Dictionary<string, string> BuildParentMap(Diagram diagram)
	{
		Dictionary<string, string> parentOf = [];

		foreach (var state in diagram.States)
		{
			if (state is CompoundState compound)
			{
				foreach (var sub in compound.SubStates)
					parentOf[sub.Id] = compound.Id;
			}
		}

		return parentOf;
	}
}
