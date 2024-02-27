using vCardLibMauiApp.ViewModels;

namespace vCardLibMauiApp.Views;

public partial class PgNuevoEditar : ContentPage
{
	public PgNuevoEditar(PgNuevoEditarViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}