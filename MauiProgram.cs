// using Microsoft.Extensions.Logging;

// namespace BtgClientManager;

// public static class MauiProgram
// {
// 	public static MauiApp CreateMauiApp()
// 	{
// 		var builder = MauiApp.CreateBuilder();
// 		builder
// 			.UseMauiApp<App>()
// 			.ConfigureFonts(fonts =>
// 			{
// 				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
// 				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
// 			});

// #if DEBUG
// 		builder.Logging.AddDebug();
// #endif

// 		return builder.Build();
// 	}
// }
using Microsoft.Maui.LifecycleEvents;

using BTGClientManager.Services;
using BTGClientManager.ViewModels;
using BTGClientManager.Views;
using BtgClientManager.Converters;
//using BtgClientManager.ViewModels;
using Microsoft.Extensions.Logging;
using BTGClientManager;

namespace BtgClientManager;

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

		// Registro de serviços
		builder.Services.AddSingleton<IClientService, ClientService>();

		// Registro de converters
		builder.Services.AddSingleton<StringCombinerConverter>();

		// Registro de ViewModels
		builder.Services.AddTransient<ClientListViewModel>();
		builder.Services.AddTransient<ClientDetailViewModel>();

		// Registro de Páginas (Views)
		builder.Services.AddTransient<ClientListPage>();
		builder.Services.AddTransient<ClientDetailPage>();

		// Configurações específicas para Windows
// 		builder.ConfigureLifecycleEvents(events =>
// 		{
// #if WINDOWS
// 			events.AddWindows(windows => windows
// 				.OnWindowCreated(window =>
// 				{
// 					window.ExtendsContentIntoTitleBar = false;
// 					var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
// 					var nativeWindow = Win32Interop.GetWindowByHandle(handle);
// 					Win32Interop.ShowWindow(nativeWindow, Win32Interop.SW_MAXIMIZE);
// 				}));
// #endif
// 		});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
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
