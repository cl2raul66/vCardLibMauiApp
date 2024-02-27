using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using vCardLib.Models;

namespace vCardLibMauiApp.ViewModels;

[QueryProperty(nameof(Contacto), "contacto")]
public partial class PgNuevoEditarViewModel : ObservableValidator
{
    [ObservableProperty]
    string titulo = "Nuevo";

    [ObservableProperty]
    vCard? contacto;

    [ObservableProperty]
    bool vererror;

    [ObservableProperty]
    [Required]
    [MinLength(3)]
    string? nombre;

    [ObservableProperty]
    string? direccioncorreo;

    [ObservableProperty]
    [Required]
    [Phone]
    string? numerotelefono;

    [RelayCommand]
    async Task Guardar()
    {
        if (HasErrors)
        {
            Vererror = true;
            await Task.Delay(4000);
            Vererror = false;
            return;
        }
        vCard vCardSend;
        if (Direccioncorreo is not null && new EmailAddressAttribute().IsValid(Direccioncorreo))
        {
            vCardSend = new vCard(vCardLib.Enums.vCardVersion.v4)
            {
                Uid = Contacto is null ? string.Empty : Contacto.Uid,
                FormattedName = Nombre!.Trim().ToUpper(),
                Language = new Language(CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
                EmailAddresses = [new EmailAddress(Direccioncorreo!.Trim().ToLower())],
                PhoneNumbers = [new TelephoneNumber(Numerotelefono!.Trim())]
            };
        }
        else
        {
            vCardSend = new vCard(vCardLib.Enums.vCardVersion.v4)
            {
                Uid = Contacto is null ? string.Empty : Contacto.Uid,
                FormattedName = Nombre!.Trim().ToUpper(),
                Language = new Language(CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
                PhoneNumbers = [new TelephoneNumber(Numerotelefono!.Trim())]
            };
        }
        WeakReferenceMessenger.Default.Send(vCardSend, Contacto is null ? "nuevo" : "editar");
        await Cancelar();
    }

    [RelayCommand]
    async Task Cancelar()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(Contacto))
        {
            if (Contacto is not null)
            {
                Titulo = "Editar";
                Nombre = Contacto.FormattedName;
                Numerotelefono = Contacto.PhoneNumbers[0].Number;
                if (Contacto.EmailAddresses.Count > 0)
                {
                    Direccioncorreo = Contacto.EmailAddresses[0].Value;
                }
            }
        }
    }
}
