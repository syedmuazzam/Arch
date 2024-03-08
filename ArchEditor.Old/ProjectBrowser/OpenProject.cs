using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ArchEditor.Managers;
using ArchEditor.Models;
using ArchEditor.Common.Interfaces;

namespace ArchEditor.ProjectBrowser
{
	internal partial class OpenProject : ObservableObject
	{
		#region Public Properties
		[ObservableProperty]
		private string _projectName = string.Empty;

		[ObservableProperty]
		private string _projectPath = string.Empty;

		private static readonly ObservableCollection<ProjectListData> _projects = new();
		public ReadOnlyObservableCollection<ProjectListData> Projects { get; }


		private ProjectListData? _selectedProject;
		public ProjectListData? SelectedProject
		{
			get { return _selectedProject; }
			set
			{
				_selectedProject = value;
				if (_selectedProject is null) return;				
				ProjectName = _selectedProject.ProjectName;
				ProjectPath = _selectedProject.ProjectPath;
				OnPropertyChanged(nameof(SelectedProject));
			}
		}
		#endregion Public Properties

		public OpenProject()
		{
			Projects = new ReadOnlyObservableCollection<ProjectListData>(_projects);
			try
			{
				ProjectStore.GetProjectList().ForEach(project => _projects.Add(project));
				SelectedProject = _projects.FirstOrDefault();
			}
			catch (Exception ex)
			{
				// TODO: log the errors
				Debug.WriteLine(ex.Message);
			}
		}

		#region Commands
		[RelayCommand]
		private void Open(IClosable window)
		{
			if (SelectedProject is null) return;
			ProjectStore.OpenProject(SelectedProject.ProjectPath);
			Exit(window);
		}

		[RelayCommand]
		private void Exit(IClosable window)
		{
			window.Close();
		}
		#endregion Commands
	}
}
