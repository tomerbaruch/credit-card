﻿using System;
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
	public partial class CategoryForm : Form
	{
		public CategoryForm(List<string> categories, string shop_name, double money, string date)
		{
			InitializeComponent();
			initData(categories, shop_name, money, date);
		}

		private void initData(List<string> categories, string shop_name, double money, string date)
		{
			this.comboBox1.Items.AddRange(categories.ToArray());
			this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

			shopLabel.Text = "Shop name: " + shop_name;
			moneyLabel.Text = "Sum: " + money.ToString();
			dateLabel.Text = "Date: " + date;
		}

		public string result;

		private void button1_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem == null && string.IsNullOrEmpty(comboBox1.Text))
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Please choose category or enter a new one");
				return;
			}

			result = comboBox1.SelectedItem == null ? comboBox1.Text : comboBox1.SelectedItem.ToString();
			this.Close();
		}
	}
}