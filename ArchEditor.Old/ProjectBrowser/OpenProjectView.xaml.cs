using System.Windows.Controls;
using System.Windows.Input;

namespace ArchEditor.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for OpenProjectView.xaml
    /// </summary>
    public partial class OpenProjectView : UserControl
    {
        public OpenProjectView()
        {
            InitializeComponent();
        }

		private void OnOpenProjectLoaded(object sender, System.Windows.RoutedEventArgs e)
		{
			FocusManager.SetFocusedElement(this, ProjectsListBox);
			Keyboard.Focus(ProjectsListBox);
		}
	}
}
