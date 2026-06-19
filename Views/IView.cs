namespace DPAT_Finite_State_Machine.Views;

using Models;
using Validators;

public interface IView
{
	string AskForFilePath();
	void ShowError(string message);
	void ShowDiagram(Diagram diagram, string? title = null);
	void ShowValidationReport(IEnumerable<ValidationVisitor> validators);
}
