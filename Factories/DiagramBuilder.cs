using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories;

public class DiagramBuilder {
	private readonly List<StateDefinition> _states = [];
	private readonly List<TransitionDefinition> _transitions = [];
	private readonly List<ActionDefinition> _actions = [];
	private readonly List<Trigger> _triggers = [];

	public DiagramBuilder AddState(StateDefinition state) {
		_states.Add(state);
		return this;
	}

	public DiagramBuilder AddTransition(TransitionDefinition transition) {
		_transitions.Add(transition);
		return this;
	}

	public DiagramBuilder AddAction(ActionDefinition action) {
		_actions.Add(action);
		return this;
	}

	public DiagramBuilder AddTrigger(Trigger trigger) {
		_triggers.Add(trigger);
		return this;
	}

	public Diagram Build() {
		throw new NotImplementedException();
		// return new Diagram(_states);
	}


	public void TestPrint()
	{
		Console.WriteLine("\n========================================");
		Console.WriteLine("           DIAGRAM DEBUG DUMP");
		Console.WriteLine("========================================");

		Console.WriteLine(
				$"States: {_states.Count}, " +
				$"Transitions: {_transitions.Count}, " +
				$"Actions: {_actions.Count}, " +
				$"Triggers: {_triggers.Count}");

		Console.WriteLine("\n========== STATES ==========");

		foreach (var state in _states)
		{
			Console.WriteLine(
					$"Id={state.Id}, " +
					$"Name={state.Name}, " +
					$"Type={state.StateType?.ToString() ?? "CompoundState"}, " +
					$"ParentId={state.ParentId ?? "None"}");
		}

		Console.WriteLine("\n========== ACTIONS ==========");

		foreach (var action in _actions)
		{
			Console.WriteLine(
					$"OwnerId={action.OwnerId}, " +
					$"Description={action.Description}, " +
					$"ActionType={action.ActionType}");
		}

		Console.WriteLine("\n========== TRIGGERS ==========");

		foreach (var trigger in _triggers)
		{
			Console.WriteLine(
					$"Id={trigger.Id}, " +
					$"Description={trigger.Description}");
		}

		Console.WriteLine("\n========== TRANSITIONS ==========");

		foreach (var transition in _transitions)
		{
			Console.WriteLine(
					$"Id={transition.Id}, " +
					$"Source={transition.SourceStateId}, " +
					$"Destination={transition.DestinationStateId}, " +
					$"Trigger={transition.TriggerId ?? "NONE"}, " +
					$"Condition={transition.GuardCondition}");
		}

		Console.WriteLine("========================================\n");
	}
}
