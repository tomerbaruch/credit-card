using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bankType bankType = bankType.Unknown;
		string bankChosen;
        public static int month_col;
		Dictionary<string, bankData> banks_hash = new Dictionary<string, bankData>();
		Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();
		Dictionary<string, int> exolidit_hash = new Dictionary<string, int>();

		string data_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\data\\";
		string input_dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\input\\";

		string shops_file = "shops.txt";
		string exolidit_file = "exolidit.txt";
		string banks_file = "banks.txt";


		public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;

			banks_hash = loadBanks();
			updateBanksCheckboxes(banks_hash);

			shop_category_hash = loadHash();
			exolidit_hash = loadExoliditHash();
		}

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string cat = Microsoft.VisualBasic.Interaction.InputBox(e.Link.LinkData.ToString(), "New category", "Enter category here", 450, 300);
        }

        void credit(){

			Dictionary<string, double?> result_map = new Dictionary<string, double?>();
            Dictionary<string, int?> shops = new Dictionary<string, int?>();

			textBox2.Text = "";

			try
            {
                string path = openFileDialog1.FileName;
                double sum = 0;
                double money = 0;
				string date;
				int row = 0, first_col = 0, second_col = 0, date_col = -1 ;
                String shop_name = "";

                Excel.Application excel = new Excel.Application();
                Excel.Workbook wb = excel.Workbooks.Open(path);
                Excel.Worksheet excelSheet = wb.ActiveSheet;

				//           switch (bankType){
				//               case bankType.Unknown:
				//                   Microsoft.VisualBasic.Interaction.MsgBox("Please choose bank");
				//                   return;
				//               case bankType.Leumi:
				//                   row = 12;
				//                   shop_name = excelSheet.Cells[12, 3].Value.ToString();
				//                   money = excelSheet.Cells[12, 5].Value;
				//                   first_col = 3;
				//                   second_col = 5;
				//                   break;
				//               case bankType.Cal:
				//                   //row = 9;
				//                   //shop_name = excelSheet.Cells[9, 2].Value.ToString();
				//                   //money = excelSheet.Cells[9, 5].Value;
				//                   //first_col = 2;
				//                   //second_col = 5;
				//                   //break;
				//                   row = 4;
				//                   shop_name = excelSheet.Cells[row, 2].Value.ToString();
				//                   money = excelSheet.Cells[row, 4].Value;
				//                   first_col = 2;
				//                   second_col = 4;
				//                   break;
				//               case bankType.Poalim:
				//                   row = 7;
				//                   shop_name = excelSheet.Cells[row, 2].Value.ToString();
				//                   money = excelSheet.Cells[row, 5].Value;
				//                   first_col = 2;
				//                   second_col = 5;
				//                   break;
				//case bankType.Benleumi:
				//	row = 4;
				//	shop_name = excelSheet.Cells[row, 2].Value.ToString();
				//	money = excelSheet.Cells[row, 4].Value;
				//	first_col = 2;
				//	second_col = 4;
				//	break;
				//default:
				//                   break;


				//           }

				if (bankType == bankType.Unknown)
				{
					Microsoft.VisualBasic.Interaction.MsgBox("Please choose bank");
					return;
				}

				bankData bankDataChosen = banks_hash[bankChosen];
				row = bankDataChosen.startRow;
				first_col = bankDataChosen.shop;
				second_col = bankDataChosen.money;
				date_col = bankDataChosen.date;

				shop_name = excelSheet.Cells[row, first_col].Value.ToString();
				string moneyText = Regex.Replace(excelSheet.Cells[row, second_col].Value, @"[^\d-]", "");
				money = double.Parse(moneyText);
				//money = excelSheet.Cells[row, second_col].Value;
				date = excelSheet.Cells[row, date_col].Value.ToString();
				date = date.Substring(0, date.IndexOf(" ") + 1);

				while (shop_name != null && !shop_name.Equals(""))
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
                        string cat = Microsoft.VisualBasic.Interaction.InputBox("Enter new category for " + shop_name + "\nSum: " + money + "\nDate: " + date, "New category", "Enter category here", 450, 300).ToString();
						if (!String.IsNullOrEmpty(cat) && !cat.Equals("Enter category here"))
						{
							try
							{
								using (StreamWriter sw = new StreamWriter(data_dir + shops_file, true, System.Text.Encoding.GetEncoding(1255), 512))
								{
									sw.WriteLine(shop_name + "#" + cat);
									shop_category_hash[shop_name] = cat;
									add_shop_to_result(result_map, cat, money);
								}
							}
							catch (Exception)
							{
							}
						}
                    }

                    row++;
                    shop_name = excelSheet.Cells[row, first_col].Value;
					date = excelSheet.Cells[row, date_col].Value.ToString();
					date = date.Substring(0, date.IndexOf(" ") + 1);

					if (excelSheet.Cells[row, second_col].Value != null && !excelSheet.Cells[row, second_col].Value.Equals("")) {
						moneyText = Regex.Replace(excelSheet.Cells[row, second_col].Value, @"[^\d-]", "");
						money = double.Parse(moneyText);
					} else {
						money = 0;
					}
                }

                print_result(result_map);
                print_attention(shops);
                textBox2.Text += sum;
                wb.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excelSheet);
                Marshal.ReleaseComObject(wb);
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
            catch (RuntimeBinderException ex)
            {
                Microsoft.VisualBasic.Interaction.MsgBox("Credit card file is invalid");
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

			if (checkboxes.Count == 1)
			{
				checkboxes.ElementAt(0).Checked = true;
			}
		}

		private void saveToExolidit(Dictionary<string, double?> result_map, Dictionary<string, int> exolidit_map)
        {
            string path = openFileDialog4.FileName;
            if (path.Equals(""))
            {
                return;
            }

            Excel.Application excel = new Excel.Application();
            Excel.Workbook wb = excel.Workbooks.Open(path);
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
        }

        public void print_result(Dictionary<string, double?> result_map)
        {
            foreach (string name in result_map.Keys)
            {
                string key = name;
                double? value = result_map[name];

                textBox2.Text += key + " " + value + "\r\n";
            }
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

		public Dictionary<string, string> loadHash()
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
			//try
			//{
			//    MailMessage mail = new MailMessage();
			//    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

			//    mail.From = new MailAddress("tomehtomeh@gmail.com");
			//    //mail.To.Add("haco29@gmail.com");
			//    mail.To.Add("tomehtomeh@gmail.com");
			//    mail.Subject = "Test Mail";
			//    mail.Body = "This is for testing SMTP mail from GMAIL from TH application";

			//    SmtpServer.Port = 587;
			//    SmtpServer.Credentials = new System.Net.NetworkCredential("tomehtomeh@gmail.com", "J6cd3q3p1358");
			//    SmtpServer.EnableSsl = true;
			//    SmtpServer.UseDefaultCredentials = false;
			//    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

			//    SmtpServer.Send(mail);
			//    MessageBox.Show("mail Send");
			//}
			//catch (Exception ex)
			//{
			//    MessageBox.Show(ex.ToString());
			//}
			textBox2.Text = "";

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			bankType = bankType.Leumi;
			bankChosen = checkBox1.Text;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            bankType = bankType.Cal;
			bankChosen = checkBox2.Text;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            bankType = bankType.Poalim;
			bankChosen = checkBox3.Text;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			bankType = bankType.Benleumi;
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
            //switch (selectedValue)
            //{
            //    case "January":
            //    case "February":
            //    case "March":
            //    case "April":
            //    case "May":
            //    case "June":
            //    case "July":
            //    case "August":
            //    case "September":
            //    case "October":
            //        month_col = selectedIndex+5;
            //        break;
            //    case "November":
            //        month_col = 3;
            //        break;
            //    case "December":
            //        month_col = 4;
            //        break;
            //    default:
            //        break;
            //}
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
