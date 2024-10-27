using SweetBoxApp.ViewModels;

namespace SweetBoxApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		this.BindingContext = vm;
        InitializeComponent();
	}
}