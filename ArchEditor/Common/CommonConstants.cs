using System;
using System.IO;

namespace ArchEditor.Common;

internal static class CommonConstants
{
	public const string ProjectExtension = ".archproj";
	public const string SceneExtension = ".archscene";

	public static readonly string ApplicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ArchEditor");
	//public static readonly XNamespace ModelsNamespace = @"http://schemas.datacontract.org/2004/07/ArchEditor.Models";

	public static string GetDefaultScenesPath(string projectPath) => Path.Combine(projectPath, "Content", "Scenes");
}
