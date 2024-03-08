using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArchEditor.Models
{
    [DataContract]
    internal class ProjectTemplate
    {
        [DataMember]
        public string? ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; } = "project.archproj";
        [DataMember]
        public List<string>? Folders { get; set; }
        [DataMember]
        public byte[]? Icon { get; set; }
        [DataMember]
        public byte[]? DisplaySnippet { get; set; }
        public string? IconFilePath { get; set; }
        public string? ProjectFilePath { get; set; }
    }
}
