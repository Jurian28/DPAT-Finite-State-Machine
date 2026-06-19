using DPAT_Finite_State_Machine.Models;
using DPAT_Finite_State_Machine.Validators;
using DPAT_Finite_State_Machine.Views;

namespace DPAT_Finite_State_Machine.Controllers;

public class MainController
{
	public static void Start(string[] args)
	{
		var view = ViewFactory.Create(ViewMode.Textual, Environment.CurrentDirectory);

		var diagram = LoadDiagram(args, view, out var diagramName, out var outputDir);

		// Show the parsed FSM textually and as a rendered image
		// (graphical view) before reporting the validation results.
		view.ShowDiagram(diagram, diagramName);
		ViewFactory.Create(ViewMode.Graphical, outputDir).ShowDiagram(diagram, diagramName);

		List<ValidationVisitor> validators =
		[
			new InitialFinalStateValidator(),
			new TransitionTargetValidator(),
			new ReachableValidator(),
			new DeterminismValidator(),
		];
		
		foreach (var validator in validators)
			diagram.Accept(validator);

		view.ShowValidationReport(validators);
	}
	
	private static Diagram LoadDiagram(string[] args, IView view, out string diagramName, out string outputDir)
	{
		LoaderController loader = new();
		var filepath = args.Length > 0 ? args[0] : null;

		while (true)
		{
			if (string.IsNullOrWhiteSpace(filepath))
			{
				filepath = view.AskForFilePath();
				continue;
			}

			if (!File.Exists(filepath))
			{
				view.ShowError($"File not found: '{filepath}'. Please try again.");
				filepath = null;
				continue;
			}

			try
			{
				diagramName = Path.GetFileNameWithoutExtension(filepath);
				outputDir = Path.GetDirectoryName(Path.GetFullPath(filepath)) ?? Environment.CurrentDirectory;
				return loader.ParseFile(filepath);
			}
			catch (Exception ex)
			{
				view.ShowError($"Could not read FSM file: {ex.Message} Please try again.");
				filepath = null;
			}
		}
	}
}
