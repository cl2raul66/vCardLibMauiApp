using vCardLibMauiApp.Views;

namespace vCardLibMauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(PgNuevoEditar), typeof(PgNuevoEditar));
    }
}
