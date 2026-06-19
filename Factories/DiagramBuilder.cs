using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Models;
using Action = DPAT_Finite_State_Machine.Models.Action;

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
		var states = ConstructStates();
		var transitions = AddTransitionsBetweenStates(states);

		return new Diagram(states, transitions);
	}
	
	private List<IState> ConstructStates() {
		Dictionary<IState, string?> states = []; // state, parentId

		foreach (var stateDefinition in _states) {

			var actions = GetActionsByOwnerId(stateDefinition.Id);

			if(stateDefinition.StateType is SimpleStateEnum stateType) { // if stateType == null its a compound state
				SimpleState state = new(stateDefinition.Id, stateDefinition.Name, stateType, actions);
				states.Add(state, stateDefinition.ParentId);
			} else {
				CompoundState state = new(stateDefinition.Id, stateDefinition.Name, actions);
				states.Add(state, stateDefinition.ParentId);
			}
		}

		return LinkStates(states);
	}

	private List<IState> LinkStates(Dictionary<IState, string?> states) {
		foreach (var state in states.Keys) {
			if (state is CompoundState compoundState) {
				var subStates = states
					.Where(kvp => kvp.Value == compoundState.Id)
					.Select(kvp => kvp.Key)
					.ToList(); // where parentId == compoundState.Id

				compoundState.SetSubStates(subStates);
			}
		}

		return states.Keys.ToList();
	}

	// A state may own several actions (entry / do / exit).
	private List<Action> GetActionsByOwnerId(string ownerId) {
		return _actions
			.Where(a => a.OwnerId == ownerId)
			.Select(a => new Action(a.Description, a.ActionType))
			.ToList();
	}
	
	private Action? GetActionByOwnerId(string ownerId) {
		var actionDefinitions = _actions.Where(a => a.OwnerId == ownerId).ToArray();

		if (actionDefinitions.Length == 0) return null; // no action;
		if (actionDefinitions.Length > 1) throw new Exception("Too many actions on one transition");

		return actionDefinitions
			.Select(a => new Action(a.Description, a.ActionType))
			.First();
	}

	private List<Transition> AddTransitionsBetweenStates(List<IState> states) {
		List<Transition> transitions = [];

		foreach (var transitionDefinition in _transitions) {
			var action = GetActionByOwnerId(transitionDefinition.Id);
			var trigger = GetTriggerById(transitionDefinition.TriggerId);
			var source = states.First(s => s.Id == transitionDefinition.SourceStateId);
			var destination = states.First(s => s.Id == transitionDefinition.DestinationStateId);

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
}
