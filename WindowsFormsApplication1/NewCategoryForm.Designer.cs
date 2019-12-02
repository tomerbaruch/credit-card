namespace WindowsFormsApplication1
{
	partial class NewCategoryForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.shopLabel = new System.Windows.Forms.Label();
			this.moneyLabel = new System.Windows.Forms.Label();
			this.dateLabel = new System.Windows.Forms.Label();
			this.ignore = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(49, 82);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(217, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Please choose category from the combo box";
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(95, 145);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(61, 205);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(92, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(117, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "   Or type new category";
			// 
			// shopLabel
			// 
			this.shopLabel.AutoSize = true;
			this.shopLabel.Location = new System.Drawing.Point(49, 9);
			this.shopLabel.Name = "shopLabel";
			this.shopLabel.Size = new System.Drawing.Size(35, 13);
			this.shopLabel.TabIndex = 4;
			this.shopLabel.Text = "label4";
			// 
			// moneyLabel
			// 
			this.moneyLabel.AutoSize = true;
			this.moneyLabel.Location = new System.Drawing.Point(49, 33);
			this.moneyLabel.Name = "moneyLabel";
			this.moneyLabel.Size = new System.Drawing.Size(35, 13);
			this.moneyLabel.TabIndex = 5;
			this.moneyLabel.Text = "label5";
			// 
			// dateLabel
			// 
			this.dateLabel.AutoSize = true;
			this.dateLabel.Location = new System.Drawing.Point(49, 57);
			this.dateLabel.Name = "dateLabel";
			this.dateLabel.Size = new System.Drawing.Size(35, 13);
			this.dateLabel.TabIndex = 6;
			this.dateLabel.Text = "label6";
			// 
			// ignore
			// 
			this.ignore.Location = new System.Drawing.Point(170, 205);
			this.ignore.Name = "ignore";
			this.ignore.Size = new System.Drawing.Size(75, 23);
			this.ignore.TabIndex = 7;
			this.ignore.Text = "ignore";
			this.ignore.UseVisualStyleBackColor = true;
			this.ignore.Click += new System.EventHandler(this.ignore_Click);
			// 
			// NewCategoryForm
			// 
			this.ClientSize = new System.Drawing.Size(320, 251);
			this.Controls.Add(this.ignore);
			this.Controls.Add(this.dateLabel);
			this.Controls.Add(this.moneyLabel);
			this.Controls.Add(this.shopLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label2);
			this.Name = "NewCategoryForm";
			this.Text = "New Category";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label shopLabel;
		private System.Windows.Forms.Label moneyLabel;
		private System.Windows.Forms.Label dateLabel;
		private System.Windows.Forms.Button ignore;
	}
}