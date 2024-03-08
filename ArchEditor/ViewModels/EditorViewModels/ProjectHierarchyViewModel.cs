using ArchEditor.Models;
using ArchEditor.Stores;
using ArchEditor.Stores.Messages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ArchEditor.ViewModels.EditorViewModels;

internal partial class ProjectHierarchyViewModel : ObservableObject
{
	private readonly UndoRedoStore _undoRedoStore;
	[ObservableProperty]
	private ProjectStore _projectStore;
	[ObservableProperty]
	private object? _selectedItem;
	[ObservableProperty]
	private GameObject? _selectedGameObject;

	public ProjectHierarchyViewModel(ProjectStore projectStore, UndoRedoStore undoRedoStore)
	{
		_projectStore = projectStore;
		_undoRedoStore = undoRedoStore;
	}

	#region Commands
	[RelayCommand]
	public void AddScene()
	{
		if (ProjectStore.CurrentProject is null) return;
		Scene newScene = ProjectStore.CurrentProject.AddScene("NewScene" + ProjectStore.CurrentProject.Scenes.Count);
		int sceneIndex = ProjectStore.CurrentProject.Scenes.IndexOf(newScene);
		_undoRedoStore.AddUndoAction(new UndoRedoAction(
			$"Add Scene {newScene.Name}",
			() => ProjectStore.CurrentProject?.RemoveScene(newScene),
			() => ProjectStore.CurrentProject?.InsertScene(newScene, sceneIndex)
		));
	}

	[RelayCommand]
	public void RemoveScene(Scene selectedScene)
	{
		if (ProjectStore.CurrentProject is null) return;
		int sceneIndex = ProjectStore.CurrentProject.Scenes.IndexOf(selectedScene);
		ProjectStore.CurrentProject?.RemoveScene(selectedScene);
		_undoRedoStore.AddUndoAction(new UndoRedoAction(
			$"Remove Scene {selectedScene.Name}",
			() => ProjectStore.CurrentProject?.InsertScene(selectedScene, sceneIndex),
			() => ProjectStore.CurrentProject?.RemoveScene(selectedScene)
			));
	}

	[RelayCommand]
	public void AddGameObject(Scene selectedScene)
	{
		GameObject newGameObject = selectedScene.AddGameObject();
		int gameObjectIndex = selectedScene.GameObjects.IndexOf(newGameObject);
		_undoRedoStore.AddUndoAction(new UndoRedoAction(
			$"Add Game Object {newGameObject.Name}",
			() => selectedScene.RemoveGameObject(newGameObject),
			() => selectedScene.InsertGameObject(newGameObject, gameObjectIndex)
			));
	}

	[RelayCommand]
	public void RemoveGameObject(GameObject selectedGameObject)
	{
		int gameObjectIndex = selectedGameObject.ParentScene.GameObjects.IndexOf(selectedGameObject);
		selectedGameObject.ParentScene.RemoveGameObject(selectedGameObject);
		_undoRedoStore.AddUndoAction(new UndoRedoAction(
			$"Remove Game Object {selectedGameObject.Name}",
			() => selectedGameObject.ParentScene.InsertGameObject(selectedGameObject, gameObjectIndex),
			() => selectedGameObject.ParentScene.RemoveGameObject(selectedGameObject)
			));
	}

	[RelayCommand]
	public static void SelectedItemChanged(object? selectedItem)
	{
		if (selectedItem is GameObject gameObject)
		{
			StrongReferenceMessenger.Default.Send(new GameObjectsSelectionChangedMessage(gameObject));
		}
	}
	#endregion
}