using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace TrainDisruptionHandler
{
	/// <summary>
	/// Interaction logic for WindowAdminPanel.xaml
	/// </summary>
	public partial class WindowAdminPanel : Window
	{
		private static Dictionary<string, Label> labels = new Dictionary<string, Label>();
		private static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
		private static Dictionary<string, TextBox> txtBoxes = new Dictionary<string, TextBox>();

		public WindowAdminPanel()
		{
			InitializeComponent();

			labels["import"] = UtilsGUI.CreateLabel("Import Data from CSV", "import");
			StackPanelAdmin.Children.Add(labels["import"]);
			txtBoxes["file"] = UtilsGUI.CreateTextBox("file");
			StackPanelAdmin.Children.Add(txtBoxes["file"]);
			buttons["file_dialog"] = UtilsGUI.CreateButton("Import CSV", "file_dialog");
			StackPanelAdmin.Children.Add(buttons["file_dialog"]);
			buttons["railreferences"] = UtilsGUI.CreateButton("Import Rail References", "railreferences");
			StackPanelAdmin.Children.Add(buttons["railreferences"]);

			buttons["file_dialog"].Click += new RoutedEventHandler(Btn_File_Dialog_Click);
			buttons["railreferences"].Click += new RoutedEventHandler(Btn_Railref_Click);
		}

		private void Btn_File_Dialog_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.DefaultExt = ".csv";
			dlg.Filter = "CSV file (*.csv)|*.csv";

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true)
				txtBoxes["file"].Text = dlg.FileName;
		}

		private void Btn_Railref_Click(object sender, RoutedEventArgs e)
		{ 
			// Execute
			bool success = UtilsDB.ConvertRailReferences(txtBoxes["file"].Text);
			if (success)
				MessageBox.Show("Successfully imported station data.","Import Success",MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("Station data import failed.\nAre you using the correct file?", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
