
using SweetBoxApp.Models;
using SweetBoxApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SweetBoxApp.ViewModels
{
    public class BusinessProductsPageViewModel : ViewModelBase
    {
        private SweetBoxWebApi _apiService;
        public ObservableCollection<Products> ProductsList { get; set; }

        public BusinessProductsPageViewModel(SweetBoxWebApi apiService, int sellerId)
        {
            _apiService = apiService;
            ProductsList = new ObservableCollection<Products>();

            // טעינת המוצרים של המוכר
            LoadProducts(sellerId);
        }

        public async void LoadProducts(int sellerId)
        {
            var products = await _apiService.GetProductsBySellerIdAsync(sellerId);

            foreach (var product in products)
            {
                ProductsList.Add(product);
            }
        }
    }
}
