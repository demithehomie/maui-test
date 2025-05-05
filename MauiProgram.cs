using System.Reflection;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.EntityFrameworkCore;
using BTGClientManager.Data;
using BTGClientManager.Services;
using BTGClientManager.ViewModels;
using BTGClientManager.Views;
using BtgClientManager.Converters;

namespace BTGClientManager;

public static class MauiProgram
{
   public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf",  "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

    // 1) appsettings.json
    builder.Configuration.AddJsonFile("appsettings.json", optional: false);

    // 2) DbContext/Postgres
    var conn = builder.Configuration.GetConnectionString("Postgres")!;
    builder.Services.AddDbContextFactory<AppDbContext>(opt =>
        opt.UseNpgsql(conn, npg => npg.EnableRetryOnFailure(3)));

    // 3) Demais serviços
    builder.Services.AddTransient<IClientService, ClientServicePersistent>();
    builder.Services.AddSingleton<StringCombinerConverter>();

    builder.Services.AddTransient<ClientListViewModel>();
    builder.Services.AddTransient<ClientDetailViewModel>();
    builder.Services.AddTransient<ClientListPage>();
    builder.Services.AddTransient<ClientDetailPage>();

#if DEBUG
    builder.Logging.AddDebug();
#endif

    // 🔸 constrói UMA ÚNICA VEZ
    var app = builder.Build();

    // 🔸 aplica migrations após a construção
    using (var scope = app.Services.CreateScope())
  {
    // resolve via fábrica (não precisa AddDbContext duplicado)
    var db = scope.ServiceProvider
                  .GetRequiredService<IDbContextFactory<AppDbContext>>()
                  .CreateDbContext();
    db.Database.Migrate();        // cria/atualiza tabelas
}

    return app;
}


  

#if WINDOWS
    public class Win32Interop
    {
        public const int SW_MAXIMIZE = 3;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetWindowByHandle(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
#endif
}
