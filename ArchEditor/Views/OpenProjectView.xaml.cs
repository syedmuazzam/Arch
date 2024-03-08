using System.Windows.Controls;

namespace ArchEditor.Views
{
    /// <summary>
    /// Interaction logic for OpenProjectView.xaml
    /// </summary>
    public partial class OpenProjectView
    {
        public OpenProjectView()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                ListBoxItem? item =
                    ProjectsListBox.ItemContainerGenerator.ContainerFromIndex(ProjectsListBox.SelectedIndex) as
                        ListBoxItem;
                item?.Focus();
            };
        }
    }
}
