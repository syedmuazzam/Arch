using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ArchEditor.Models;
using ArchEditor.Utilities;
using ArchEditor.Managers;
using ArchEditor.Common.Interfaces;

namespace ArchEditor.ProjectBrowser
{
	internal partial class CreateProject : ObservableObject
	{
		// TODO: get the path from the installation location
		private readonly string templatePath = @"..\..\ArchEditor\ProjectTemplates";

		#region Public Properties
		private string _projectName = "NewProject";
		public string ProjectName
		{
			get => _projectName;
			set
			{
				_projectName = value;
				ValidateProjectPath();
				SetProperty(ref _projectName, value);
			}
		}

		private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ArchProjects";
		public string ProjectPath
		{
			get => _projectPath;
			set
			{
				_projectPath = value;
				ValidateProjectPath();
				SetProperty(ref _projectPath, value);
			}
		}

		[ObservableProperty]
		private bool _isPathValid;

		[ObservableProperty]
		private string _errorMsg = string.Empty;

		private ObservableCollection<ProjectTemplate> _projectTemplates = new();
		public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

		private ProjectTemplate? _selectedProjectTemplate;
		public ProjectTemplate? SelectedProjectTemplate
		{
			get
			{
				return _selectedProjectTemplate;
			}
			set
			{
				_selectedProjectTemplate = value;
				ValidateProjectPath();
				SetProperty(ref _selectedProjectTemplate, value);
			}
		}
		#endregion Public Properties

		public CreateProject()
		{
			ProjectTemplates = new(_projectTemplates);
			try
			{
				string[] templateFiles = Directory.GetFiles(templatePath, "template.xml", SearchOption.AllDirectories);
				Debug.Assert(templateFiles.Any());
				foreach (string templateFile in templateFiles)
				{
					ProjectTemplate? template = Serializer.FromFile<ProjectTemplate>(templateFile);
					string templateFolder = Path.GetDirectoryName(templateFile) ?? "";
					if (string.IsNullOrEmpty(templateFolder) || template is null)
						continue;
					template.IconFilePath = Path.GetFullPath(Path.Combine(templateFolder, "icon.png"));
					template.Icon = File.ReadAllBytes(template.IconFilePath);
					template.DisplaySnippet = ProjectStore.GetDisplaySnippet(Path.GetDirectoryName(templateFile));
					template.ProjectFilePath = Path.GetFullPath(Path.Combine(templateFolder, template.ProjectFile));
					_projectTemplates.Add(template);
				}

				SelectedProjectTemplate = _projectTemplates.FirstOrDefault();
				ValidateProjectPath();
			}
			catch (Exception ex)
			{
				// TODO: log errors
				Debug.WriteLine(ex.Message);
			}
		}

		#region Commands
		[RelayCommand]
		/// <summary>
		/// Creates a new project on command click
		/// </summary>
		/// <param name="SelectedProjectTemplate">
		/// The project template to use
		/// </param>
		/// <returns>
		/// Returns the full path of the project file if successful, otherwise returns an empty string
		/// </returns>
		private void Create(IClosable window)
		{
			try
			{
				ValidateProjectPath();
				if (!IsPathValid) return;
				string path = Path.Combine(ProjectPath, ProjectName);
#pragma warning disable CS8604 // Possible null reference argument.
				Project project = new(ProjectName, path, SelectedProjectTemplate);
#pragma warning restore CS8604 // Possible null reference argument.
				if (string.IsNullOrEmpty(project.ProjectPath))
				{
					ErrorMsg = "Something went wrong while creating the project";
					throw new Exception(ErrorMsg);
				}

				ProjectStore.OpenProject(project.ProjectPath);
				Exit(window);
			}
			catch (Exception ex)
			{
				// TODO: log errors
				Debug.WriteLine(ex.Message);
			}
		}

		[RelayCommand]
		private void Exit(IClosable window)
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
		private bool ValidateProjectPath()
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
				return false;
			}
			else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				ErrorMsg = "Project name contains invalid characters";
			}
			else if (string.IsNullOrEmpty(ProjectPath.Trim()))
			{
				ErrorMsg = "Project path cannot be empty";
				return false;
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

			return IsPathValid;
		}
	}
}