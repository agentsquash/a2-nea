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
using System.ComponentModel;
using Microsoft.Win32;

namespace TrainDisruptionHandler
{
	/// <summary>
	/// Interaction logic for WindowAdminPanel.xaml
	/// </summary>
	///
	public partial class WindowAdminPanel : Window
	{
		private static Dictionary<string, Label> labels = new Dictionary<string, Label>();
		private static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
		private static Dictionary<string, TextBox> txtBoxes = new Dictionary<string, TextBox>();
		private BackgroundWorker worker;
		private ProgressBar progressBar;

		public WindowAdminPanel()
		{
			InitializeComponent();

			labels["import"] = UtilsGUI.CreateLabel("Import Data from CSV", "import");
			StackPanelAdmin.Children.Add(labels["import"]);
			txtBoxes["file"] = UtilsGUI.CreateTextBox("file");
			StackPanelAdmin.Children.Add(txtBoxes["file"]);
			buttons["file_dialog"] = UtilsGUI.CreateButton("Select CSV File", "file_dialog");
			StackPanelAdmin.Children.Add(buttons["file_dialog"]);
			buttons["station_data"] = UtilsGUI.CreateButton("Import Station Data", "station_data");
			StackPanelAdmin.Children.Add(buttons["station_data"]);
			buttons["tiploc"] = UtilsGUI.CreateButton("Import Rail References", "tiploc");
			StackPanelAdmin.Children.Add(buttons["tiploc"]);
			buttons["connections"] = UtilsGUI.CreateButton("Import Connection Data", "connections");
			StackPanelAdmin.Children.Add(buttons["connections"]);
			buttons["fixed_links"] = UtilsGUI.CreateButton("Import Fixed Links", "fixed_links");
			StackPanelAdmin.Children.Add(buttons["fixed_links"]);
			buttons["reset"] = UtilsGUI.CreateButton("Reset Stations Data", "reset");
			StackPanelAdmin.Children.Add(buttons["reset"]);
			labels["import"] = UtilsGUI.CreateLabel("Importing...", "import");

			buttons["file_dialog"].Click += new RoutedEventHandler(Btn_File_Dialog_Click);
			buttons["station_data"].Click += new RoutedEventHandler(Btn_Station_Data_Click);
			buttons["tiploc"].Click += new RoutedEventHandler(Btn_Tiploc_Click);
			buttons["connections"].Click += new RoutedEventHandler(Btn_Connection_Click);
			buttons["fixed_links"].Click += new RoutedEventHandler(Btn_Fixed_Links_Click);
			buttons["reset"].Click += new RoutedEventHandler(Btn_Reset_Click);
			progressBar = new ProgressBar()
			{
				Name = "Bar_Progress",
				IsIndeterminate = true,
				Orientation = Orientation.Horizontal
			};

			CheckStationsGUITools();
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

		private void Btn_Station_Data_Click(object sender, RoutedEventArgs e)
		{
			worker = new BackgroundWorker();
			worker.DoWork += WorkerStationsData_DoWork;
			worker.RunWorkerCompleted += WorkerStationsData_RunWorkerCompleted;
			DisableStationsTools("Station Data");
			worker.RunWorkerAsync(txtBoxes["file"].Text);
		}

		private void Btn_Tiploc_Click(object sender, RoutedEventArgs e)
		{
			// Execute
			worker = new BackgroundWorker();
			worker.DoWork += WorkerRailRef_DoWork;
			worker.RunWorkerCompleted += WorkerRailRef_RunWorkerCompleted;
			DisableStationsTools("TIPLOC data");
			worker.RunWorkerAsync(txtBoxes["file"].Text);
		}

		private void Btn_Connection_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Btn_Fixed_Links_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Btn_Reset_Click(object sender, RoutedEventArgs e)
		{
			UtilsDB.ResetStationData();
			CheckStationsGUITools();
		}

		private void WorkerStationsData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if ((bool)e.Result)
				MessageBox.Show("Successfully imported station code data.", "Import Success", MessageBoxButton.OK, MessageBoxImage.Information);
			else
			{
				UtilsDB.ResetStationData();
				MessageBox.Show("Station data import failed.\nAre you using the correct file?", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			CheckStationsGUITools();
		}

		private void WorkerStationsData_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				e.Result = UtilsDB.ConvertStationCodes((string)e.Argument);
			}
			catch
			{
				e.Result = false;
			}
		}

		private void WorkerRailRef_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if ((bool)e.Result)
				MessageBox.Show("Successfully imported TIPLOC data.", "Import Success", MessageBoxButton.OK, MessageBoxImage.Information);
			else
				MessageBox.Show("TIPLOC data import failed.\nAre you using the correct file?", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void WorkerRailRef_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				e.Result = UtilsDB.ConvertRailReferences((string)e.Argument);
			}
			catch
			{
				e.Result = false;
			}
		}

		private void CheckStationsGUITools()
		{
			buttons["file_dialog"].IsEnabled = true;
			StackPanelAdmin.Children.Remove(labels["import"]);
			StackPanelAdmin.Children.Remove(progressBar);
			if (UtilsDB.CheckStationDataExists())
			{
				EnableStationsAdditionalTools();
			}
			else
			{
				DisableStationsAdditionalTools();
			}
		}
		
		private void EnableStationsAdditionalTools()
		{
			buttons["station_data"].IsEnabled = false;
			buttons["tiploc"].IsEnabled = true;
			buttons["connections"].IsEnabled = true;
			buttons["fixed_links"].IsEnabled = true;
			buttons["reset"].IsEnabled = true;
		}

		private void DisableStationsAdditionalTools()
		{
			buttons["station_data"].IsEnabled = true;
			buttons["tiploc"].IsEnabled = false;
			buttons["connections"].IsEnabled = false;
			buttons["fixed_links"].IsEnabled = false;
			buttons["reset"].IsEnabled = false;
		}

		private void DisableStationsTools(string importType)
		{
			string userDisplay = "Importing " + importType + "...";
			labels["import"].Content = userDisplay;
			StackPanelAdmin.Children.Add(labels["import"]);
			StackPanelAdmin.Children.Add(progressBar);
		}
	}
}
