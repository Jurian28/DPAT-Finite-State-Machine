using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Models;

namespace DPAT_Finite_State_Machine.Factories;

public static class StateFactory {

	public static StateDefinition FromString(string line) {
		var parts = ParseString(line);

		return ConstructStateFromStringParts(parts);
	}

	private static string[] ParseString(string line) {
		var partsByQuote = line.Split('"', StringSplitOptions.TrimEntries);

		if (partsByQuote.Length < 3) {
			throw new Exception("Invalid code: state is missing a quoted name");
		}
		
		var firstParts = partsByQuote[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
		var name = partsByQuote[1];
		var lastParts = partsByQuote[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

		if (!lastParts.Remove(":")) {
			throw new Exception("Invalid code: missing ':' for state");
		}

		var allParts = firstParts;

		allParts.Add(name);
		allParts.AddRange(lastParts);

		allParts[^1] = allParts[^1].TrimEnd(';');

		return allParts.ToArray(); // [STATE, final, _, powered off, FINAL]
	}

	private static StateDefinition ConstructStateFromStringParts(string[] parts) {
		var id = parts[1];
		var parent = parts[2];
		var name = parts[3];
		var stateType = GetSimpleStateEnumFromString(parts[4]);

		if(parent == "_") parent = null;

		return new StateDefinition(id, name, stateType, parent);
	}

	private static SimpleStateEnum? GetSimpleStateEnumFromString(string stateType) {
		return stateType switch {
			"INITIAL" => SimpleStateEnum.Initial,
			"FINAL" => SimpleStateEnum.Final,
			"SIMPLE" => SimpleStateEnum.Simple,
			"COMPOUND" => null,
			_ => throw new Exception("Invalid code: invalid stateType: " + stateType),
		};
	}
}
