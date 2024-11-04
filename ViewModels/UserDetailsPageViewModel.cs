using System;
using System.Windows.Input;
using SweetBoxApp.Services;
using SweetBoxApp.Models;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace SweetBoxApp.ViewModels
{
    public class UserDetailsPageViewModel : ViewModelBase
    {
        private readonly SweetBoxWebApi _apiService;
        private readonly IServiceProvider _serviceProvider;
        private int userId;
        private string fullName;
        private string email;
        private string password;
        private string userType;
        private string? errorMessage;

        public ICommand SaveCommand { get; private set; }

        public UserDetailsPageViewModel(SweetBoxWebApi apiService, IServiceProvider serviceProvider)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
            SaveCommand = new Command(SaveUserDetails);
        }

        public int UserId
        {
            get => userId;
            set
            {
                userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }


        public string FullName
        {
            get => fullName;
            set
            {
                if (fullName != value)
                {
                    fullName = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(FullName));
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
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Password));
                }
            }
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
                }
            }
        }


        public string UserType
        {
            get => userType;
            set
            {
                if (userType != value)
                {
                    userType = value;
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }


        public string? ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public void Initialize(User user)
        {
            UserId = user.UserId;
            FullName = user.FullName;
            Email = user.Email;
            UserType = user.UserType;
            Password = user.Password;
        }

        private async void SaveUserDetails()
        {
            try
            {
                User updatedUser = new User
                {
                    UserId = UserId,
                    FullName = FullName,
                    Email = Email,
                    Password = Password,
                    UserType = UserType,
                };

                bool isSuccess = await _apiService.UpdateUserDetailsAsync(updatedUser);

                if (isSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "User details updated successfully.", "OK");
                }
                else
                {
                    ErrorMessage = "Failed to update user details. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

    }
}
