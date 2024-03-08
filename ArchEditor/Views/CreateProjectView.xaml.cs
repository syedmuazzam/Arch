using System.Windows.Controls;

namespace ArchEditor.Views;

/// <summary>
/// Interaction logic for CreateProjectView.xaml
/// </summary>
public partial class CreateProjectView
{
	public CreateProjectView()
	{
		InitializeComponent();

        Loaded += (_, _) =>
        {
            ListBoxItem? item =
                ProjectTemplatesListBox.ItemContainerGenerator.ContainerFromIndex(ProjectTemplatesListBox.SelectedIndex)
                    as ListBoxItem;
            item?.Focus();
        };

    }
}
