using SweetBoxApp.ViewModels;
namespace SweetBoxApp.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


}


