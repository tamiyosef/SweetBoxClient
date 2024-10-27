using SweetBoxApp.Views;

namespace SweetBoxApp
{
    public partial class App : Application
    {
        public App(LoginPage loginPage)
        {
            InitializeComponent();

            //MainPage = new AppShell();

            MainPage = new NavigationPage(loginPage);


        }
    }
}
