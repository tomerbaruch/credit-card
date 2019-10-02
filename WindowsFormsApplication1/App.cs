using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Text.RegularExpressions;
using System.Globalization;
using WindowsFormsApplication1;

namespace CreditCardAnalyzer
{
    public partial class Form1 : Form
    {
		string bankChosen;
        public static int month_col;
		Dictionary<string, bankData> banks_hash = new Dictionary<string, bankData>();
		Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();
		Dictionary<string, int> exolidit_hash = new Dictionary<string, int>();
		List<string> sortedCategories = new List<string>();

		string data_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\data\\";
		string input_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\input\\";

		string shops_file = "shops.txt";
		string exolidit_file = "exolidit.txt";
		string exolidit_path_file = "exolidit_path.txt";
		string banks_file = "banks.txt";

		string exolidit_path;
		string creditCardPath;
		bool loadResultToExolidit = false;

		public Form1()
        {
			this.StartPosition = FormStartPosition.CenterScreen;

			InitializeComponent();
            comboBox1.SelectedIndex = 0;

			banks_hash = loadBanks();
			updateBanksCheckboxes(banks_hash);

			shop_category_hash = loadShopCategoryHash();
			exolidit_hash = loadExoliditHash();
			exolidit_path = loadExoliditPath();

			loadCreditCardIfPresent();
			prepareSortedCategories();
		}

		private void prepareSortedCategories()
		{
			sortedCategories = shop_category_hash.Values.Distinct().ToList();
			sortedCategories.Sort();
		}

		private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string cat = Microsoft.VisualBasic.Interaction.InputBox(e.Link.LinkData.ToString(), "New category", "Enter category here", 450, 300);
        }

        void credit(){

			Dictionary<string, double?> result_map = new Dictionary<string, double?>();
            Dictionary<string, int?> shops = new Dictionary<string, int?>();

			textBox2.Text = "";

			if (string.IsNullOrEmpty(creditCardPath))
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Please choose credit card file");
				return;
			}

			Excel.Application excel = new Excel.Application();
			Excel.Workbook wb = excel.Workbooks.Open(creditCardPath);
			Excel.Worksheet excelSheet = wb.ActiveSheet;

