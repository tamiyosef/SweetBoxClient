using SweetBoxApp.ViewModels;

namespace SweetBoxApp.Views;

public partial class BusinessProductsPage : ContentPage
{
	public BusinessProductsPage(BusinessProductsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}