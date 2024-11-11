
using Microsoft.Maui.Controls;
using SweetBoxApp.Models;
using SweetBoxApp.Services;
using SweetBoxApp.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SweetBoxApp.ViewModels
{
    public class BusinessProductsPageViewModel : ViewModelBase
    {
        private SweetBoxWebApi _apiService;
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<Products> ProductsList { get; set; }
        public ICommand ProductSelectedCommand { get; }
        private Products _selectedProduct;

        public BusinessProductsPageViewModel(IServiceProvider serviceProvider,SweetBoxWebApi apiService)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
            ProductsList = new ObservableCollection<Products>();

            // יצירת הפקודה לניווט לדף פרטי המוצר
            ProductSelectedCommand = new Command<Products>(OnProductSelected);
            
        }
        public Products SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                    // קריאה לפונקציה בעת בחירת מוצר
                    OnProductSelected(_selectedProduct);
                }
            }
        }

        public async Task LoadProducts(int sellerId)

        {
            ProductsList.Clear(); // אפס את הרשימה לפני הטעינה החדשה
            var products = await _apiService.GetProductsBySellerIdAsync(sellerId);

            foreach (var product in products)
            {
                ProductsList.Add(product);
            }

        }

        private async void OnProductSelected(Products selectedProduct)
        {
            if (selectedProduct == null)
                return;

            var productDetailsPage = _serviceProvider.GetRequiredService<ProductDetailsPage>();
            await Shell.Current.Navigation.PushAsync(productDetailsPage);

        }
    }
}
