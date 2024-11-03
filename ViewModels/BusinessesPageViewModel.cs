using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using SweetBoxApp.Models;
using SweetBoxApp.Services;
using SweetBoxApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SweetBoxApp.ViewModels;

public class BusinessesPageViewModel : ViewModelBase
{

    private SweetBoxWebApi _apiService;
    private Sellers selectedSeller;
    private readonly IServiceProvider _serviceProvider;
    public bool IsSellerSelected => SelectedSeller != null;

    public ObservableCollection<Sellers> SellersList { get; set; }

    public Sellers SelectedSeller
    {
        get => selectedSeller;
        set
        {
            selectedSeller = value;
            OnPropertyChanged(nameof(SelectedSeller));
            OnPropertyChanged(nameof(IsSellerSelected));
            if (SellerSelectedCommand.CanExecute(selectedSeller))
            {
                SellerSelectedCommand.Execute(selectedSeller);
            }

        }
    }
    public ICommand SellerSelectedCommand { get; }

    public BusinessesPageViewModel(SweetBoxWebApi apiService, IServiceProvider serviceProvider)
    {
        _apiService = apiService;
        SellersList = new ObservableCollection<Sellers>();

        // קריאה לטעינת המוכרים בעת יצירת ה-ViewModel
        LoadSellers();
        // אתחול הפקודה
        SellerSelectedCommand = new Command(OnSellerSelected);
        _serviceProvider = serviceProvider;
    }

    public async Task LoadSellers()
    {
        // שליפת המוכרים מה-API או מה-DB
        var sellers = await _apiService.GetSellersAsync();

        // הוספת המוכרים שנשלפו לרשימה שתוצג
        foreach (var seller in sellers)
        {
            SellersList.Add(seller);
        }
    }
    // פונקציה שתופעל בעת לחיצה על הכפתור לפתיחת דף המוצרים
    private async void OnSellerSelected(object selectedSellerObj)
    {
        var selectedSeller = selectedSellerObj as Sellers;
        if (selectedSeller != null)
        {
            // קבלת הדף וה-ViewModel מ-DI
            var productsPage = _serviceProvider.GetRequiredService<BusinessProductsPage>();
            var viewModel = _serviceProvider.GetRequiredService<BusinessProductsPageViewModel>();

            // העברת המזהה של המוכר ל-ViewModel
            viewModel.LoadProducts(selectedSeller.SellerId);

            // הגדרת ה-BindingContext של הדף
            productsPage.BindingContext = viewModel;

            // ניווט לדף
            await Shell.Current.Navigation.PushAsync(productsPage);

        }
    }





}