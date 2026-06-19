namespace DPAT_Finite_State_Machine.Validators;

using Models;

public abstract class ValidationVisitor : IValidationVisitor
{
	private readonly List<string> _errors = [];

	// Human readable name, shown by the View in the validation report.
	public abstract string Name { get; }

	public IReadOnlyList<string> Errors => _errors;

	public bool IsValid => _errors.Count == 0;

	protected void AddError(string message) => _errors.Add(message);

	public virtual void Visit(SimpleState simpleState) { }

	public virtual void Visit(CompoundState compoundState) { }

	public virtual void Visit(Transition transition) { }

	public virtual void Visit(Diagram diagram) { }
}
