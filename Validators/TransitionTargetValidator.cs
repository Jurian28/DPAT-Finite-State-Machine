namespace DPAT_Finite_State_Machine.Validators;

using Models;

public class TransitionTargetValidator : ValidationVisitor
{
	public override string Name => "Transition target";

	private readonly HashSet<string> _stateIds = [];

	public override void Visit(SimpleState simpleState) => _stateIds.Add(simpleState.Id);

	public override void Visit(CompoundState compoundState) => _stateIds.Add(compoundState.Id);

	public override void Visit(Transition transition)
	{
		var target = transition.ToState;

		if (!_stateIds.Contains(target.Id))
		{
			AddError($"A transition targets unknown state '{target.Id}'.");
			return;
		}
		
		switch (target)
		{
			case CompoundState:
				AddError($"A transition ends at compound state '{target.Name}' ({target.Id}); transitions must end at a simple state.");
				break;
			case SimpleState { StateType: SimpleStateEnum.Initial }:
				AddError($"A transition targets initial state '{target.Name}' ({target.Id}); transitions into an initial state are not allowed.");
				break;
		}
	}
}
