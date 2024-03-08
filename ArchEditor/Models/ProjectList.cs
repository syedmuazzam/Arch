using ArchEditor.Common;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ArchEditor.Models;

[DataContract(Name = "Project")]
internal class ProjectListData
{
	[DataMember] public string ProjectName { get; set; }
	[DataMember] public string ProjectPath { get; set; }
	[DataMember] public DateTime Date { get; set; }
	public string FullPath => Path.Combine(ProjectPath, ProjectName + CommonConstants.ProjectExtension);

	public byte[] DisplaySnippet { get; set; }

	public ProjectListData(string projectName, string projectPath, DateTime date)
	{
		ProjectName = projectName;
		ProjectPath = projectPath;
		Date = date;
		DisplaySnippet = File.ReadAllBytes(Path.Combine(projectPath, "Snippet.png"));
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		DisplaySnippet = File.ReadAllBytes(Path.Combine(ProjectPath, "Snippet.png"));
	}
}

[DataContract]
class ProjectList
{
	[DataMember] public ObservableCollection<ProjectListData> Projects { get; set; } = new();
}
