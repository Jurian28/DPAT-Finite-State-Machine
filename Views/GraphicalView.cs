namespace DPAT_Finite_State_Machine.Views;

using System.Diagnostics;
using System.Text;
using Models;
using Action = Models.Action;

public class GraphicalView(string outputDirectory) : ConsoleView
{
	public override void ShowDiagram(Diagram diagram, string? title = null)
	{
		var name = string.IsNullOrWhiteSpace(title) ? "fsm" : title!;

		// Turn the model into PlantUML text and write the .puml file.
		var puml = BuildPlantUml(diagram, name);
		var pumlPath = Path.Combine(outputDirectory, $"{name}.puml");
		File.WriteAllText(pumlPath, puml);

		Console.WriteLine($"\n[GRAPHIC] PlantUML source written to: {pumlPath}");

		// Render the .puml into an image (only works when PlantUML is installed).
		var imagePath = RenderImage(pumlPath);
		if (imagePath is null)
		{
			Console.WriteLine("[GRAPHIC] Could not run 'plantuml'. Open the .puml file manually or install PlantUML.\n");
			return;
		}

		Console.WriteLine($"[GRAPHIC] Diagram image written to: {imagePath}");
		TryOpen(imagePath);
		Console.WriteLine();
	}
	
	// Produces the PlantUML state-diagram source for the whole FSM.
	private static string BuildPlantUml(Diagram diagram, string name)
	{
		StringBuilder sb = new();
		sb.AppendLine("@startuml");
		sb.AppendLine($"title {name}");
		sb.AppendLine("hide empty description");
		sb.AppendLine();

		// State declarations (compound states nest their sub-states).
		foreach (var state in TopLevelStates(diagram))
			AppendState(sb, state, 0);

		sb.AppendLine();

		// Every transition, declared flat by alias (aliases are global in PlantUML).
		foreach (var state in diagram.States)
			foreach (var transition in state.Transitions)
				sb.AppendLine($"{Alias(state)} --> {Alias(transition.ToState)} : {TransitionLabel(transition)}");

		sb.AppendLine("@enduml");
		return sb.ToString();
	}

	private static void AppendState(StringBuilder sb, IState state, int depth)
	{
		string pad = new(' ', depth * 2);

		switch (state)
		{
			// Initial/final states are PlantUML pseudo-states ([*]); no declaration.
			case SimpleState { StateType: SimpleStateEnum.Initial }:
			case SimpleState { StateType: SimpleStateEnum.Final }:
				return;

			case SimpleState simple:
				sb.AppendLine($"{pad}state \"{simple.Name}\" as {Alias(simple)}");
				AppendActions(sb, Alias(simple), simple.Actions, pad);
				break;

			case CompoundState compound:
				sb.AppendLine($"{pad}state \"{compound.Name}\" as {Alias(compound)} {{");
				foreach (var sub in compound.SubStates)
					AppendState(sb, sub, depth + 1);
				sb.AppendLine($"{pad}}}");
				AppendActions(sb, Alias(compound), compound.Actions, pad);
				break;
		}
	}

	private static void AppendActions(StringBuilder sb, string alias, IEnumerable<Action> actions, string pad)
	{
		foreach (var action in actions)
			sb.AppendLine($"{pad}{alias} : {ActionLabel(action.ActionType)} / {Escape(action.Description)}");
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

	// Initial/final map to the [*] pseudo-state; everything else to a safe alias.
	private static string Alias(IState state)
	{
		if (state is SimpleState { StateType: SimpleStateEnum.Initial or SimpleStateEnum.Final })
			return "[*]";

		StringBuilder sb = new("S_");
		foreach (var c in state.Id)
			sb.Append(char.IsLetterOrDigit(c) ? c : '_');
		return sb.ToString();
	}

	private static string TransitionLabel(Transition transition)
	{
		List<string> head = [];
		if (!string.IsNullOrWhiteSpace(transition.Trigger?.Description))
			head.Add(transition.Trigger!.Description);
		if (!string.IsNullOrWhiteSpace(transition.Condition))
			head.Add($"[{transition.Condition}]");

		var label = string.Join(" ", head);
		if (transition.Action is { } effect)
			label += $" / {effect.Description}";

		return Escape(label);
	}

	private static string ActionLabel(ActionTypeEnum type) => type switch
	{
		ActionTypeEnum.EntryAction => "entry",
		ActionTypeEnum.DoAction => "do",
		ActionTypeEnum.ExitAction => "exit",
		ActionTypeEnum.TransitionAction => "effect",
		_ => type.ToString(),
	};

	// Keep labels on a single line; PlantUML treats a bare newline as a break.
	private static string Escape(string text) =>
		text.Replace("\r", " ").Replace("\n", " ");

	// Runs `plantuml -tpng <file>`; returns the image path on success, else null.
	private static string? RenderImage(string pumlPath)
	{
		try
		{
			ProcessStartInfo info = new()
			{
				FileName = "plantuml",
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
			};
			info.ArgumentList.Add("-tpng");
			info.ArgumentList.Add(pumlPath);

			// No plantuml on PATH -> Start throws / returns null -> caught below.
			using var process = Process.Start(info);
			if (process is null) return null;

			process.WaitForExit();
			if (process.ExitCode != 0) return null;

			// PlantUML writes the .png next to the .puml, with the same base name.
			var imagePath = Path.ChangeExtension(pumlPath, ".png");
			return File.Exists(imagePath) ? imagePath : null;
		}
		catch
		{
			return null;
		}
	}
	
	private static void TryOpen(string imagePath)
	{
		try
		{
			Process.Start(new ProcessStartInfo(imagePath) { UseShellExecute = true });
		}
		catch (Exception ex)
		{
			// Viewer is optional, but report why so it isn't a silent no-op.
			Console.WriteLine($"[GRAPHIC] Could not open the image automatically: {ex.Message}");
		}
	}
}
