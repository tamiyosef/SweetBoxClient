using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SweetBoxApp.Services;
using SweetBoxApp.Models;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SweetBoxApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;




namespace SweetBoxApp.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {

        private string name;
        private int? userId {  get; set; }
        private string? user_error;
        private string email;
        private string password;
        private string? password_error;
        private string user_type;
        private bool isSellerChecked;
        private bool isBuyerChecked;
        private readonly IServiceProvider serviceProvider;

        // הוספת אובייקט ממחלקת השירותים שיוכל להפעיל את הפונקציות במחלקה
        private SweetBoxWebApi api_service;


        public RegistrationPageViewModel(SweetBoxWebApi api_service, IServiceProvider serviceProvider)
        {
            this.api_service = api_service;
            RegistrationCommand = new Command(Register);
            this.serviceProvider = serviceProvider;


        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                User_Error = ""; // איפוס שגיאת שם המשתמש
                OnPropertyChanged(nameof(Name));
                // בדיקת תקינות שם המשתמש

                if (!string.IsNullOrEmpty(Name))

                {
                    if (char.IsDigit(Name[0]))
                    {
                        User_Error = "!!שם המשתמש לא יכול להתחיל בספרה!!";
                        OnPropertyChanged(nameof(Name));
                    }
                    
                }
            }
        }

        public string User_Error
        {
            get
            {
                return user_error;
            }
            set
            {
                user_error = value;
                OnPropertyChanged(nameof(User_Error));
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string? Password
        {
            get { return password; }
            set
            {
                password = value;
                Password_Error = "";
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(User_Error));
                if (string.IsNullOrEmpty(password))
                {
                    Password_Error = ""; // איפוס השגיאה אם השדה ריק
                }
                else 
                {
                    if (password != null)
                    {
                        bool IsPasswordOk = IsValidPassword(password);
                        if (!IsPasswordOk)
                        {
                            Password_Error = "!!סיסמה חייבת להכיל לפחות אות גדולה אחת ומספר!!";
                        }
                    }
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            bool hasUpperCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }

                if (hasUpperCase && hasDigit)
                {
                    break; // אם מצאנו כבר גם אות גדולה וגם ספרה, אפשר לעצור את הלולאה
                }
            }
            return hasUpperCase && hasDigit;

        }

        public string Password_Error
        {
            get { return password_error; }
            set
            {
                password_error = value;
                OnPropertyChanged(nameof(Password_Error));
                //OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string User_Type
        {
            get
            {
                return user_type;
            }
            set
            {
                user_type = value;
                OnPropertyChanged(nameof(User_Type));
            }
        }

        // מאפיין לבחירת "מוכר"
        public bool IsSellerChecked
        {
            get { return isSellerChecked; }
            set
            {
                if (isSellerChecked != value)
                {
                    isSellerChecked = value;
                    if (isSellerChecked)
                    {
                        User_Type = "2"; // עדכון ל-2 עבור מוכר
                        IsBuyerChecked = false; // Uncheck the Buyer radio button
                    }
                    OnPropertyChanged(nameof(IsSellerChecked));
                }
            }
        }

        // מאפיין לבחירת "קונה"
        public bool IsBuyerChecked
        {
            get { return isBuyerChecked; }
            set
            {
                if (isBuyerChecked != value)
                {
                    isBuyerChecked = value;
                    if (isBuyerChecked)
                    {
                        User_Type = "3"; // עדכון ל-3 עבור קונה
                        IsSellerChecked = false; // Uncheck the Seller radio button
                    }
                    OnPropertyChanged(nameof(IsBuyerChecked));
                }
            }
        }



        public ICommand RegistrationCommand
        {
            get; private set;
        }

        public async void Register()
        {
            Models.User user = new Models.User
            {
                
                FullName = name,
                Email = email,
                UserType = user_type,
                Password = password,
            };


            // check
            int? res = await this.api_service.Register(user);
            // אם ההרשמה הצליחה
            if (res!=null)
            {
                // בדיקת סוג המשתמש
                if (User_Type == "2") // אם המשתמש הוא מוכר
                {

                    // קבלת ה-SellerRegistrationPage וה-ViewModel דרך DI
                    var sellerRegistrationPage = serviceProvider.GetRequiredService<SellerRegistrationPage>();
                    var sellerRegistrationViewModel = serviceProvider.GetRequiredService<SellerRegistrationPageViewModel>();

                    // אתחול ה-ViewModel עם ה-SellerId שנוצר
                    sellerRegistrationViewModel.Initialize((int)res);

                    // הגדרת ה-ViewModel כ-BindingContext של הדף
                    sellerRegistrationPage.BindingContext = sellerRegistrationViewModel;
                    await App.Current.MainPage.Navigation.PushAsync(sellerRegistrationPage);

                }
                else if (User_Type == "3") // אם המשתמש הוא קונה
                {
                    // מעבר לדף BusinessesPage
                    var BusinessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
                    await App.Current.MainPage.Navigation.PushAsync(BusinessesPage);
                }
            }
            else
            {
                // טיפול במקרה שההרשמה נכשלה (הודעת שגיאה למשתמש, למשל)
                await Application.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה, נסה שוב.", "אישור");
            }




        }


    }
}
