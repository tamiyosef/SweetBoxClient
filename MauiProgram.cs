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


            // עבור קונה
            // עמוד הצגת עסקים
            builder.Services.AddTransient<BusinessesPageViewModel>();
            builder.Services.AddTransient<BusinessesPage>();
            // משותף עבור הקונה והמוכר : עמוד הצגת המוצרים של מוכר
            builder.Services.AddSingleton<BusinessProductsPage>();
            builder.Services.AddSingleton<BusinessProductsPageViewModel>();
            // עמוד הצגת פרטים אישיים עבור קונה
            builder.Services.AddSingleton<UserDetailsPage>();
            builder.Services.AddSingleton<UserDetailsPageViewModel>();

            // עבור מוכר :
            // עמוד המשך הרשמה עבור מוכר
            builder.Services.AddTransient<SellerRegistrationPage>();
            builder.Services.AddTransient<SellerRegistrationPageViewModel>();
            // עמוד הצגת המוצרים של העסק שלו
            builder.Services.AddSingleton<SellerDetailsPage>();
            builder.Services.AddSingleton<SellerDetailsPageViewModel>();
            // עמוד פרטי המוצר
            builder.Services.AddSingleton<ProductDetailsPage>();
            builder.Services.AddSingleton<ProductDetailsViewModel>();
            // עמוד פרטי העסק
            builder.Services.AddSingleton<SellerDetailsPage>();
            builder.Services.AddSingleton<SellerDetailsPageViewModel>();



            //הוספת השירותים לשירות DI
            builder.Services.AddSingleton<SweetBoxWebApi>();

            //הוספת Shell של קונה ומוכר לשירות DI
            builder.Services.AddTransient<SellerShell>();
            builder.Services.AddTransient<AppShell>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
