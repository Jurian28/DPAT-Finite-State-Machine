using DPAT_Finite_State_Machine.Model;

namespace DPAT_Finite_State_Machine.Controllers;

public class MainController
{
	public void Start()
	{
		string filepath = "FSM1.txt";
		LoaderController loader = new();

		Diagram diagram = loader.ParseFile(filepath);

	}
}
