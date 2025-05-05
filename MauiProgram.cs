using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Npgsql;
using BTGClientManager.Data;
using BTGClientManager.Services;
using BTGClientManager.ViewModels;
using BTGClientManager.Views;
using BtgClientManager.Converters;
using BTGClientManager.Helpers;
using System.Diagnostics;

namespace BTGClientManager;

public static class MauiProgram
{
    // 🔐 Coloque aqui a connection-string que você quiser usar.
    // Exemplo: local/ou remota — escolha uma e comente a outra.
    private const string ConnectionString =
        // @"Host=localhost;Port=5432;Database=btgclientdb;Username=postgres;Password=sua_senha";
        @"Host=dpg-d0cdknqli9vc73b7v4ng-a.oregon-postgres.render.com;Port=5432;Database=testemaui;Username=demetrius;Password=sMTTzzwwrekZIRhCzIU4twOJb1GSPjlc;SslMode=Require;Trust Server Certificate=true";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf",  "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        try
        {
            // Diagnosticar e corrigir problemas de conexão 
            Task.Run(async () => 
            {
                try
                {
                    // Garantir criação da tabela
                    await ConnectionDebugHelper.EnsureTableCreated(ConnectionString);
                    
                    // Executar diagnóstico e mostrar resultados detalhados
                    var diagnosis = await ConnectionDebugHelper.DiagnoseConnection(ConnectionString);
                    Debug.WriteLine("==== DIAGNÓSTICO DE CONEXÃO ====");
                    Debug.WriteLine(diagnosis);
                    Debug.WriteLine("================================");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERRO AO DIAGNOSTICAR: {ex}");
                }
            }).Wait();

            // Configura o DbContextFactory com modificações de case sensitivity
            builder.Services.AddDbContextFactory<AppDbContext>(opt =>
                opt.UseNpgsql(ConnectionString, npgsqlOptions =>
                    npgsqlOptions.EnableRetryOnFailure(3)  // Adiciona retentativas em caso de falha
                )
                .LogTo(message => Debug.WriteLine($"[EF Core] {message}"), 
                       LogLevel.Information)
                .EnableSensitiveDataLogging()
            );

            // Serviços, converters, VMs e páginas.
            builder.Services.AddTransient<IClientService, ClientServicePersistent>();
            builder.Services.AddSingleton<StringCombinerConverter>();
            builder.Services.AddTransient<ClientListViewModel>();
            builder.Services.AddTransient<ClientDetailViewModel>();
            builder.Services.AddTransient<ClientListPage>();
            builder.Services.AddTransient<ClientDetailPage>();

            // Constrói e testa a conexão
            var app = builder.Build();

            // Verifica a conexão com o banco dentro de um novo escopo
            using (var scope = app.Services.CreateScope())
            {
                var ctxFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
                using var db = ctxFactory.CreateDbContext();
                
                // Força o EF Core a tentar conectar
                var canConnect = db.Database.CanConnect();
                Debug.WriteLine($"Teste de conexão EF Core: {(canConnect ? "SUCESSO" : "FALHA")}");
                
                if (canConnect)
                {
                    Debug.WriteLine("Executando Migrate...");
                    db.Database.Migrate();
                    
                    // Testa se consegue contar os clientes
                    try
                    {
                        var count = db.Clients.Count();
                        Debug.WriteLine($"Contagem de clientes: {count}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Erro ao contar clientes: {ex.Message}");
                    }
                }
            }
            
            return app;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ERRO CRÍTICO NA INICIALIZAÇÃO: {ex}");
            throw; // Reraise para que o erro não passe despercebido
        }
    }
}