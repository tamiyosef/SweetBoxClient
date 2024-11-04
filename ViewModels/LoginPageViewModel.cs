using Microsoft.Extensions.DependencyInjection;
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
    private string email;
    private string password;

    public LoginPageViewModel(SweetBoxWebApi service, IServiceProvider serviceProvider)
	{
        this.service = service;
        this.serviceProvider = serviceProvider;
        this.LoginCommand = new Command(OnLogin);
        this.GoToRegisterCommand = new Command(OnRegisterClicked);
    }

    public string Email
    {
        get => email;
        set
        {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
                // can add more logic here like email validation etc.
                // can add error message property and set it here
            }
        }
    }

    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                // can add more logic here like email validation etc.
                // can add error message property and set it here
            }
        }
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

                var buyerShell = serviceProvider.GetRequiredService<AppShell>();
                App.Current.MainPage = buyerShell;

                // ���� UserDetailsPageViewModel ��-DI ������ �������
                var userDetailsViewModel = serviceProvider.GetRequiredService<UserDetailsPageViewModel>();
                userDetailsViewModel.Initialize(user); // ����� ���� ������

                // ����� BindingContext ���� UserDetailsPage
                var userDetailsPage = serviceProvider.GetRequiredService<UserDetailsPage>();
                userDetailsPage.BindingContext = userDetailsViewModel;



            }
            if (user.UserType == "2")
            {
               
                // ���� BusinessProductsPageViewModel �-DI
                var viewModel = serviceProvider.GetRequiredService<BusinessProductsPageViewModel>();

                // ����� ������� �� �����
                await viewModel.LoadProducts(user.UserId);

                // ����� �-BindingContext �� BusinessProductsPage
                var businessProductsPage = serviceProvider.GetRequiredService<BusinessProductsPage>();
                businessProductsPage.BindingContext = viewModel;
                var sellerShell = serviceProvider.GetRequiredService<SellerShell>();

                App.Current.MainPage = sellerShell;

                // ���� SellerDetailsPageViewModel �-DI ������ �������
                var sellerDetailsViewModel = serviceProvider.GetRequiredService<SellerDetailsPageViewModel>();
                await sellerDetailsViewModel.Initialize(user.UserId);

                // ����� BindingContext ���� SellerDetailsPage
                var sellerDetailsPage = serviceProvider.GetRequiredService<SellerDetailsPage>();
                sellerDetailsPage.BindingContext = sellerDetailsViewModel;

            }

        } 
        
        else
        {
            Debug.WriteLine("Login failed");
        } 
    }

    private async void OnRegisterClicked()
    {
        // ���� ��� ��� �-DI ������ ����
        var registrationPage = serviceProvider.GetRequiredService<RegistrationPage>();
        await App.Current.MainPage.Navigation.PushAsync(registrationPage);
    }





}


