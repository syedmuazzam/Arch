using ArchEditor.Common.Interfaces;
using ArchEditor.ViewModels;

using System.Windows;

namespace ArchEditor.Views;

/// <summary>
/// Interaction logic for ProjectBrowserWindow.xaml
/// </summary>
public partial class ProjectBrowserWindow : ICloseable
{
	public ProjectBrowserWindow()
	{
		InitializeComponent();
		Loaded += OnProjectBrowserWindowLoaded;
	}

	private void OnProjectBrowserWindowLoaded(object sender, RoutedEventArgs e)
	{
		Loaded -= OnProjectBrowserWindowLoaded;
		if (((ProjectBrowserViewModel)DataContext).OpenProjectViewModel.Projects.Count == 0)
		{
			TabControl.SelectedIndex = 1;
		}
	}

	private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
	{
		if (DataContext is not ProjectBrowserViewModel viewModel) return;
		if (viewModel.GetProjectToOpen() != string.Empty)
		{
			DialogResult = true;
			viewModel.SetProjectToOpen(string.Empty);
		}

		viewModel.OnWindowClose();
	}
}

