using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SweetBoxApp.Services;
using SweetBoxApp.ViewModels;
using SweetBoxApp.Views;

namespace SweetBoxApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //הוספת העמודים לשירות DI
            // עמוד התחברות כללי
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            // עמוד הרשמה כללי
            builder.Services.AddSingleton<RegistrationPage>();
            builder.Services.AddSingleton<RegistrationPageViewModel>();
            // עמוד המשך הרשמה עבור מוכר
            builder.Services.AddTransient<SellerRegistrationPage>();
            builder.Services.AddTransient<SellerRegistrationPageViewModel>();
            // עמוד הצגת עסקים עבור קונה
            builder.Services.AddTransient<BusinessesPageViewModel>();
            builder.Services.AddTransient<BusinessesPage>();
            //  עבור הקונה : עמוד הצגת המוצרים של המוכר
            builder.Services.AddSingleton<BusinessProductsPage>();
            builder.Services.AddSingleton<BusinessProductsPageViewModel>();

            // עבור המוכר : עמוד הצגת המוצרים של העסק שלו
            builder.Services.AddSingleton<SellerDetailsPage>();
            builder.Services.AddSingleton<SellerDetailsPageViewModel>();

            builder.Services.AddSingleton<ProductDetailsPage>();
            builder.Services.AddSingleton<ProductDetailsViewModel>();


            //הוספת השירותים לשירות DI
            builder.Services.AddSingleton<SweetBoxWebApi>();


            //הוספת Shell של קונה ומוכר לשירות DI
            builder.Services.AddTransient<SellerShell>();
            builder.Services.AddTransient<AppShell>();
            //builder.Services.AddSingleton<BuyerShell>();
            //builder.Services.AddSingleton<SellerShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
