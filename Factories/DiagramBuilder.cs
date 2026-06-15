using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;
using Action = DPAT_Finite_State_Machine.Model.Action;

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
		List<IState> states = ConstructStates();

		List<Transition> transitions = AddTransitionsBetweenStates(states);


		TestPrintStates(states);

		return new Diagram(states, transitions);


		// throw new NotImplementedException();
		// return new Diagram(_states);
	}


    private List<IState> ConstructStates() {
		Dictionary<IState, string?> states = []; // state, parentId

		foreach (StateDefinition stateDefinition in _states) {

            Action? action = GetActionByOwnerId(stateDefinition.Id);

			if(stateDefinition.StateType is SimpleStateEnum stateType) { // if stateType == null its a compound state
				SimpleState state = new(stateDefinition.Id, stateDefinition.Name, stateType, action);
				states.Add(state, stateDefinition.ParentId);
			} else {
				CompoundState state = new(stateDefinition.Id, stateDefinition.Name, action);
				states.Add(state, stateDefinition.ParentId);
			} 
		}

		return LinkStates(states);
	}

	private List<IState> LinkStates(Dictionary<IState, string?> states) {
		foreach (IState state in states.Keys) {
			if (state is CompoundState compoundState) {
				List<IState> subStates = states
					.Where(kvp => kvp.Value == compoundState.Id)
					.Select(kvp => kvp.Key)
					.ToList(); // where parentId == compoundState.Id

				compoundState.SetSubStates(subStates);
			}
		}

		return states.Keys.ToList();
	}

	private Action? GetActionByOwnerId(string ownerId) {
		ActionDefinition[] actionDefinitions = _actions.Where(a => a.OwnerId == ownerId).ToArray();

		if (actionDefinitions.Length == 0) return null; // no action;
		if (actionDefinitions.Length != 1) throw new Exception("To many actions on one owner");

		return actionDefinitions
			.Select(a => new Action(a.Description, a.ActionType))
			.First();
	}

	private List<Transition> AddTransitionsBetweenStates(List<IState> states) {
		List<Transition> transitions = [];

		foreach (TransitionDefinition transitionDefinition in _transitions) {
			Action? action = GetActionByOwnerId(transitionDefinition.Id);
			Trigger? trigger = GetTriggerById(transitionDefinition.TriggerId);
			IState source = states.First(s => s.Id == transitionDefinition.SourceStateId);
			IState destination = states.First(s => s.Id == transitionDefinition.DestinationStateId);

			Transition transition = new(
				action, 
				trigger, 
				destination, 
				transitionDefinition.GuardCondition
			);
			transitions.Add(transition);

			source.AddTransition(transition);
		}

		return transitions;
	}

    private Trigger? GetTriggerById(string? triggerId) {
		return _triggers.FirstOrDefault(t => t.Id == triggerId);
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

	public static void TestPrintStates(IEnumerable<IState> states)
	{
		foreach (var state in states)
		{
			TestPrintState(state, 0);
			Console.WriteLine("==================================================\n");
		}
	}

	private static void TestPrintState(IState state, int indent)
	{
		string padding = new(' ', indent * 2);

		switch (state)
		{
			case SimpleState simple:
				Console.WriteLine(
						$"{padding}SimpleState: {simple.Name} ({simple.Id}) [{simple.StateType}]");
				Console.WriteLine(
						$"{padding}  Action: {simple.Action?.ActionType} - {simple.Action?.Description ?? "None"}");

				TestPrintTransitions(simple.Transitions, indent + 1);
				break;

			case CompoundState compound:
				Console.WriteLine(
						$"{padding}CompoundState: {compound.Name} ({compound.Id})");
				Console.WriteLine(
						$"{padding}  Action: {compound.Action?.ActionType} - {compound.Action?.Description ?? "None"}");

				TestPrintTransitions(compound.Transitions, indent + 1);

				if (compound.SubStates.Count > 0)
				{
					Console.WriteLine($"{padding}  SubStates:");

					foreach (var subState in compound.SubStates)
					{
						TestPrintState(subState, indent + 2);
					}
				}

				break;
		}
	}

	private static void TestPrintTransitions(
			IEnumerable<Transition> transitions,
			int indent)
	{
		string padding = new(' ', indent * 2);

		foreach (var transition in transitions)
		{
			Console.WriteLine($"{padding}Transition:");

			Console.WriteLine(
					$"{padding}  Trigger: {transition.Trigger?.Id} - {transition.Trigger?.Description ?? "None"}");

			Console.WriteLine(
					$"{padding}  Action: {transition.Action?.ActionType} - {transition.Action?.Description ?? "None"}");

			Console.WriteLine(
					$"{padding}  Condition: {transition.Condition}");

			Console.WriteLine(
					$"{padding}  To State: {transition.ToState.Name} ({transition.ToState.Id})");
		}
	}
}
