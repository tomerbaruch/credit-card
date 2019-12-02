using CreditCardAnalyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
	public partial class NewVendor : Form
	{
		public NewVendor()
		{
			InitializeComponent();
		}

		public bankData bankData;

		private void okButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(nameTextBox.Text) ||
				string.IsNullOrEmpty(startRowTextBox.Text) ||
				string.IsNullOrEmpty(shopTextBox.Text) ||
				string.IsNullOrEmpty(moneyTextBox.Text) ||
				string.IsNullOrEmpty(dateTextBox.Text))
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Please fill all text boxes");
				return;
			}

			try
			{
				bankData = new bankData(nameTextBox.Text, startRowTextBox.Text, shopTextBox.Text, moneyTextBox.Text, dateTextBox.Text);
			}
			catch (Exception ex)
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Please fill only allowed values");
				return;
			}

			this.Close();
		}
	}
}
