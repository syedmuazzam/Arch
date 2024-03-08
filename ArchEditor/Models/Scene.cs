using ArchEditor.Common;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace ArchEditor.Models;

[DataContract(IsReference = true)]
internal class Scene
{
	#region Public Properties
	[DataMember] public string Name { get; set; }
	[DataMember] public string ScenePath { get; set; }
	public string FullPath => Path.Combine(ScenePath, Name + CommonConstants.SceneExtension);
	[DataMember(Name = nameof(GameObjects))] private readonly ObservableCollection<GameObject> _gameObjects = new();
	public ReadOnlyObservableCollection<GameObject> GameObjects { get; private set; }
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
		GameObjects = new ReadOnlyObservableCollection<GameObject>(_gameObjects);
	}

	[OnDeserialized]
	// ReSharper disable once UnusedParameter.Local
	private void OnDeserialized(StreamingContext context)
	{
		GameObjects = new ReadOnlyObservableCollection<GameObject>(_gameObjects);
	}

	/// <summary>
	/// Adds a new game object to the scene
	/// </summary>
	/// <param name="name">
	/// The name of the game object. If null or empty, the name will be "NewGameObject" followed by the number of game objects in the scene
	/// </param>
	public GameObject AddGameObject(string name = "")
	{
		if (string.IsNullOrEmpty(name)) name = $"NewGameObject {_gameObjects.Count}";
		Debug.Assert(!string.IsNullOrEmpty(name));
		GameObject gameObject = new(name, this);
		_gameObjects.Add(gameObject);
		return gameObject;
	}

	/// <summary>
	/// Inserts a game object into the scene
	/// </summary>
	/// <param name="gameObject">
	/// The game object to insert
	/// </param>
	/// <param name="index">
	/// The index to insert the game object at
	/// </param>
	public void InsertGameObject(GameObject gameObject, int index)
	{
		Debug.Assert(gameObject is not null);
		_gameObjects.Insert(index, gameObject);
	}

	/// <summary>
	/// Removes a game object from the scene
	/// </summary>
	/// <param name="gameObject">
	/// The game object to remove
	/// </param>
	public void RemoveGameObject(GameObject gameObject)
	{
		Debug.Assert(_gameObjects.Contains(gameObject));
		_gameObjects.Remove(gameObject);
	}
}
