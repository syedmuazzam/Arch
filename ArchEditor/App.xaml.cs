using System.IO;
using ArchEditor.Stores;
using ArchEditor.ViewModels;
using ArchEditor.ViewModels.EditorViewModels;
using ArchEditor.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Windows;
using ArchEditor.Common;
using Serilog;

namespace ArchEditor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	private readonly IHost? _host;

	public App()
	{
		Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

		_host = Host.CreateDefaultBuilder()
			.UseSerilog((_, loggerConfiguration) =>
			{
				loggerConfiguration
					.WriteTo.File(Path.Combine(CommonConstants.ApplicationDataPath,"log-.txt"), rollingInterval:RollingInterval.Day)
					.MinimumLevel.Information();
			})
			.ConfigureServices((_, services) =>
			{
				ConfigureServices(services);
			})
			.Build();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		await _host!.StartAsync();
		ProjectBrowserWindow projectBrowserWindow = _host!.Services.GetRequiredService<ProjectBrowserWindow>();
		if (projectBrowserWindow.ShowDialog() != true)
		{
			Shutdown();
			return;
		}

		WorkspaceWindow workspaceWindow = _host!.Services.GetRequiredService<WorkspaceWindow>();
		workspaceWindow.ShowDialog();

		Shutdown();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_host!.StopAsync();
		_host.Dispose();

		base.OnExit(e);
	}

	private static void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<ProjectStore>();
		services.AddSingleton<UndoRedoStore>();

		services.AddTransient<OpenProjectViewModel>();
		services.AddTransient<CreateProjectViewModel>();
		services.AddTransient<ProjectBrowserViewModel>();

		services.AddTransient<ProjectHierarchyViewModel>();
		services.AddTransient<InspectorViewModel>();
		services.AddTransient<WorkspaceViewModel>();

		services.AddSingleton(s => new ProjectBrowserWindow
		{
			DataContext = s.GetRequiredService<ProjectBrowserViewModel>()
		});
		services.AddTransient(s => new WorkspaceWindow
		{
			DataContext = s.GetRequiredService<WorkspaceViewModel>()
		});
	}
}
