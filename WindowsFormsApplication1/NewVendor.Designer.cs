namespace WindowsFormsApplication1
{
	partial class NewVendor
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
			this.name = new System.Windows.Forms.Label();
			this.startRow = new System.Windows.Forms.Label();
			this.shop = new System.Windows.Forms.Label();
			this.date = new System.Windows.Forms.Label();
			this.money = new System.Windows.Forms.Label();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.moneyTextBox = new System.Windows.Forms.TextBox();
			this.dateTextBox = new System.Windows.Forms.TextBox();
			this.shopTextBox = new System.Windows.Forms.TextBox();
			this.startRowTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// name
			// 
			this.name.AutoSize = true;
			this.name.Location = new System.Drawing.Point(59, 59);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(35, 13);
			this.name.TabIndex = 0;
			this.name.Text = "Name";
			// 
			// startRow
			// 
			this.startRow.AutoSize = true;
			this.startRow.Location = new System.Drawing.Point(58, 82);
			this.startRow.Name = "startRow";
			this.startRow.Size = new System.Drawing.Size(49, 13);
			this.startRow.TabIndex = 1;
			this.startRow.Text = "Start row";
			// 
			// shop
			// 
			this.shop.AutoSize = true;
			this.shop.Location = new System.Drawing.Point(58, 107);
			this.shop.Name = "shop";
			this.shop.Size = new System.Drawing.Size(69, 13);
			this.shop.TabIndex = 2;
			this.shop.Text = "Shop column";
			// 
			// date
			// 
			this.date.AutoSize = true;
			this.date.Location = new System.Drawing.Point(58, 154);
			this.date.Name = "date";
			this.date.Size = new System.Drawing.Size(67, 13);
			this.date.TabIndex = 3;
			this.date.Text = "Date column";
			// 
			// money
			// 
			this.money.AutoSize = true;
			this.money.Location = new System.Drawing.Point(58, 130);
			this.money.Name = "money";
			this.money.Size = new System.Drawing.Size(76, 13);
			this.money.TabIndex = 4;
			this.money.Text = "Money column";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(136, 55);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(100, 20);
			this.nameTextBox.TabIndex = 1;
			// 
			// moneyTextBox
			// 
			this.moneyTextBox.Location = new System.Drawing.Point(136, 127);
			this.moneyTextBox.Name = "moneyTextBox";
			this.moneyTextBox.Size = new System.Drawing.Size(100, 20);
			this.moneyTextBox.TabIndex = 4;
			// 
			// dateTextBox
			// 
			this.dateTextBox.Location = new System.Drawing.Point(136, 151);
			this.dateTextBox.Name = "dateTextBox";
			this.dateTextBox.Size = new System.Drawing.Size(100, 20);
			this.dateTextBox.TabIndex = 5;
			// 
			// shopTextBox
			// 
			this.shopTextBox.Location = new System.Drawing.Point(136, 103);
			this.shopTextBox.Name = "shopTextBox";
			this.shopTextBox.Size = new System.Drawing.Size(100, 20);
			this.shopTextBox.TabIndex = 3;
			// 
			// startRowTextBox
			// 
			this.startRowTextBox.Location = new System.Drawing.Point(136, 79);
			this.startRowTextBox.Name = "startRowTextBox";
			this.startRowTextBox.Size = new System.Drawing.Size(100, 20);
			this.startRowTextBox.TabIndex = 2;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(105, 205);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 10;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// NewVendor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.startRowTextBox);
			this.Controls.Add(this.shopTextBox);
			this.Controls.Add(this.dateTextBox);
			this.Controls.Add(this.moneyTextBox);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.money);
			this.Controls.Add(this.date);
			this.Controls.Add(this.shop);
			this.Controls.Add(this.startRow);
			this.Controls.Add(this.name);
			this.Name = "NewVendor";
			this.Text = "NewVendor";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label name;
		private System.Windows.Forms.Label startRow;
		private System.Windows.Forms.Label shop;
		private System.Windows.Forms.Label date;
		private System.Windows.Forms.Label money;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.TextBox moneyTextBox;
		private System.Windows.Forms.TextBox dateTextBox;
		private System.Windows.Forms.TextBox shopTextBox;
		private System.Windows.Forms.TextBox startRowTextBox;
		private System.Windows.Forms.Button okButton;
	}
}