using SweetBoxApp.ViewModels;


namespace SweetBoxApp.Views;

public partial class BusinessesPage : ContentPage
{
	public BusinessesPage(BusinessesPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;	
    }
}