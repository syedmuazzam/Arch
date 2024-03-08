using ArchEditor.Stores;
using ArchEditor.ViewModels.EditorViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ArchEditor.ViewModels;

internal partial class WorkspaceViewModel : ObservableObject
{
	[ObservableProperty] private ObservableObject _upperLeftViewModel;
	[ObservableProperty] private ObservableObject _upperRightViewModel;
	[ObservableProperty] private string _statusMessage = string.Empty;

	private readonly UndoRedoStore _undoRedoStore;
	private readonly ProjectStore _projectStore;

	public WorkspaceViewModel(InspectorViewModel upperRightViewModel, ProjectHierarchyViewModel upperLeftViewModel, ProjectStore projectStore, UndoRedoStore undoRedoStore)
	{
		_upperRightViewModel = upperRightViewModel;
		_upperLeftViewModel = upperLeftViewModel;
		_projectStore = projectStore;
		_undoRedoStore = undoRedoStore;
	}

	#region Commands
	[RelayCommand]
	public void Undo()
	{
		_undoRedoStore.Undo();
	}

	[RelayCommand]
	public void Redo()
	{
		_undoRedoStore.Redo();
	}

	[RelayCommand]
	public void Save()
	{
		StatusMessage = "Saving project...";
		_projectStore.SaveProject();
		StatusMessage = "Project saved!";
	}
	#endregion
}