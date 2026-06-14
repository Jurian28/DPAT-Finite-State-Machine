using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Factories;

public class TriggerFactory {

	public static Trigger FromString(string line) {
		string[] parts = ParseString(line);

		return ConstructTriggerFromStringParts(parts);
	}

    private static string[] ParseString(string line) {
		List<string> partsByQuote = line.Split('"', StringSplitOptions.TrimEntries).ToList();

		List<string> firstParts = partsByQuote[0].Split(" ").ToList();

		List<string> allParts = firstParts;
		allParts.AddRange(partsByQuote[1]);

		return allParts.ToArray(); // TRIGGER, power_on, turn power on
	}

	private static Trigger ConstructTriggerFromStringParts(string[] parts)
	{
		string id = parts[1];
		string description = parts[2];
		return new Trigger(id, description);
	}
}
