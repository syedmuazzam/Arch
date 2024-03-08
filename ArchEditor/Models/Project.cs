using ArchEditor.Common;
using ArchEditor.Utilities;

using Serilog;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace ArchEditor.Models;

[DataContract(Name = "Game")]
internal class Project
{
	private string _defaultScenesPath;
	#region Public Properties
	[DataMember] public string Name { get; set; }
	[DataMember] public string ProjectPath { get; set; }
	public string FullPath => Path.Combine(ProjectPath, Name + CommonConstants.ProjectExtension);
	[DataMember] public byte[]? DisplaySnippet { get; set; }
	[DataMember(Name = "Scenes")] private readonly ObservableCollection<Scene> _scenes = new();
	public ReadOnlyObservableCollection<Scene> Scenes { get; private set; }
	[DataMember] public Scene StartupScene { get; set; }
	[DataMember] public Scene ActiveScene { get; set; }
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
	/// <param name="projectTemplate">
	/// The project template to use
	/// </param>
	public Project(string name, string projectPath, ProjectTemplate projectTemplate)
	{
		Name = name;
		ProjectPath = projectPath;
		DisplaySnippet = projectTemplate.DisplaySnippet;
		Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
		_defaultScenesPath = CommonConstants.GetDefaultScenesPath(ProjectPath);
		StartupScene = new Scene("SampleScene", _defaultScenesPath);
		_scenes.Add(StartupScene);
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

			File.Copy(Path.Combine(Path.GetDirectoryName(projectTemplate.ProjectFilePath)!, "Snippet.png"), Path.Combine(ProjectPath, "Snippet.png"));
			if (!Directory.Exists(_defaultScenesPath)) Directory.CreateDirectory(_defaultScenesPath);
			Serializer.ToFile(this, FullPath);
		}
		catch (Exception ex)
		{
			Log.Error("{ex.Message}",ex.Message);
			throw;
		}
	}

	[OnDeserialized]
	// ReSharper disable once UnusedParameter.Local
	private void OnDeserialized(StreamingContext context)
	{
		_defaultScenesPath = CommonConstants.GetDefaultScenesPath(ProjectPath);
		Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
		DisplaySnippet = File.ReadAllBytes(Path.Combine(ProjectPath, "Snippet.png"));
	}

	/// <summary>
	/// Adds a scene to the project
	/// </summary>
	/// <param name="name">
	/// The name of the scene
	/// </param>
	/// <param name="path">
	/// The path to the scene. If not specified, the default scenes path will be used
	/// </param>
	public Scene AddScene(string name, string path = "")
	{
		Debug.Assert(!string.IsNullOrEmpty(name));
		if (string.IsNullOrEmpty(path)) path = _defaultScenesPath;
		Scene scene = new(name, path);
		_scenes.Add(scene);
		return scene;
	}

	/// <summary>
	/// Inserts a scene into the project at the specified index
	/// </summary>
	/// <param name="scene">
	/// The scene to insert
	/// </param>
	/// <param name="index">
	/// The index at which to insert the scene
	/// </param>
	public void InsertScene(Scene scene, int index)
	{
		Debug.Assert(scene is not null);
		_scenes.Insert(index, scene);
	}

	/// <summary>
	/// Removes a scene from the project
	/// </summary>
	/// <param name="scene">
	/// The scene to remove
	/// </param>
	public void RemoveScene(Scene scene)
	{
		Debug.Assert(_scenes.Contains(scene));
		_scenes.Remove(scene);
	}
}
