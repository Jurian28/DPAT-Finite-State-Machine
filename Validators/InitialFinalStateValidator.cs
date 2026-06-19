namespace DPAT_Finite_State_Machine.Validators;

using Models;

public class InitialFinalStateValidator : ValidationVisitor
{
	public override string Name => "Initial / final state";

	private int _initialCount;
	private int _finalCount;

	public override void Visit(SimpleState simpleState)
	{
		if (simpleState.StateType == SimpleStateEnum.Initial)
		{
			_initialCount++;
		}
		else if (simpleState.StateType == SimpleStateEnum.Final)
		{
			_finalCount++;

			// A final state is an end state: it may not have outgoing transitions.
			if (simpleState.Transitions.Count > 0)
				AddError(
					$"Final state '{simpleState.Name}' ({simpleState.Id}) has outgoing transitions; transitions from a final state are not allowed.");
		}
	}

	public override void Visit(Diagram diagram)
	{
		if (_initialCount == 0)
			AddError("The diagram has no initial state.");
		else if (_initialCount > 1)
			AddError($"The diagram has {_initialCount} initial states, expected exactly one.");

		if (_finalCount == 0)
			AddError("The diagram has no final state.");
	}
}
