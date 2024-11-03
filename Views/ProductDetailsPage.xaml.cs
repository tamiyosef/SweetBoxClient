using SweetBoxApp.ViewModels;

namespace SweetBoxApp.Views;

public partial class ProductDetailsPage : ContentPage
{
	public ProductDetailsPage(ProductDetailsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}