namespace DPAT_Finite_State_Machine.Views;

using Models;
using Action = Models.Action;

public class TextualView : ConsoleView
{
	private const int BorderWidth = 56;

	public override void ShowDiagram(Diagram diagram, string? title = null)
	{
		string hashes = new('#', BorderWidth);
		Console.WriteLine();
		Console.WriteLine(hashes);
		Console.WriteLine($"# Diagram: {title ?? "Finite State Machine"}");
		Console.WriteLine(hashes);
		Console.WriteLine();

		foreach (var state in TopLevelStates(diagram))
		{
			RenderState(state, 0);
			Console.WriteLine();
		}
	}
	
	private static IEnumerable<IState> TopLevelStates(Diagram diagram)
	{
		var subStateIds = diagram.States
			.OfType<CompoundState>()
			.SelectMany(c => c.SubStates)
			.Select(s => s.Id)
			.ToHashSet();

		return diagram.States.Where(s => !subStateIds.Contains(s.Id));
	}
	
	private void RenderState(IState state, int depth)
	{
		switch (state)
		{
			case SimpleState { StateType: SimpleStateEnum.Initial } initial:
				WriteLine(depth, $"O   Initial state: {initial.Name}");
				RenderTransitions(initial, depth);
				break;

			case SimpleState { StateType: SimpleStateEnum.Final } final:
				WriteLine(depth, $"(O) Final state: {final.Name}");
				break;

			case SimpleState simple:
				RenderBox(simple.Name, simple.Actions, depth, '-');
				RenderTransitions(simple, depth);
				break;

			case CompoundState compound:
				RenderCompound(compound, depth);
				break;
		}
	}

	private void RenderCompound(CompoundState compound, int depth)
	{
		var heavy = Border(depth, '=');
		Console.WriteLine(heavy);
		WriteLine(depth, $"|| Compound state: {compound.Name}");
		Console.WriteLine(Border(depth, '-'));
		RenderActions(compound.Actions, depth);

		Console.WriteLine();
		foreach (var subState in compound.SubStates)
		{
			RenderState(subState, depth + 1);
			Console.WriteLine();
		}

		Console.WriteLine(heavy);
		RenderTransitions(compound, depth);
	}

	// A boxed state: name on top, the state actions (entry/do/exit) below.
	private void RenderBox(string name, List<Action> actions, int depth, char edge)
	{
		var border = Border(depth, edge);
		Console.WriteLine(border);
		WriteLine(depth, $"| {name}");
		Console.WriteLine(border);

		if (actions.Count <= 0) return;
		
		RenderActions(actions, depth);
		Console.WriteLine(border);
	}

	private static void RenderActions(IEnumerable<Action> actions, int depth)
	{
		foreach (var action in actions)
			WriteLine(depth, $"| {ActionLabel(action.ActionType)} / {action.Description}");
	}

	private static void RenderTransitions(IState state, int depth)
	{
		foreach (var transition in state.Transitions)
			WriteLine(depth, $"---{TransitionLabel(transition)}--> {transition.ToState.Name}");
	}

	// Builds the "trigger [guard] / effect" label shown on a transition arrow.
	private static string TransitionLabel(Transition transition)
	{
		List<string> head = [];
		if (!string.IsNullOrWhiteSpace(transition.Trigger?.Description))
			head.Add(transition.Trigger!.Description);
		if (!string.IsNullOrWhiteSpace(transition.Condition))
			head.Add($"[{transition.Condition}]");

		var label = string.Join(" ", head);
		if (label.Length == 0)
			label = "(automatic)";

		if (transition.Action is { } effect)
			label += $" / {effect.Description}";

		return label;
	}

	private static string ActionLabel(ActionTypeEnum type) => type switch
	{
		ActionTypeEnum.EntryAction => "On Entry",
		ActionTypeEnum.DoAction => "Do",
		ActionTypeEnum.ExitAction => "On Exit",
		ActionTypeEnum.TransitionAction => "Effect",
		_ => type.ToString(),
	};

	private static string Indent(int depth) => new(' ', depth * 2);

	private static string Border(int depth, char edge) =>
		Indent(depth) + new string(edge, BorderWidth);

	private static void WriteLine(int depth, string text) =>
		Console.WriteLine(Indent(depth) + text);
}
