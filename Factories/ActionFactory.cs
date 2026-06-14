using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories;

public class ActionFactory {

	public static ActionDefinition FromString(string line) {
		string[] parts = ParseString(line);

		return ConstructActionFromStringParts(parts);
	}

	private static string[] ParseString(string line) {
		List<string> partsByQuote = line.Split('"', StringSplitOptions.TrimEntries).ToList();

		List<string> firstParts = partsByQuote[0].Split(" ").ToList();
		List<string> lastParts = partsByQuote[2].Split(" ").ToList();

		if (!lastParts.Remove(":")) {
			throw new Exception("Invalid code: missing ':' for action");
		} 

		List<string> allParts = firstParts;

		allParts.AddRange(partsByQuote[1]);
		allParts.AddRange(lastParts);

		allParts[^1] = allParts[^1].TrimEnd(';');

		return allParts.ToArray(); 
	}

	private static ActionDefinition ConstructActionFromStringParts(string[] parts) {
		string ownerId = parts[1];
		string description = parts[2];
		ActionTypeEnum actionType = GetActionTypeEnumFromString(parts[3]);

		return new ActionDefinition(ownerId, description, actionType);
	}

	private static ActionTypeEnum GetActionTypeEnumFromString(string actionType) {
		return actionType switch {
			"ENTRY_ACTION" => ActionTypeEnum.Entry_action,
			"DO_ACTION" => ActionTypeEnum.Do_action,
			"EXIT_ACTION" => ActionTypeEnum.Exit_action,
			"TRANSITION_ACTION" => ActionTypeEnum.Transition_action,

			_ => throw new Exception("Invalid code: invalid actionType: " + actionType),
		};
	}
}
