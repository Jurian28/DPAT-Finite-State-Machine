using DPAT_Finite_State_Machine.Factories.Definitions;
using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories;

public class StateFactory {

	public static StateDefinition FromString(string line) {
		string[] parts = ParseString(line);

		return ConstructStateFromStringParts(parts);
	}

	private static string[] ParseString(string line) {
		List<string> partsByQuote = line.Split('"', StringSplitOptions.TrimEntries).ToList();

		List<string> firstParts = partsByQuote[0].Split(" ").ToList();
		List<string> lastParts = partsByQuote[2].Split(" ").ToList();

		if (!lastParts.Remove(":")) {
			throw new Exception("Invalid code: missing ':' for state");
		} 

		List<string> allParts = firstParts;

		allParts.AddRange(partsByQuote[1]);
		allParts.AddRange(lastParts);

		allParts[^1] = allParts[^1].TrimEnd(';');

		return allParts.ToArray(); // [STATE, final, _, powered off, FINAL]
	}

	private static StateDefinition ConstructStateFromStringParts(string[] parts) {
		string id = parts[1];
		string? parent = parts[2];
		string name = parts[3];
		SimpleStateEnum? stateType = GetSimpleStateEnumFromString(parts[4]);

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
