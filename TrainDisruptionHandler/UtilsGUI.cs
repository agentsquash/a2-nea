using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TrainDisruptionHandler
{
	class UtilsGUI
	{ 
		public static Button CreateButton(string Display, string Name)
		{
			Button button = new Button
			{
				Content = Display,
				Name = "Btn_" + Name
			};
			return button;
		}

		public static PasswordBox CreatePasswordBox (string Name)
		{
			PasswordBox passwordBox = new PasswordBox
			{
				Name = "Pwd_" + Name
			};
			return passwordBox;
		}

		public static Label CreateLabel (string Display, string Name)
		{
			Label label = new Label
			{
				Content = Display,
				Name = "Lbl_" + Name
			};
			return label;
		}

		public static TextBox CreateTextBox (string Name)
		{
			TextBox textBox = new TextBox
			{
				Name = "Txt_" + Name
			};
			return textBox;
		}
	}
}
