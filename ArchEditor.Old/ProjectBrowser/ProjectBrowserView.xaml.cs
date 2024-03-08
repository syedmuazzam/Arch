using ArchEditor.Common.Interfaces;
using System.Windows;

namespace ArchEditor.ProjectBrowser
{
    /// <summary>
    /// Interaction logic for ProjectBrowserDialog.xaml
    /// </summary>
    public partial class ProjectBrowserDialogView : Window, IClosable
    {
        public ProjectBrowserDialogView()
        {
            InitializeComponent();
        }
    }
}
