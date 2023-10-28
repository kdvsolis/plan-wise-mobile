using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlanWise.Data;
using System.Reflection;

namespace PlanWise
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            using var stream = Assembly.GetExecutingAssembly()
    .GetManifestResourceStream("PlanWise.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Configuration.AddConfiguration(config);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("url")) });
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}