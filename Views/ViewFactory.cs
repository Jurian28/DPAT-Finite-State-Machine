namespace DPAT_Finite_State_Machine.Views;

public static class ViewFactory
{
	public static IView Create(ViewMode mode, string outputDirectory) => mode switch
	{
		ViewMode.Graphical => new GraphicalView(outputDirectory),
		_ => new TextualView(),
	};
}
