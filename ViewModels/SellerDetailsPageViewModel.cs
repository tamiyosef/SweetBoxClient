using SweetBoxApp.Models;
using SweetBoxApp.Services;
using System.Windows.Input;

namespace SweetBoxApp.ViewModels
{
    public class SellerDetailsPageViewModel : ViewModelBase
    {
        private SweetBoxWebApi api_service;

        public int SellerId { get; private set; }

        private string businessName;
        private string businessAddress;
        private string businessPhone;
        private string description;
        private string profilePicture;

        public SellerDetailsPageViewModel(SweetBoxWebApi api_service)
        {
            this.api_service = api_service;
        }

        public string BusinessName
        {
            get => businessName;
            set
            {
                businessName = value;
                OnPropertyChanged(nameof(BusinessName));
            }
        }

        public string BusinessAddress
        {
            get => businessAddress;
            set
            {
                businessAddress = value;
                OnPropertyChanged(nameof(BusinessAddress));
            }
        }

        public string BusinessPhone
        {
            get => businessPhone;
            set
            {
                businessPhone = value;
                OnPropertyChanged(nameof(BusinessPhone));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string ProfilePicture
        {
            get => profilePicture;
            set
            {
                profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        public async void Initialize(int sellerId)
        {
            SellerId = sellerId;

            // קבלת פרטי המוכר מה-API
            var seller = await api_service.GetSellerByIdAsync(SellerId);

            if (seller != null)
            {
                BusinessName = seller.BusinessName;
                BusinessAddress = seller.BusinessAddress;
                BusinessPhone = seller.BusinessPhone;
                Description = seller.Description;
                ProfilePicture = seller.ProfilePicture;
            }
        }
    }
}
