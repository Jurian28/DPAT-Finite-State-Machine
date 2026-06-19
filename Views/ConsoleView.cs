namespace DPAT_Finite_State_Machine.Views;

using Validators;

public abstract class ConsoleView : IView
{
	public string AskForFilePath()
	{
		Console.Write("Enter the absolute path to the FSM file: ");
		var input = Console.ReadLine()?.Trim() ?? string.Empty;
		return input.Trim('"', '\'');
	}

	public void ShowError(string message)
	{
		Console.WriteLine($"\n[ERROR] {message}\n");
	}

	public abstract void ShowDiagram(Models.Diagram diagram, string? title = null);

	public void ShowValidationReport(IEnumerable<ValidationVisitor> validators)
	{
		Console.WriteLine("\n========================================");
		Console.WriteLine("            VALIDATION REPORT");
		Console.WriteLine("========================================");

		var allValid = true;

		foreach (var validator in validators)
		{
			if (validator.IsValid)
			{
				Console.WriteLine($"[ OK ]  {validator.Name}");
				continue;
			}

			allValid = false;
			Console.WriteLine($"[FAIL]  {validator.Name}");

			foreach (var error in validator.Errors)
				Console.WriteLine($"          - {error}");
		}

		Console.WriteLine("----------------------------------------");
		Console.WriteLine(allValid
			? "Result: the state machine is VALID."
			: "Result: the state machine is INVALID.");
		Console.WriteLine("========================================\n");
	}
}
