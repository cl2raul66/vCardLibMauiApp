using CommunityToolkit.Mvvm.ComponentModel;
using vCardLib.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using vCardLibMauiApp.Views;
using CommunityToolkit.Mvvm.Messaging;
using vCardLibMauiApp.Services;

namespace vCardLibMauiApp.ViewModels;

public partial class PgContactosViewModel : ObservableRecipient
{
    readonly IContactosService contactosServ;

    public PgContactosViewModel(IContactosService contactosService)
    {
        IsActive = true;
        contactosServ = contactosService;
        if (contactosServ.Existe)
        {
            Vcards = new(contactosServ.GetallLimit(15));
        }
    }

    [ObservableProperty]
    ObservableCollection<vCard>? vcards;

    [ObservableProperty]
    vCard? selectedVcard;

    [RelayCommand]
    async Task IrNuevo()
    {
        await Shell.Current.GoToAsync(nameof(PgNuevoEditar), true);
    }

    [RelayCommand]
    async Task IrEditar()
    {
        await Shell.Current.GoToAsync(nameof(PgNuevoEditar), true, new Dictionary<string, object>() { { "contacto", SelectedVcard! } });
    }

    [RelayCommand]
    async Task Eliminar()
    {
        bool result = contactosServ.Delete(SelectedVcard!.Uid!);
        if (result)
        {
            Vcards!.Remove(SelectedVcard!);
            SelectedVcard = null;
        }
        await Task.CompletedTask;
    }

    [RelayCommand]
    async Task IrImportar()
    {
        SelectedVcard = null;
        await Task.CompletedTask;
    }

    [RelayCommand]
    async Task IrExportar()
    {
        SelectedVcard = null;
        await Task.CompletedTask;
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        //nuevo
        WeakReferenceMessenger.Default.Register<PgContactosViewModel, vCard, string>(this, "nuevo", (r, m) =>
        {
            Vcards ??= [];
            bool result = contactosServ.Insert(m);
            if (result)
            {
                Vcards.Insert(0, m);
            }
            SelectedVcard = null;
        });
        //editar
        WeakReferenceMessenger.Default.Register<PgContactosViewModel, vCard, string>(this, "editar", (r, m) =>
        {
            bool result = contactosServ.Update(m);
            if (result)
            {
                int idx = Vcards!.IndexOf(SelectedVcard!);
                if (idx > -1)
                {
                    r.Vcards![idx] = m;
                }
                SelectedVcard = null;
            }
        });
    }
}
