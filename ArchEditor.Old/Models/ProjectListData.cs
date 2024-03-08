using ArchEditor.Managers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArchEditor.Models
{
	[DataContract(Name ="Project")]
	class ProjectListData
	{
		[DataMember]
		public string ProjectName { get; set; }
		[DataMember]
		public string ProjectPath { get; set; }
		[DataMember]
		public DateTime Date { get; set; }
		public byte[] DisplaySnippet { get; set; }

		public ProjectListData(string projectName, string projectPath, DateTime date)
        {
			ProjectName = projectName;
			ProjectPath = projectPath;
			Date = date;
			DisplaySnippet = ProjectStore.GetDisplaySnippet(ProjectPath);
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			DisplaySnippet = ProjectStore.GetDisplaySnippet(ProjectPath);
		}

    }

	[DataContract]
	class ProjectList
	{
		[DataMember]
		public List<ProjectListData> Projects { get; set; } = new();
	}
}
