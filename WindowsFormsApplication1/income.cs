using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApplication1
{
    public partial class Income : Form
    {
        String Name = "Tomer";
        String Type = "Work";
        int row, col;

        public Income()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            textBox1.Text = "0";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Name = (String)cmb.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "C:\\Users\\tomerbaruch\\SkyDrive\\מסמכים\\‏‏האקסולידית - תומר וליחן 16-17.xlsx";

            Excel.Application excel = new Excel.Application();
            Excel.Workbook wb = excel.Workbooks.Open(path);
            Excel.Worksheet worksheet = wb.ActiveSheet;

            int row = updateRow();
            int month_col = Form1.month_col;
            int amount;
            try
            {
                amount = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception ex)
            {
                Microsoft.VisualBasic.Interaction.MsgBox("Please enter valid amount");
                return;
            }

            worksheet.Cells[row, month_col].Value =
                    worksheet.Cells[row, month_col].Value == null || worksheet.Cells[row, month_col].Value.Equals("") ?
                    amount : worksheet.Cells[row, month_col].Value + amount;

            excel.DisplayAlerts = false;
            wb.Save();
            wb.Close(true);
            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(wb);
            Microsoft.VisualBasic.Interaction.MsgBox("Updated successfuly.");
            this.Close();
        }

        private int updateRow()
        {
            if (Type.Equals("Other"))
            {
                return 12;
            }
            if (Name.Equals("Tomer"))
            {
                if (Type.Equals("Work"))
                {
                    return 7;
                }
                if (Type.Equals("Private lessons"))
                {
                    return 9;
                }
            }
            if (Name.Equals("Lihen"))
            {
                if (Type.Equals("Work"))
                {
                    return 6;
                }
                if (Type.Equals("Private lessons"))
                {
                    return 8;
                }
            }
            return 0;
        }

        

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Type = (String)cmb.SelectedItem;
        }
    }
}
