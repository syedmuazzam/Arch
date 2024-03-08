using ArchEditor.Models;
using ArchEditor.Stores;
using ArchEditor.Stores.Messages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using System.Collections.ObjectModel;

namespace ArchEditor.ViewModels.EditorViewModels;

internal partial class InspectorViewModel : ObservableObject, IRecipient<GameObjectsSelectionChangedMessage>
{
	[ObservableProperty]
	private GameObject? _gameObject;

	public string? Name { get; private set; }
	public Transform? Transform { get; private set; }
	public ReadOnlyObservableCollection<Component>? Components { get; private set; }

	private readonly UndoRedoStore _undoRedoStore;
	public InspectorViewModel(UndoRedoStore undoRedoStore)
	{
		_undoRedoStore = undoRedoStore;
		StrongReferenceMessenger.Default.Register(this);
	}

	public void Receive(GameObjectsSelectionChangedMessage message)
	{
		GameObject = message.Value;
		Name = GameObject.Name;
	}

	#region Commands
	[RelayCommand]
	private void AddComponent()
	{
		GameObject?.AddComponent();
	}

	[RelayCommand]
	private void UpdateGameObjectName(string newGameObjectName)
	{
		string oldGameObjectName = GameObject!.Name;
		GameObject!.Name = newGameObjectName;
		_undoRedoStore.AddUndoAction(new UndoRedoAction(
			"Update Game Object Name",
			() => GameObject.Name = oldGameObjectName,
			() => GameObject.Name = newGameObjectName
			));
	}
	#endregion
}