			try
            {
                double sum = 0;
                double money = 0;
				string date;
				int row = 0, first_col = 0, second_col = 0, date_col = -1 ;
                String shop_name = "";

				if (string.IsNullOrEmpty(bankChosen))
				{
					Microsoft.VisualBasic.Interaction.MsgBox("Please choose bank");
					return;
				}

				//excel = new Excel.Application();
    //            wb = excel.Workbooks.Open(creditCardPath);
    //            excelSheet = wb.ActiveSheet;

				bankData bankDataChosen = banks_hash[bankChosen];
				row = bankDataChosen.startRow;
				first_col = bankDataChosen.shop;
				second_col = bankDataChosen.money;
				date_col = bankDataChosen.date;

				shop_name = excelSheet.Cells[row, first_col].Value.ToString();
				string moneyTextValue = excelSheet.Cells[row, second_col].Value.ToString();
				string moneyText = Regex.Replace(moneyTextValue, @"[^0-9*.-]+", "");
				money = double.Parse(moneyText, CultureInfo.InvariantCulture);
				//money = excelSheet.Cells[row, second_col].Value;
				date = excelSheet.Cells[row, date_col].Value.ToString();
				//date = date.Substring(0, date.IndexOf(" ") + 1);

				while (!String.IsNullOrEmpty(shop_name))
                {
                    sum += money;
                    if (shop_category_hash.ContainsKey(shop_name))
                    {
                        if (shops.ContainsKey(shop_name))
                        {
                            shops[shop_name] = shops[shop_name] + 1;
                        }
                        else
                        {
                            shops[shop_name] = 1;
                        }
                        string cat = shop_category_hash[shop_name];
                        add_shop_to_result(result_map, cat, money);
                    }
                    else
                    {
						CategoryForm categoryForm = new CategoryForm(sortedCategories, shop_name, money, date);
						categoryForm.StartPosition = FormStartPosition.CenterParent;
						categoryForm.ShowDialog();
						string cat = categoryForm.result;

						//string cat = Microsoft.VisualBasic.Interaction.InputBox("Enter new category for " + shop_name + "\nSum: " + money + "\nDate: " + date, "New category", "Enter category here", 450, 300).ToString();
						if (!String.IsNullOrEmpty(cat) && !cat.Equals("Enter category here"))
						{
							try
							{
								using (StreamWriter sw = new StreamWriter(data_dir + shops_file, true, System.Text.Encoding.GetEncoding(1255), 512))
								{
									sw.WriteLine(shop_name + "#" + cat);
									shop_category_hash[shop_name] = cat;
									add_shop_to_result(result_map, cat, money);

									if (categoryForm.isNew)
									{
										sortedCategories.Add(cat);
										sortedCategories.Sort();
									}
								}
							}
							catch (Exception)
							{
							}
						}
                    }

                    row++;
                    shop_name = excelSheet.Cells[row, first_col].Value;

					//finished
					if (String.IsNullOrEmpty(shop_name))
					{
						break;
					}

					date = excelSheet.Cells[row, date_col].Value.ToString();
					//date = date.Substring(0, date.IndexOf(" ") + 1);

					if (excelSheet.Cells[row, second_col].Value != null && !excelSheet.Cells[row, second_col].Value.Equals("")) {
						moneyText = Regex.Replace(excelSheet.Cells[row, second_col].Value.ToString(), @"[^0-9*.-]+", "");
						money = double.Parse(moneyText, CultureInfo.InvariantCulture);
					} else {
						money = 0;
					}
                }

                print_result(result_map);
                print_attention(shops);
                textBox2.Text += "Total amount: " + sum;
				//closeAllFiles(excel, wb, excelSheet);
				string user = Console.ReadLine();

                saveToExolidit(result_map, exolidit_hash);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
			}
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
			}
            catch (Exception ex)
            {
                Microsoft.VisualBasic.Interaction.MsgBox("Credit card file is invalid");
			}

			closeAllFiles(excel, wb, excelSheet);
		}

		private void closeAllFiles(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet excelSheet)
		{
			wb.Close();
			excel.Quit();
			Marshal.ReleaseComObject(excelSheet);
			Marshal.ReleaseComObject(wb);
		}

		private void loadCreditCardIfPresent()
		{
			var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				".xls",
				".xlsx",
				".xlsm",
			};

			var directory = new DirectoryInfo(input_dir);
			FileInfo[] files = directory.GetFiles();

			if (files.Count() > 0) {
				creditCardPath = files.OrderByDescending(f => f.CreationTime).First().FullName;
			}
		}

		private void updateBanksCheckboxes(Dictionary<string, bankData> banks_hash)
		{

			List<CheckBox> checkboxes = new List<CheckBox>();
			checkboxes.Add(checkBox1);
			checkboxes.Add(checkBox2);
			checkboxes.Add(checkBox3);
			checkboxes.Add(checkBox4);

			foreach (CheckBox checkbox in checkboxes) {
				checkbox.Hide();
				checkbox.Checked = false;
			}

			int i = 0;
			foreach (bankData bank in banks_hash.Values)
			{
				checkboxes.ElementAt(i).Text = bank.name;
				checkboxes.ElementAt(i).Show();
				i++;
			}

			//mark first as default
			checkboxes.ElementAt(0).Checked = true;
		}

		private void saveToExolidit(Dictionary<string, double?> result_map, Dictionary<string, int> exolidit_map)
        {
            string path = openFileDialog4.FileName;
			if (!String.IsNullOrEmpty(path))
			{
				updateExoliditPathFile(path);
				exolidit_path = path;
			}

            if (!loadResultToExolidit)
            {
                return;
            }

            Excel.Application excel = new Excel.Application();
            Excel.Workbook wb = excel.Workbooks.Open(exolidit_path);
            Excel.Worksheet worksheet = wb.ActiveSheet;

            foreach (string category in result_map.Keys)
            {
                double value = result_map[category].Value;
                int row = 0;
                if (exolidit_map.ContainsKey(category))
                {
                    row = exolidit_map[category];
                }
                else
                {
                    String str_row = Microsoft.VisualBasic.Interaction.InputBox("Enter row for " + category, "New category", "Enter category here", 450, 300).ToString();

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(data_dir + exolidit_file, true, System.Text.Encoding.GetEncoding(1255), 512))
                        {
                            sw.WriteLine(category + "#" + str_row);
                            row = Convert.ToInt32(str_row);
                            sw.Close();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

				// update the excel itself
                worksheet.Cells[row, month_col].Value =
                    worksheet.Cells[row, month_col].Value == null || worksheet.Cells[row, month_col].Value.Equals("") ?
                    value : worksheet.Cells[row, month_col].Value + value;
            }
            excel.DisplayAlerts = false;
            wb.Save();
            wb.Close(true);
            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(wb);
            Microsoft.VisualBasic.Interaction.MsgBox("Finished update Exolidit.");
        }

		private void updateExoliditPathFile(string path)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(data_dir + exolidit_path_file, false, System.Text.Encoding.GetEncoding(1255), 512))
				{
					sw.WriteLine(path);
					sw.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		public void print_attention(Dictionary<string, int?> shops)
        {
            foreach (string name in shops.Keys)
            {
                string key = name;
                int value = shops[name].Value;
                if (value > 1)
                {
                    textBox2.Text += "Pay attention you have " + value + " charges from " + key + "\r\n";
                }
            }

			textBox2.Text += "\r\n";
		}

        public void print_result(Dictionary<string, double?> result_map)
        {
            foreach (string name in result_map.Keys)
            {
                string key = name;
                double? value = result_map[name];

                textBox2.Text += key + " " + value + "\r\n";
            }

			textBox2.Text += "\r\n";
		}

        public void add_shop_to_result(Dictionary<string, double?> result_map, string cat, double money)
        {
            if (result_map.ContainsKey(cat))
            {
                double curr_sum = result_map[cat].Value;
                result_map[cat] = curr_sum + money;
            }
            else
            {
                result_map[cat] = money;
            }
        }

        public Dictionary<string, int> loadExoliditHash()
        {
            Dictionary<string, int> category_exolidit_row_hash = new Dictionary<string, int>();

            if (!File.Exists(data_dir + exolidit_file))
            {
                File.CreateText(data_dir + exolidit_file);
            }

            System.IO.StreamReader br = new System.IO.StreamReader(data_dir + exolidit_file, System.Text.Encoding.GetEncoding(1255));
            try
            {
                string line = br.ReadLine();

                while (line != null)
                {
                    string[] arr = line.Split('#');
                    if (category_exolidit_row_hash.ContainsKey(arr[0]))
                    {
                        line = br.ReadLine();
                        continue;
                    }
                    string category = arr[0];
                    int row = Convert.ToInt32(arr[1]);
                    category_exolidit_row_hash[category] = row;
                    line = br.ReadLine();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                br.Close();
            }


            return category_exolidit_row_hash;
        }

		public Dictionary<string, bankData> loadBanks()
		{
			Dictionary<string, bankData> bankMap = new Dictionary<string, bankData>();

			if (!File.Exists(data_dir + banks_file))
			{
				File.CreateText(data_dir + banks_file);
			}


			System.IO.StreamReader br = new System.IO.StreamReader(data_dir + banks_file, System.Text.Encoding.GetEncoding(1255));
			try
			{
				string line = br.ReadLine();
				line = br.ReadLine(); //skip template line

				while (line != null)
				{
					string[] arr = line.Split('#');
					if (arr.Length != 5)
					{
						line = br.ReadLine();
						continue;
					}

					string bankName = arr[0];
					bankData bankData = new bankData(arr[0], arr[1], arr[2], arr[3], arr[4]);

					if (bankMap.ContainsKey(arr[0]))
					{
						line = br.ReadLine();
						continue;
					}

					bankMap.Add(bankName, bankData);
					line = br.ReadLine();
				}
			}
			catch (Exception)
			{

			}
			finally
			{
				br.Close();
			}


			return bankMap;
		}

		private string loadExoliditPath()
		{
			string exolidit_path = "";

			if (!File.Exists(data_dir + exolidit_path_file))
			{
				File.CreateText(data_dir + exolidit_path_file);
			}

			System.IO.StreamReader br = new System.IO.StreamReader(data_dir + exolidit_path_file, System.Text.Encoding.GetEncoding(1255));
			try
			{
				string line = br.ReadLine();

				while (line != null)
				{
					exolidit_path = line;
					line = br.ReadLine();
				}
			}
			catch (Exception)
			{

			}
			finally
			{
				br.Close();
			}


			return exolidit_path;
		}

		public Dictionary<string, string> loadShopCategoryHash()
        {
            Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();

			if (!File.Exists(data_dir + shops_file))
            {
                File.CreateText(data_dir + shops_file);
            }


            System.IO.StreamReader br = new System.IO.StreamReader(data_dir + shops_file, System.Text.Encoding.GetEncoding(1255));
            try
            {
                string line = br.ReadLine();

                while (line != null)
                {
                    string[] arr = line.Split('#');
                    if (shop_category_hash.ContainsKey(arr[0]))
                    {
                        line = br.ReadLine();
                        continue;
                    }
                    string shop = arr[0];
                    string category = arr[1];
                    shop_category_hash[shop] = category;
                    line = br.ReadLine();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                br.Close();
            }


            return shop_category_hash;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            credit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = input_dir;
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
			textBox2.Text = "";

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			bankChosen = checkBox1.Text;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
			bankChosen = checkBox2.Text;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
			bankChosen = checkBox3.Text;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			bankChosen = checkBox4.Text;
		}

        private void button4_Click_1(object sender, EventArgs e)
        {
            openFileDialog4.InitialDirectory = input_dir;
            openFileDialog4.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog4.ShowDialog();
		}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            String selectedValue = (String)cmb.SelectedItem;
            int selectedIndex = cmb.SelectedIndex;
            month_col = selectedIndex + 3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Income income = new Income();
            income.Show();
        }

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(exolidit_path))
			{
				checkBox5.Checked = false;
				Microsoft.VisualBasic.Interaction.MsgBox("Please choose exolidit file first");
				return;
			}

			loadResultToExolidit = !loadResultToExolidit;
		}

		private void openFileDialog4_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			exolidit_path = openFileDialog4.FileName;
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			creditCardPath = openFileDialog1.FileName;
		}
	}


	public class InputBox
    {
        public static Form frmInputDialog;
        public static Label lblPrompt;
        public static Button btnOK;
        public static Button btnCancel;
        public static TextBox txtInput;

        public InputBox()
        {

        }

        private static void InitializeComponent()
        {
            frmInputDialog = new Form();
            lblPrompt = new Label();
            btnOK = new Button();
            btnCancel = new Button();
        }


    }
}
