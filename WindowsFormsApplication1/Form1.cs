﻿using System;
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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bankType bankType;

        public Form1()
        {
            InitializeComponent();
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
                    case bankType.Leumi:
                        row = 12;
                        shop_name = excelSheet.Cells[12, 3].Value.ToString();
                        money = excelSheet.Cells[12, 5].Value;
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
                    double curr_sum;
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
                    money = excelSheet.Cells[row, second_col].Value != null ? excelSheet.Cells[row, second_col].Value : -10000000;
                }

                print_result(result_map);
                print_attention(shops);
                textBox2.Text += sum;
                wb.Close();
                string user = Console.ReadLine();


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
                    //Console.WriteLine("Pay attention you have " + value + " charges from " + key);
                }
            }
        }

        public void print_result(Dictionary<string, double?> result_map)
        {
            int ypos = 300;
            foreach (string name in result_map.Keys)
            {
                string key = name;
                double? value = result_map[name];

                //LinkLabel link = new LinkLabel();
                //link.Text = key + " " + value +"\r\n";
                //link.LinkClicked += new LinkLabelLinkClickedEventHandler(this.link_LinkClicked);
                //LinkLabel.Link data = new LinkLabel.Link();
                //data.LinkData = key + " " + value +"\r\n"; ;
                //link.Links.Add(data);
                //link.AutoSize = true;
                //link.Location = new System.Drawing.Point(100, ypos);
                //link.TabIndex = 9999;
                //ypos += 10;


                ///
                // Create the LinkLabel.
                //this.linkLabel1 = new System.Windows.Forms.LinkLabel();

                //// Configure the LinkLabel's location. 
                //this.linkLabel1.Location = new System.Drawing.Point(100, 300);
                //// Specify that the size should be automatically determined by the content.
                //this.linkLabel1.AutoSize = true;

                //this.linkLabel1.TabIndex = 2;

                //// Add an event handler to do something when the links are clicked.
                //this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LinkClicked);

                //// Set the text for the LinkLabel.
                //this.linkLabel1.Text = key + " " + value + "\r\n";

                //// Set up how the form should be displayed and add the controls to the form.
                //this.Controls.AddRange(new System.Windows.Forms.Control[] { this.linkLabel1 });
                //this.Text = key + " " + value + "\r\n";

                //link.LinkClicked += new LinkLabelLinkClickedEventHandler(this.link_LinkClicked);
                //LinkLabel.Link data = new LinkLabel.Link();
                //data.LinkData = key + " " + value + "\r\n"; ;
                //link.Links.Add(data);
                ///


                //this.richTextBox1.Controls.Add(link);
                //this.richTextBox1.AppendText(link.Text);
                //this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;


                textBox2.Text += key + " " + value + "\r\n";
                //Console.WriteLine(key + " " + value);
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

        public Dictionary<string, string> loadHash()
        {
            Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();

            if (!Directory.Exists("C:\\credit")){
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
