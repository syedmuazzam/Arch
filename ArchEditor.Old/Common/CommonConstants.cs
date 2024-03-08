using System.IO;
using System.Xml.Linq;

namespace ArchEditor.Common
{
	static class CommonConstants
	{
		public const string ProjectExtension = ".archproj";
		public const string SceneExtension = ".archscene";

		public static readonly XNamespace ModelsNamespace = @"http://schemas.datacontract.org/2004/07/ArchEditor.Models";

		public static string GetDefaultScenesPath(string projectPath) => Path.Combine(projectPath, "Content", "Scenes");
	}
}
