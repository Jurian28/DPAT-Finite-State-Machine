using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Models;

namespace DPAT_Finite_State_Machine.Factories;

public static class ActionFactory {

	public static ActionDefinition FromString(string line) {
		var parts = ParseString(line);

		return ConstructActionFromStringParts(parts);
	}

	private static string[] ParseString(string line) {
		var partsByQuote = line.Split('"', StringSplitOptions.TrimEntries);

		if (partsByQuote.Length < 3) {
			throw new Exception("Invalid code: action is missing a quoted description");
		}

		var firstParts = partsByQuote[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
		var description = partsByQuote[1];
		var lastParts = partsByQuote[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

		if (!lastParts.Remove(":")) {
			throw new Exception("Invalid code: missing ':' for action");
		}

		var allParts = firstParts;

		allParts.Add(description);
		allParts.AddRange(lastParts);

		allParts[^1] = allParts[^1].TrimEnd(';');

		return allParts.ToArray();
	}

	private static ActionDefinition ConstructActionFromStringParts(string[] parts) {
		var ownerId = parts[1];
		var description = parts[2];
		var actionType = GetActionTypeEnumFromString(parts[3]);

		return new ActionDefinition(ownerId, description, actionType);
	}

	private static ActionTypeEnum GetActionTypeEnumFromString(string actionType) {
		return actionType switch {
			"ENTRY_ACTION" => ActionTypeEnum.EntryAction,
			"DO_ACTION" => ActionTypeEnum.DoAction,
			"EXIT_ACTION" => ActionTypeEnum.ExitAction,
			"TRANSITION_ACTION" => ActionTypeEnum.TransitionAction,

			_ => throw new Exception("Invalid code: invalid actionType: " + actionType),
		};
	}
}
