using DPAT_Finite_State_Machine.Models;

namespace DPAT_Finite_State_Machine.Factories;

public static class TriggerFactory {

	public static Trigger FromString(string line) {
		var parts = ParseString(line);

		return ConstructTriggerFromStringParts(parts);
	}

    private static string[] ParseString(string line) {
		var partsByQuote = line.Split('"', StringSplitOptions.TrimEntries);

		if (partsByQuote.Length < 2) {
			throw new Exception("Invalid code: trigger is missing a quoted description");
		}

		var firstParts = partsByQuote[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
		var description = partsByQuote[1];

		var allParts = firstParts;
		allParts.Add(description);

		return allParts.ToArray(); // [TRIGGER, power_on, turn power on]
	}

	private static Trigger ConstructTriggerFromStringParts(string[] parts) {
		var id = parts[1];
		var description = parts[2];
		return new Trigger(id, description);
	}
}
