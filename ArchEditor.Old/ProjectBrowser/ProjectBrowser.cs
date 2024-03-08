using ArchEditor.Common;

namespace ArchEditor.ProjectBrowser
{
	internal class ProjectBrowser : ViewModelBase
	{
		#region Public Properties
		private string? projectText;
		public string? ProjectText
		{
			get => projectText;
			set
			{
				projectText = value;
				OnPropertyChanged(nameof(ProjectText));
			}
		}
		#endregion Public Properties

		public ProjectBrowser() => projectText = "abc";
	}
}
