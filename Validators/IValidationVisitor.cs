namespace DPAT_Finite_State_Machine.Validators;

using DPAT_Finite_State_Machine.Model;

public interface IValidationVisitor
{
	void Visit(SimpleState simpleState);
	void Visit(CompoundState compoundState);
	void Visit(Transition transition);
	void Visit(Diagram diagram);
}
