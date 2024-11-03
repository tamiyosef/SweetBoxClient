using SweetBoxApp.Views;
using System.Windows.Input;

namespace SweetBoxApp;

public partial class AppShell : Shell

{
    public AppShell()
    {
        InitializeComponent();
        // הוספת עמודים נוספים ל SHELL
        // עמודים שלא נרצה שיופיעו בטאבים או בתפריט המבורגר
        Routing.RegisterRoute("ProductDetailsPage", typeof(ProductDetailsPage));
    }
}
