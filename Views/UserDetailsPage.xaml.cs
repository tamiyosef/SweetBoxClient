using SweetBoxApp.ViewModels;

namespace SweetBoxApp.Views;

public partial class UserDetailsPage : ContentPage
{
	public UserDetailsPage(UserDetailsPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        var backButton = new ToolbarItem
        {
            Text = "חזור"
        };
        backButton.Clicked += OnBackButtonClicked;

        // הוספת ה-ToolbarItem לסרגל הכלים של הדף
        ToolbarItems.Add(backButton);
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // חזרה לדף הקודם באמצעות Shell
        await Shell.Current.GoToAsync("//BusinessesPage");
    }
}