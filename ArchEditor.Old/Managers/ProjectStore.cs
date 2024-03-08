using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Linq;
using System.Collections.Generic;

using ArchEditor.Models;
using ArchEditor.Common;
using ArchEditor.Common.Resources;
using ArchEditor.Utilities;
using ArchEditor.Workspace;

namespace ArchEditor.Managers
{
	static class ProjectStore
	{
		private static readonly string _applicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ArchEditor");
		private static readonly string _projectsListPath = Path.Combine(_applicationDataPath, "Projects.xml");

		/// <summary>
		/// Adds the project to the project list
		/// </summary>
		/// <param name="projectData">
		public static void AddToProjectList(ProjectListData projectData)
		{
			try
			{
				if (!Directory.Exists(_applicationDataPath)) Directory.CreateDirectory(_applicationDataPath);
				XDocument xProjectList = new();
				bool isProjectListValid = false;
				if (File.Exists(_projectsListPath))
				{
					xProjectList = XDocument.Load(_projectsListPath);
					string schemaString = CommonSchemas.ProjectsListSchema.ToString();
					XmlSchemaSet xProjectListSchemas = new();
					xProjectListSchemas.Add(CommonConstants.ModelsNamespace.ToString(), XmlReader.Create(new StringReader(schemaString)));
					isProjectListValid = true;
					xProjectList.Validate(xProjectListSchemas, (s, e) =>
					{
						// TODO: log the errors
						Debug.WriteLine(e.Message);
						isProjectListValid = false;
					});
				}

				if (isProjectListValid)
				{
					xProjectList.Descendants(CommonConstants.ModelsNamespace + "Projects").First().Add(Serializer.ToXmlElement(projectData));
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
				// TODO: log the errors
				Debug.WriteLine(ex.Message);
			}
		}

		public static List<ProjectListData> GetProjectList()
		{
			ProjectList? projectList = Serializer.FromFile<ProjectList>(_projectsListPath);
			if (projectList is null) return new List<ProjectListData>();
			List<ProjectListData> projects = projectList.Projects;
			return projects;
		}

		public static void OpenProject(string projectPath)
		{
			//Project? project = Serializer.FromFile<Project>(projectPath);
			//if (project is null) return;
			WorkspaceView workspaceView = new();
			workspaceView.Show();
		}

		public static byte[] GetDisplaySnippet(string projectPath)
		{
			return File.ReadAllBytes(Path.Combine(projectPath, "Snippet.png"));
		}
	}
}
