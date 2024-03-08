using ArchEditor.ProjectBrowser;
using System.Windows;

namespace ArchEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ProjectBrowserDialogView projectBrowserDialogView = new();
            projectBrowserDialogView.Show();

            base.OnStartup(e);
        }
    }
}
