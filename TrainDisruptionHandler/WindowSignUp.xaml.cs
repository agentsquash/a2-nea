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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrainDisruptionHandler
{
	/// <summary>
	/// Interaction logic for WindowSignUp.xaml
	/// </summary>
	public partial class WindowSignUp : Window
	{
		private static Dictionary<string, Label> labels = new Dictionary<string, Label>();
		private static Dictionary<string, Button> buttons = new Dictionary<string, Button>();
		private static Dictionary<string, PasswordBox> pwBoxes = new Dictionary<string, PasswordBox>();
		private static Dictionary<string, TextBox> txtBoxes = new Dictionary<string, TextBox>();

		public WindowSignUp()
		{
			InitializeComponent();

			// Construct signup page
			labels["username"] = UtilsGUI.CreateLabel("Username:", "username");
			StackPanelSignUp.Children.Add(labels["username"]);
			txtBoxes["username"] = UtilsGUI.CreateTextBox("Username");
			StackPanelSignUp.Children.Add(txtBoxes["username"]);

			labels["email"] = UtilsGUI.CreateLabel("Email Address:", "email");
			StackPanelSignUp.Children.Add(labels["email"]);
			txtBoxes["email"] = UtilsGUI.CreateTextBox("email");
			StackPanelSignUp.Children.Add(txtBoxes["email"]);

			labels["password"] = UtilsGUI.CreateLabel("Password:", "password");
			StackPanelSignUp.Children.Add(labels["password"]);
			pwBoxes["password"] = UtilsGUI.CreatePasswordBox("password");
			StackPanelSignUp.Children.Add(pwBoxes["password"]);
			labels["confirm_pass"] = UtilsGUI.CreateLabel("Confirm Password:", "confirm_pass");
			StackPanelSignUp.Children.Add(labels["confirm_pass"]);
			pwBoxes["confirm_pass"] = UtilsGUI.CreatePasswordBox("confirm_pass");
			StackPanelSignUp.Children.Add(pwBoxes["confirm_pass"]);

			buttons["signup"] = UtilsGUI.CreateButton("Sign Up", "signup");
			StackPanelSignUp.Children.Add(buttons["signup"]);

			labels["login"] = UtilsGUI.CreateLabel("Already have an account? Sign in:", "login");
			StackPanelSignUp.Children.Add(labels["login"]);
			buttons["login"] = UtilsGUI.CreateButton("Sign In", "login");
			StackPanelSignUp.Children.Add(buttons["login"]);

			buttons["login"].Click += new RoutedEventHandler(Btn_login_Click);
			buttons["signup"].Click += new RoutedEventHandler(Btn_signup_Click);
		}

		void Btn_login_Click(object sender, RoutedEventArgs e)
		{
			WindowMain windowMain = new WindowMain();
			windowMain.Show();
			this.Close();
		}

		void Btn_signup_Click(object sender, RoutedEventArgs e)
		{
			// Validate username
			bool usernameValid = UtilsAuth.UsernameValidation(txtBoxes["username"].Text);
			bool emailValid = false;
			bool passwordValid = false;

			if (!usernameValid)
				MessageBox.Show("Invalid username entered.", "Sign Up Failed", MessageBoxButton.OK, MessageBoxImage.Error);

			// Validate email address
			if (usernameValid)
			{
				emailValid = UtilsAuth.EmailValidation(txtBoxes["email"].Text);
				if (!emailValid)
					MessageBox.Show("Invalid email entered.", "Sign Up Failed", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			// Validate password
			if (emailValid)
			{
				passwordValid = UtilsAuth.PasswordValidation(pwBoxes["password"].Password, pwBoxes["confirm_pass"].Password);
				if (!passwordValid)
					MessageBox.Show("Invalid password.", "Sign Up Failed", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			// Create account
			if (passwordValid)
			{
				UtilsDB.CreateLocalAccount(txtBoxes["username"].Text, txtBoxes["email"].Text, UtilsAuth.PasswordHash(pwBoxes["password"].Password));
				MessageBox.Show("Account created!\nYou will now be returned to the login screen.", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
				WindowMain loginWindow = new WindowMain();
				loginWindow.Show();
				this.Close();
			}
			
		}
	}
}
