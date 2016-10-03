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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bankType bankType = bankType.Unknown;
        int month_col;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string cat = Microsoft.VisualBasic.Interaction.InputBox(e.Link.LinkData.ToString(), "New category", "Enter category here", 450, 300);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bankType = bankType.Leumi;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

       void credit(){
            Dictionary<string, string> shop_category_hash = loadHash();
            Dictionary<string, int> exolidit_hash = loadExoliditHash();
            
            Dictionary<string, double?> result_map = new Dictionary<string, double?>();
            Dictionary<string, int?> shops = new Dictionary<string, int?>();

            try
            {
                string path = openFileDialog1.FileName;
                //string path = "C:\\credit\\" + excel_name;
                double sum = 0;
                double money = 0;
                int row=0,first_col=0,second_col=0;
                String shop_name = "";

                Excel.Application excel = new Excel.Application();
                Excel.Workbook wb = excel.Workbooks.Open(path);
                Excel.Worksheet excelSheet = wb.ActiveSheet;

                switch (bankType){
                    case bankType.Unknown:
                        Microsoft.VisualBasic.Interaction.MsgBox("Please choose bank");
                        return;
                    case bankType.Leumi:
                        row = 12;
                        shop_name = excelSheet.Cells[12, 3].Value.ToString();
                        money = excelSheet.Cells[12, 5].Value;
                        first_col = 3;
                        second_col = 5;
                        break;
                    case bankType.Cal:
                        row = 9;
                        shop_name = excelSheet.Cells[9, 2].Value.ToString();
                        money = excelSheet.Cells[9, 5].Value;
                        first_col = 2;
                        second_col = 5;
                        break;
                    case bankType.Poalim:
                        row = 25;
                        shop_name = excelSheet.Cells[25, 4].Value.ToString();
                        money = excelSheet.Cells[25, 6].Value;
                        first_col = 4;
                        second_col = 6;
                        break;
                    default:
                        break;


                }
                //Read the first cell leumi
                //int i = 12;
                //String shop_name = excelSheet.Cells[12, 3].Value.ToString();
                //Double money = excelSheet.Cells[12, 5].Value;

                //Read the first cell hitec
                //int i = 9;
                //String shop_name = excelSheet.Cells[9, 2].Value.ToString();
                //Double money = excelSheet.Cells[9, 5].Value;
                //int first_col = 2;
                //int second_col = 5;

                //Read the first cell poalim
                //int i = 25;
                //int first_col = 4;
                //int second_col = 6;
                //String shop_name = excelSheet.Cells[25, 4].Value.ToString();
                //Double money = excelSheet.Cells[25, 6].Value;

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
                        string cat = Microsoft.VisualBasic.Interaction.InputBox("Enter new category for " + shop_name, "New category", "Enter category here", 450, 300).ToString();

                        try
                        {
                            using (StreamWriter sw = new StreamWriter("C:\\credit\\shops.txt", true, System.Text.Encoding.GetEncoding(1255), 512))
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

                    row++;
                    shop_name = excelSheet.Cells[row, first_col].Value;
                    money = excelSheet.Cells[row, second_col].Value != null 
                        && !excelSheet.Cells[row, second_col].Value.Equals("") ? excelSheet.Cells[row, second_col].Value : 0;
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
                        using (StreamWriter sw = new StreamWriter("C:\\credit\\exolidit.txt", true, System.Text.Encoding.GetEncoding(1255), 512))
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

            if (!Directory.Exists("C:\\credit")){
                Directory.CreateDirectory("C:\\credit");
            }


            if (!File.Exists("C:\\credit\\exolidit.txt"))
            {
                File.CreateText("C:\\credit\\exolidit.txt");
            }


            System.IO.StreamReader br = new System.IO.StreamReader("C:\\credit\\exolidit.txt", System.Text.Encoding.GetEncoding(1255));
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

        public Dictionary<string, string> loadHash()
        {
            Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();

            if (!Directory.Exists("C:\\credit"))
            {
                Directory.CreateDirectory("C:\\credit");
            }


            if (!File.Exists("C:\\credit\\shops.txt"))
            {
                File.CreateText("C:\\credit\\shops.txt");
            }


            System.IO.StreamReader br = new System.IO.StreamReader("C:\\credit\\shops.txt", System.Text.Encoding.GetEncoding(1255));
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog1.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("tomehtomeh@gmail.com");
                //mail.To.Add("haco29@gmail.com");
                mail.To.Add("tomehtomeh@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL from TH application";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tomehtomeh@gmail.com", "J6cd3q3p1358");
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            bankType = bankType.Cal;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            bankType = bankType.Poalim;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog4_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            openFileDialog4.InitialDirectory = "C:\\";
            openFileDialog4.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog4.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            String selectedValue = (String)cmb.SelectedItem;
            int selectedIndex = cmb.SelectedIndex;
            switch (selectedValue)
            {
                case "January":
                case "February":
                case "March":
                case "April":
                case "May":
                case "June":
                case "July":
                case "August":
                case "September":
                case "October":
                    month_col = selectedIndex+5;
                    break;
                case "November":
                    month_col = 3;
                    break;
                case "December":
                    month_col = 4;
                    break;
                default:
                    break;
            }
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
