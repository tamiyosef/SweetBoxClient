using SweetBoxApp.ViewModels;

namespace SweetBoxApp.Views;

public partial class SellerDetailsPage : ContentPage
{
	public SellerDetailsPage(SellerDetailsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}