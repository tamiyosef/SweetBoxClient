using SweetBoxApp.Views;
using System.Windows.Input;

namespace SweetBoxApp;

public partial class AppShell : Shell

{
    public AppShell()
    {
        InitializeComponent();
        // ����� ������ ������ � SHELL
        // ������ ��� ���� ������� ������ �� ������ �������
        Routing.RegisterRoute("ProductDetailsPage", typeof(ProductDetailsPage));
    }
}
