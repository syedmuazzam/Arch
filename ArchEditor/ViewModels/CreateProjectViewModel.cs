using ArchEditor.Common.Interfaces;
using ArchEditor.Models;
using ArchEditor.Stores;
using ArchEditor.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Serilog;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
// ReSharper disable UnusedParameterInPartialMethod

namespace ArchEditor.ViewModels;

internal partial class CreateProjectViewModel : ObservableObject
{
	// TODO: get the path from the installation location
	private const string _templatePath = @"..\..\ArchEditor\ProjectTemplates";
	private readonly ProjectStore _projectStore;

	#region Public Properties
	[ObservableProperty]
	private string _projectName = "NewProject";
	partial void OnProjectNameChanged(string value) => ValidateProjectPath();

	[ObservableProperty]
	private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ArchProjects";
	partial void OnProjectPathChanged(string value) => ValidateProjectPath();

	[ObservableProperty] private bool _isPathValid;

	[ObservableProperty] private string _errorMsg = string.Empty;

	private readonly ObservableCollection<ProjectTemplate> _projectTemplates = new();
	public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

	[ObservableProperty]
	private ProjectTemplate? _selectedProjectTemplate;
	partial void OnSelectedProjectTemplateChanged(ProjectTemplate? value) => ValidateProjectPath();

	#endregion Public Properties

	public CreateProjectViewModel(ProjectStore projectStore)
	{
		ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
		_projectStore = projectStore;
		try
		{
			string[] templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
			Debug.Assert(templateFiles.Any());
			foreach (string templateFile in templateFiles)
			{
				ProjectTemplate? template = Serializer.FromFile<ProjectTemplate>(templateFile);
				string templateFolder = Path.GetDirectoryName(templateFile) ?? "";
				if (string.IsNullOrEmpty(templateFolder) || template is null)
					continue;
				template.DisplaySnippet = File.ReadAllBytes(Path.Combine(templateFolder, "Snippet.png"));
				template.ProjectFilePath = Path.GetFullPath(Path.Combine(templateFolder, template.ProjectFile!));
				_projectTemplates.Add(template);
			}

			SelectedProjectTemplate = _projectTemplates.FirstOrDefault();
			ValidateProjectPath();
		}
		catch (Exception ex)
		{
			Log.Error("{ex.Message}", ex.Message);
			throw;
		}
	}

	#region Commands
	[RelayCommand]
	private void Create(ICloseable window)
	{
		try
		{
			ValidateProjectPath();
			if (!IsPathValid) return;
			string path = Path.Combine(ProjectPath, ProjectName);
			Project project = _projectStore.CreateProject(ProjectName, path, SelectedProjectTemplate!);
			if (string.IsNullOrEmpty(project.ProjectPath))
			{
				ErrorMsg = "Something went wrong while creating the project";
				throw new Exception(ErrorMsg);
			}

			_projectStore.OpenProject(project.FullPath);
			Exit(window);
		}
		catch (Exception ex)
		{
			Log.Error("{ex.Message}", ex.Message);
			throw;
		}
	}

	[RelayCommand]
	private static void Exit(ICloseable window)
	{
		window.Close();
	}
	#endregion Commands

	/// <summary>
	/// Validates the project path and name
	/// </summary>
	/// <returns>
	/// Returns true if the path and name are valid, otherwise returns false
	/// </returns>
	private void ValidateProjectPath()
	{
		string path = ProjectPath;
		if (!Path.EndsInDirectorySeparator(path))
		{
			path += Path.DirectorySeparatorChar;
			ProjectPath = path;
		}

		path += $@"{ProjectName}" + Path.DirectorySeparatorChar;
		IsPathValid = false;
		if (string.IsNullOrEmpty(ProjectName.Trim()))
		{
			ErrorMsg = "Project name cannot be empty";
		}
		else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			ErrorMsg = "Project name contains invalid characters";
		}
		else if (string.IsNullOrEmpty(ProjectPath.Trim()))
		{
			ErrorMsg = "Project path cannot be empty";
		}
		else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			ErrorMsg = "Project path contains invalid characters";
		}
		else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
		{
			ErrorMsg = "Project path already exists and is not empty";
		}
		else if (SelectedProjectTemplate is null)
		{
			ErrorMsg = "Please select a project template";
		}
		else
		{
			IsPathValid = true;
			ErrorMsg = string.Empty;
		}
	}
}
