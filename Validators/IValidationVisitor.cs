namespace DPAT_Finite_State_Machine.Validators;

using Models;

public interface IValidationVisitor
{
	void Visit(SimpleState simpleState);
	void Visit(CompoundState compoundState);
	void Visit(Transition transition);
	void Visit(Diagram diagram);
}
