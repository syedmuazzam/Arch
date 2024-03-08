using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Schema;
using ArchEditor.Common;
using ArchEditor.Managers;
using ArchEditor.Utilities;

namespace ArchEditor.Models
{
	[DataContract(Name = "Game")]
	class Project
	{
		#region Public Properties
		[DataMember]
		public string Name { get; set; }
		public string ProjectPath { get; set; }
		public string FullPath => Path.Combine(ProjectPath, Name + CommonConstants.ProjectExtension);
		[DataMember]
		public byte[]? DisplaySnippet { get; set; }
		[DataMember(Name = "Scenes")]
		private ObservableCollection<Scene> _scenes = new();
		public ReadOnlyObservableCollection<Scene> Scenes { get; }
		[DataMember]
		public Scene StartupScene { get; set; }
		[DataMember]
		public Scene ActiveScene { get; set; }
		#endregion Public Properties

		/// <summary>
		/// Creates a new project
		/// </summary>
		/// <param name="name">
		/// The name of the project
		/// </param>
		/// <param name="projectPath">
		/// The path to the project
		/// </param>
		public Project(string name, string projectPath, ProjectTemplate projectTemplate)
		{
			Name = name;
			ProjectPath = projectPath;
			DisplaySnippet = projectTemplate.DisplaySnippet;
			Scenes = new(_scenes);
			string defaultScenesPath = CommonConstants.GetDefaultScenesPath(ProjectPath);
			StartupScene = new Scene("Default", defaultScenesPath);
			ActiveScene = StartupScene;
			try
			{
				if (!Directory.Exists(projectPath)) Directory.CreateDirectory(projectPath);
				if (projectTemplate.Folders is not null)
				{
					foreach (string? folder in projectTemplate.Folders)
						Directory.CreateDirectory(Path.Combine(projectPath, folder));
					DirectoryInfo directoryInfo = new(Path.Combine(projectPath, ".Arch"));
					directoryInfo.Attributes |= FileAttributes.Hidden;
				}

				File.Copy(Path.Combine(Path.GetDirectoryName(projectTemplate.ProjectFilePath), "Snippet.png"), Path.Combine(ProjectPath, "Snippet.png"));
				if (!Directory.Exists(defaultScenesPath)) Directory.CreateDirectory(defaultScenesPath);
				_scenes.Add(new("Default", defaultScenesPath));
				XDocument? projectXml = Serializer.ToXmlDoc(this);
				if (projectXml is null)
				{
					ProjectPath = string.Empty;
					return;
				}

				// adding the default scenes path to the project xml
				string ns = @"{http://schemas.datacontract.org/2004/07/ArchEditor.Models}";
				projectXml.Descendants(ns + "Scenes").Descendants(ns + "Scene").First()
					.Add(new XElement(ns + "Path", defaultScenesPath));
				projectXml.Save(FullPath);
				ProjectStore.AddToProjectList(new(Name, ProjectPath, DateTime.Now));
			}
			catch (Exception ex)
			{
				// TODO: log errors
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
