using System.IO;
using System.Runtime.Serialization;

using ArchEditor.Common;

namespace ArchEditor.Models
{
	[DataContract(IsReference = true)]
	class Scene
	{
		#region Public Properties
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string ScenePath { get; set; }
		public string FullPath => Path.Combine(ScenePath, Name + CommonConstants.SceneExtension);
		#endregion Public Properties

		/// <summary>
		/// Creates a new scene
		/// </summary>
		/// <param name="name">
		/// The name of the scene
		/// </param>
		/// <param name="scenePath">
		/// The path to the scene
		/// </param>
		public Scene(string name, string scenePath)
		{
			Name = name;
			ScenePath = scenePath;
		}
	}
}
