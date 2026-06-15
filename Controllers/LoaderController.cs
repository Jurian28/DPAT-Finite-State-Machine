using DPAT_Finite_State_Machine.Factories;
using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Controllers;

public class LoaderController
{

	public Diagram ParseFile(string filepath) {
		DiagramBuilder builder = new();

		foreach (string line in File.ReadLines(filepath)) {
			if (string.IsNullOrWhiteSpace(line)) continue; 
			if (line.TrimStart()[0] == '#') continue;

			ParseLine(line, builder);
		}
		return builder.Build();
	}

    private void ParseLine(string line, DiagramBuilder builder) {
		string firstWord = line.Split(" ")[0];

		switch (firstWord) {
			case "STATE": 
				StateDefinition stateDefinition = StateFactory.FromString(line); 
				builder.AddState(stateDefinition);
				break;
			case "TRIGGER":
				Trigger trigger = TriggerFactory.FromString(line);
				builder.AddTrigger(trigger);
				break;
			case "ACTION":
				ActionDefinition action = ActionFactory.FromString(line);
				builder.AddAction(action);
				break;
			case "TRANSITION": 
				TransitionDefinition transition = TransitionFactory.FromString(line);
				builder.AddTransition(transition);
				break;

			default:
				throw new Exception("Invalid file loaded");
		}
    }
}
