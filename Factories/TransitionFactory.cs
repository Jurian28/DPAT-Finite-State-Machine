using System.Text.RegularExpressions;
using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories;

public class TransitionFactory {

	public static TransitionDefinition FromString(string line) {
		string[] parts = ParseString(line);

		return ConstructTransitionFromStringParts(parts);
	}

    private static string[] ParseString(string line) {

		var quoteParts = line.Split('"', StringSplitOptions.TrimEntries).ToList();
		var arrowParts = quoteParts[0].Split(" -> ").ToList();

		var firstParts = arrowParts[0].Split(" ").ToList();
		var secondParts = arrowParts[1].Split(" ").ToList();
		var lastParts = quoteParts[1];

		var parts = firstParts;
		parts.AddRange(secondParts);
		parts.AddRange(lastParts);

		return parts.ToArray(); // TRANSITION, identifier, source_state_identifier, destination_state_identifier, [trigger_identifier], guard_condition
	}

	private static TransitionDefinition ConstructTransitionFromStringParts(string[] parts) {
		string id = parts[1];
		string src = parts[2];
		string desc = parts[3];
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
