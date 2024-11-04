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
            Text = "����"
        };
        backButton.Clicked += OnBackButtonClicked;

        // ����� �-ToolbarItem ����� ����� �� ���
        ToolbarItems.Add(backButton);
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // ���� ��� ����� ������� Shell
        await Shell.Current.GoToAsync("//BusinessesPage");
    }
}