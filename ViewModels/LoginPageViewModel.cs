using SweetBoxApp.Models;
using SweetBoxApp.Services;
using SweetBoxApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SweetBoxApp.ViewModels;

public class LoginPageViewModel : ViewModelBase
{

    public SweetBoxWebApi service;
    private readonly IServiceProvider serviceProvider;

    public ICommand LoginCommand { get; set; }
    public ICommand GoToRegisterCommand { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }

    public LoginPageViewModel(SweetBoxWebApi service, IServiceProvider serviceProvider)
	{
        this.service = service;
        this.serviceProvider = serviceProvider;
        this.LoginCommand = new Command(OnLogin);
        this.GoToRegisterCommand = new Command(OnRegisterClicked);
    }

    public async void OnLogin()
    {
        LoginInfo info = new LoginInfo
        {
            Email = this.Email,
            Password = this.Password
        };
        User user = await service.Login(info);
        if (user != null)
        {
            if (user.UserType == "3")
            {
                var businessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
                await App.Current.MainPage.Navigation.PushAsync(businessesPage);
            }
            if (user.UserType == "2")
            {
                //Navigate to the main page
                App.Current.MainPage = new MainPage();
            }
            //Navigate to the main page
            //App.Current.MainPage = new MainPage();
            // קבלת הדף דרך ה-DI וניווט אליו
            //var businessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
            //await App.Current.MainPage.Navigation.PushAsync(businessesPage);
        } 
        
        else
        {
            Debug.WriteLine("Login failed");
        } 
    }

    private async void OnRegisterClicked()
    {
        // קבלת הדף דרך ה-DI וניווט אליו
        var registrationPage = serviceProvider.GetRequiredService<RegistrationPage>();
        await App.Current.MainPage.Navigation.PushAsync(registrationPage);
    }





}


