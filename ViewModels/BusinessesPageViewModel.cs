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

    public BusinessesPageViewModel(SweetBoxWebApi apiService)
    {
        _apiService = apiService;
        SellersList = new ObservableCollection<Sellers>();

        // ����� ������ ������� ��� ����� �-ViewModel
        LoadSellers();
        // ����� ������
        SellerSelectedCommand = new Command(OnSellerSelected);
    }

    public async void LoadSellers()
    {
        // ����� ������� ��-API �� ��-DB
        var sellers = await _apiService.GetSellersAsync();

        // ����� ������� ������ ������ �����
        foreach (var seller in sellers)
        {
            SellersList.Add(seller);
        }
    }
    // ������� ������ ��� ����� �� ������ ������ �� �������
    private async void OnSellerSelected(object selectedSellerObj)
    {
        var selectedSeller = selectedSellerObj as Sellers;
        if (selectedSeller != null)
        {
            var productsPage = new BusinessProductsPage(new BusinessProductsPageViewModel(_apiService, selectedSeller.SellerId));
            await Application.Current.MainPage.Navigation.PushAsync(productsPage);
        }
    }





}