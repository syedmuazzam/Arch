using ArchEditor.Stores;

using CommunityToolkit.Mvvm.ComponentModel;

namespace ArchEditor.ViewModels;

internal partial class ProjectBrowserViewModel : ObservableObject
{
    private readonly ProjectStore _projectStore;

    #region Public Properties
    public CreateProjectViewModel CreateProjectViewModel { get; }
    public OpenProjectViewModel OpenProjectViewModel { get; }

    [ObservableProperty] private string? _projectText;
    #endregion Public Properties

    public ProjectBrowserViewModel(OpenProjectViewModel openProjectViewModel, CreateProjectViewModel createProjectViewModel, ProjectStore projectStore)
    {
        OpenProjectViewModel = openProjectViewModel;
        CreateProjectViewModel = createProjectViewModel;
        _projectStore = projectStore;
        ProjectText = "abc";
    }

    public string GetProjectToOpen() => _projectStore.ProjectToOpen;
    public void SetProjectToOpen(string projectToOpen) => _projectStore.ProjectToOpen = projectToOpen;
    #region Commands
    public void OnWindowClose()
    {
        _projectStore.CloseProject();
    }
    #endregion Commands
}
