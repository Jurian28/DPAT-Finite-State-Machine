using DPAT_Finite_State_Machine.Factories;
using DPAT_Finite_State_Machine.Models;

namespace DPAT_Finite_State_Machine.Controllers;

public class LoaderController
{

	public Diagram ParseFile(string filepath) {
		DiagramBuilder builder = new();

		foreach (var line in File.ReadLines(filepath)) {
			if (string.IsNullOrWhiteSpace(line)) continue;
			if (line.TrimStart()[0] == '#') continue;

			ParseLine(line, builder);
		}
		return builder.Build();
	}

    private void ParseLine(string line, DiagramBuilder builder) {
		var firstWord = line.Split(" ")[0];

		switch (firstWord) {
			case "STATE":
				var stateDefinition = StateFactory.FromString(line);
				builder.AddState(stateDefinition);
				break;
			case "TRIGGER":
				var trigger = TriggerFactory.FromString(line);
				builder.AddTrigger(trigger);
				break;
			case "ACTION":
				var action = ActionFactory.FromString(line);
				builder.AddAction(action);
				break;
			case "TRANSITION":
				var transition = TransitionFactory.FromString(line);
				builder.AddTransition(transition);
				break;

			default:
				throw new Exception("Invalid file loaded");
		}
    }
}
