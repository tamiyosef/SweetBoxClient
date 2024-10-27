using Microsoft.Win32;
using SweetBoxApp.Models;
using SweetBoxApp.ViewModels;
using SweetBoxApp.Views;
using SweetBoxApp.Services;
using System.Windows.Input;
namespace SweetBoxApp.ViewModels;

public class SellerRegistrationPageViewModel : ViewModelBase
{
    private SweetBoxWebApi api_service;
    private readonly IServiceProvider serviceProvider;
    public int SellerId { get; set; }
    public string businessName;
    public string businessAddress;
    public string businessPhone;
    public string description;
    public string? profilePicture;
    public SellerRegistrationPageViewModel(SweetBoxWebApi api_service, IServiceProvider serviceProvider)
    {
		this.api_service = api_service;
        this.serviceProvider = serviceProvider;
        //this.SellerId = sellerId;
        SellerRegistrationCommand = new Command(Seller_Registraton);
    }

    // פונקציה לאתחול ה-SellerId אחרי יצירת ה-ViewModel דרך DI
    public void Initialize(int sellerId)
    {
        this.SellerId = sellerId;
    }

    public string BusinessName
    {
        get { return businessName; }
        set
        {
            businessName = value;
            OnPropertyChanged(nameof(BusinessName));
            
        }
    }

    public string BusinessAddress
    {
        get { return businessAddress; }
        set
        {
            businessAddress = value;
            OnPropertyChanged(nameof(BusinessAddress));

        }
    }

    public string BusinessPhone
    {
        get { return businessPhone; }
        set
        {
            businessPhone = value;
            OnPropertyChanged(nameof(BusinessPhone));

        }
    }

    public string Description
    {
        get { return description; }
        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));

        }
    }

    public string ProfilePicture
    {
        get { return profilePicture; }
        set
        {
            profilePicture = value;
            OnPropertyChanged(nameof(ProfilePicture));

        }
    }

    public ICommand SellerRegistrationCommand
    {
        get; private set;
    }

    public async void Seller_Registraton()
    {

        // יצירת אובייקט המוכר עם הפרטים שהוכנסו ע"י הלקוח והמספר המזהה שנוצר בעמוד הקודם 
        Sellers seller = new Sellers
         {
            SellerId = SellerId, // ה-SellerId הוא ה-UserId של המשתמש
            BusinessName = BusinessName,
            BusinessAddress = BusinessAddress,
            BusinessPhone = BusinessPhone,
            ProfilePicture = ProfilePicture,
            Description = Description
         };

        // שליחת הנתונים לשרת לצורך הוספת המוכר
        bool success = await api_service.RegisterSeller(seller);

        if (success)
        {
            await Application.Current.MainPage.DisplayAlert("הצלחה", "העסק נרשם בהצלחה!", "אישור");
            // ניווט לדף SellerDetailsPage
            var sellerDetailsPage = serviceProvider.GetRequiredService<SellerDetailsPage>();
            (sellerDetailsPage.BindingContext as SellerDetailsPageViewModel)?.Initialize(SellerId);


            // הניווט לעמוד החדש, אין צורך להגדיר את ה-BindingContext מכיוון שזה נעשה אוטומטית בקובץ XAML.CS
            await App.Current.MainPage.Navigation.PushAsync(sellerDetailsPage);

        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("שגיאה", "הרשמת העסק נכשלה, נסה שוב.", "אישור");
        }
    }


}
