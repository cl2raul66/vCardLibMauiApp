using vCardLibMauiApp.ViewModels;

namespace vCardLibMauiApp.Views;

public partial class PgContactos : ContentPage
{
	public PgContactos(PgContactosViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}