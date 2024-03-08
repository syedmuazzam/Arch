using ArchEditor.Common;
using ArchEditor.Models;
using ArchEditor.Utilities;

using Serilog;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ArchEditor.Stores;

/// <summary>
/// The project store is responsible for managing the projects
/// </summary>
internal class ProjectStore
{
	private readonly string _projectsListPath = Path.Combine(CommonConstants.ApplicationDataPath, "Projects.xml");
	private readonly XNamespace _modelsNamespace = @"http://schemas.datacontract.org/2004/07/ArchEditor.Models";
	
	public Project? CurrentProject { get; set; }
	public string ProjectToOpen { get; set; } = string.Empty;

	/// <summary>
	/// Opens a project from the given path
	/// </summary>
	/// <param name="projectPath">
	/// The path to the project
	/// </param>
	public void OpenProject(string projectPath)
	{
		CurrentProject = Serializer.FromFile<Project>(projectPath);
		if (CurrentProject is null) return;
		ProjectToOpen = projectPath;
	}

	/// <summary>
	/// Closes the current project
	/// </summary>
	public void CloseProject()
	{
	}

	/// <summary>
	/// Creates a new project
	/// </summary>
	/// <param name="projectName"></param>
	/// <param name="projectPath"></param>
	/// <param name="projectTemplate"></param>
	/// <returns></returns>
	public Project CreateProject(string projectName, string projectPath, ProjectTemplate projectTemplate)
	{
		Project project = new(projectName, projectPath, projectTemplate);
		AddToProjectList(new ProjectListData(projectName, projectPath, DateTime.Now));
		return project;
	}

	/// <summary>
	/// Saves the project. If no project is given, the current project is saved
	/// </summary>
	/// <param name="project"></param>
	public void SaveProject(Project? project = null)
	{
		project ??= CurrentProject;
		if (project is null) return;
		Serializer.ToFile(project, project.FullPath);
	}

	/// <summary>
	/// Adds a project to the project list
	/// </summary>
	/// <param name="projectData"></param>
	public void AddToProjectList(ProjectListData projectData)
	{
		try
		{
			if (!Directory.Exists(CommonConstants.ApplicationDataPath)) Directory.CreateDirectory(CommonConstants.ApplicationDataPath);
			XDocument xProjectList = new();
			bool isProjectListValid = false;
			if (File.Exists(_projectsListPath))
			{
				xProjectList = XDocument.Load(_projectsListPath);
				string schemaString = CommonSchemas.ProjectsListSchema;
				XmlSchemaSet xProjectListSchemas = new();
				xProjectListSchemas.Add(_modelsNamespace.ToString(), XmlReader.Create(new StringReader(schemaString)));
				isProjectListValid = true;
				xProjectList.Validate(xProjectListSchemas, (_, e) =>
				{
					Log.Error("{e.Message}", e.Message);
					isProjectListValid = false;
				});
			}

			if (isProjectListValid)
			{
				xProjectList.Descendants(_modelsNamespace + "Projects").First().Add(Serializer.ToXmlElement(projectData));
				xProjectList.Save(_projectsListPath);
			}
			else
			{
				ProjectList projectList = new();
				projectList.Projects.Add(projectData);
				Serializer.ToFile(projectList, _projectsListPath);
			}
		}
		catch (Exception ex)
		{
			Log.Error("{ex.Message}", ex.Message);
			throw;
		}
	}

	/// <summary>
	/// Gets the list of projects
	/// </summary>
	/// <returns></returns>
	public List<ProjectListData> GetProjectList()
	{
		ProjectList? projectList = Serializer.FromFile<ProjectList>(_projectsListPath);
		if (projectList is null) return new List<ProjectListData>();
		List<ProjectListData> projects = new(projectList.Projects);
		return projects;
	}
}
