using ArchEditor.Common.Interfaces;
using ArchEditor.Models;
using ArchEditor.Stores;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Serilog;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ArchEditor.ViewModels;

internal partial class OpenProjectViewModel : ObservableObject
{
	private readonly ProjectStore _projectStore;

	#region Public Properties
	[ObservableProperty]
	private string _projectName = string.Empty;

	[ObservableProperty]
	private string _projectPath = string.Empty;

	private static readonly ObservableCollection<ProjectListData> _projects = new();
	public ReadOnlyObservableCollection<ProjectListData> Projects { get; }

	[ObservableProperty]
	private ProjectListData? _selectedProject;
	partial void OnSelectedProjectChanged(ProjectListData? value)
	{
		if (value is null) return;
		ProjectName = value.ProjectName;
		ProjectPath = value.ProjectPath;
	}
	#endregion Public Properties

	public OpenProjectViewModel(ProjectStore projectStore)
	{
		_projectStore = projectStore;
		Projects = new ReadOnlyObservableCollection<ProjectListData>(_projects);
		try
		{
			_projectStore.GetProjectList().ForEach(project => _projects.Add(project));
			SelectedProject = _projects.FirstOrDefault();
		}
		catch (Exception ex)
		{
			Log.Error("{ex.Message}", ex.Message);
			throw;
		}
	}

	#region Commands
	[RelayCommand]
	private void Open(ICloseable window)
	{
		try
		{
			if (SelectedProject is null) return;
			_projectStore.OpenProject(SelectedProject.FullPath);
			Exit(window);
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	[RelayCommand]
	private static void Exit(ICloseable window)
	{
		window.Close();
	}
	#endregion Commands
}
