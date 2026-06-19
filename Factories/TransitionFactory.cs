using DPAT_Finite_State_Machine.Factories.Definitions;

namespace DPAT_Finite_State_Machine.Factories;

public static class TransitionFactory {

	public static TransitionDefinition FromString(string line) {
		var parts = ParseString(line);

		return ConstructTransitionFromStringParts(parts);
	}

    private static string[] ParseString(string line) {

		var quoteParts = line.Split('"', StringSplitOptions.TrimEntries);
		var beforeQuote = quoteParts[0].TrimEnd(';', ' ');
		var guardCondition = quoteParts.Length >= 2 ? quoteParts[1] : string.Empty;
		var arrowParts = beforeQuote.Split(" -> ");

		if (arrowParts.Length != 2) {
			throw new Exception("Invalid code: transition is missing '->'");
		}

		var firstParts = arrowParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
		var secondParts = arrowParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

		var parts = firstParts;
		parts.AddRange(secondParts);
		parts.Add(guardCondition);

		return parts.ToArray();
		// [TRANSITION, identifier, source_state_identifier, destination_state_identifier, [trigger_identifier], guard_condition]
	}

	private static TransitionDefinition ConstructTransitionFromStringParts(string[] parts) {
		var id = parts[1];
		var src = parts[2];
		var desc = parts[3];
		string? trigger;
		string guardCondition;

		if (parts.Length == 6) {
			trigger = parts[4];
			guardCondition = parts[5];
		} else if(parts.Length == 5) {
			trigger = null;
			guardCondition = parts[4];
		} else {
			throw new Exception("Transition definition has wrong amount of parts");
		}

		return new TransitionDefinition(id, src, desc, trigger, guardCondition);
	}
}
