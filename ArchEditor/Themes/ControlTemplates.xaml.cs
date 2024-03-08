using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ArchEditor.Themes;

public partial class ControlTemplates
{
	private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
	{
		TextBox textBox = (TextBox)sender;
		BindingExpression? expression = textBox.GetBindingExpression(TextBox.TextProperty);
		switch (e.Key)
		{
			case Key.Enter:
				expression?.UpdateSource();
				e.Handled = true;
				Keyboard.ClearFocus();
				break;
			case Key.Escape:
				expression?.UpdateTarget();
				Keyboard.ClearFocus();
				break;
		}
	}
}
