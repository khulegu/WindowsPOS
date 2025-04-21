namespace POSForm
{
    partial class MainForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            cartDataGrid = new DataGridView();
            buttonPay = new Button();
            label1 = new Label();
            categoriesLayout = new FlowLayoutPanel();
            productsLayout = new FlowLayoutPanel();
            textBoxBarcode = new TextBox();
            ItemName = new DataGridViewTextBoxColumn();
            Decrement = new DataGridViewButtonColumn();
            Quantity = new DataGridViewTextBoxColumn();
            Increment = new DataGridViewButtonColumn();
            UnitPrice = new DataGridViewTextBoxColumn();
            Discount = new DataGridViewTextBoxColumn();
            Total = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cartDataGrid).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(categoriesLayout);
            splitContainer1.Panel2.Controls.Add(productsLayout);
            splitContainer1.Panel2.Controls.Add(textBoxBarcode);
            splitContainer1.Panel2.RightToLeft = RightToLeft.No;
            splitContainer1.Size = new Size(1724, 1052);
            splitContainer1.SplitterDistance = 874;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(cartDataGrid, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonPay, 0, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Size = new Size(874, 1052);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // cartDataGrid
            // 
            cartDataGrid.AllowUserToAddRows = false;
            cartDataGrid.AllowUserToDeleteRows = false;
            cartDataGrid.AllowUserToResizeColumns = false;
            cartDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            cartDataGrid.Columns.AddRange(new DataGridViewColumn[] { ItemName, Decrement, Quantity, Increment, UnitPrice, Discount, Total });
            cartDataGrid.Dock = DockStyle.Fill;
            cartDataGrid.Location = new Point(10, 10);
            cartDataGrid.Margin = new Padding(10);
            cartDataGrid.MaximumSize = new Size(10000, 10000);
            cartDataGrid.Name = "cartDataGrid";
            cartDataGrid.ReadOnly = true;
            cartDataGrid.RowHeadersWidth = 82;
            cartDataGrid.Size = new Size(854, 852);
            cartDataGrid.TabIndex = 0;
            cartDataGrid.CellClick += cartDataGrid_CellClick;
            // 
            // buttonPay
            // 
            buttonPay.BackColor = Color.LimeGreen;
            buttonPay.Dock = DockStyle.Fill;
            buttonPay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonPay.ForeColor = Color.White;
            buttonPay.Location = new Point(10, 962);
            buttonPay.Margin = new Padding(10);
            buttonPay.Name = "buttonPay";
            buttonPay.Size = new Size(854, 80);
            buttonPay.TabIndex = 1;
            buttonPay.Text = "Pay";
            buttonPay.UseVisualStyleBackColor = false;
            buttonPay.Click += buttonPay_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(10, 882);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(854, 60);
            label1.TabIndex = 2;
            label1.Text = "labelTotalPrice";
            // 
            // categoriesLayout
            // 
            categoriesLayout.Location = new Point(27, 602);
            categoriesLayout.Name = "categoriesLayout";
            categoriesLayout.Size = new Size(778, 200);
            categoriesLayout.TabIndex = 2;
            // 
            // productsLayout
            // 
            productsLayout.AutoScroll = true;
            productsLayout.Location = new Point(27, 90);
            productsLayout.Name = "productsLayout";
            productsLayout.Padding = new Padding(5);
            productsLayout.Size = new Size(778, 473);
            productsLayout.TabIndex = 1;
            // 
            // textBoxBarcode
            // 
            textBoxBarcode.Location = new Point(27, 12);
            textBoxBarcode.Name = "textBoxBarcode";
            textBoxBarcode.PlaceholderText = "Barcode";
            textBoxBarcode.Size = new Size(778, 39);
            textBoxBarcode.TabIndex = 0;
            textBoxBarcode.KeyDown += textBoxBarcode_KeyDown;
            // 
            // ItemName
            // 
            ItemName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ItemName.DataPropertyName = "Name";
            ItemName.HeaderText = "Item Name";
            ItemName.MinimumWidth = 10;
            ItemName.Name = "ItemName";
            ItemName.ReadOnly = true;
            // 
            // Decrement
            // 
            Decrement.HeaderText = "";
            Decrement.MinimumWidth = 10;
            Decrement.Name = "Decrement";
            Decrement.ReadOnly = true;
            Decrement.Text = "-";
            Decrement.ToolTipText = "-";
            Decrement.UseColumnTextForButtonValue = true;
            Decrement.Width = 30;
            // 
            // Quantity
            // 
            Quantity.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Quantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Quantity.DefaultCellStyle = dataGridViewCellStyle1;
            Quantity.HeaderText = "Quantity";
            Quantity.MinimumWidth = 10;
            Quantity.Name = "Quantity";
            Quantity.ReadOnly = true;
            Quantity.Width = 151;
            // 
            // Increment
            // 
            Increment.HeaderText = "";
            Increment.MinimumWidth = 10;
            Increment.Name = "Increment";
            Increment.ReadOnly = true;
            Increment.Text = "+";
            Increment.ToolTipText = "+";
            Increment.UseColumnTextForButtonValue = true;
            Increment.Width = 30;
            // 
            // UnitPrice
            // 
            UnitPrice.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            UnitPrice.DataPropertyName = "Price";
            UnitPrice.HeaderText = "U/Price";
            UnitPrice.MinimumWidth = 10;
            UnitPrice.Name = "UnitPrice";
            UnitPrice.ReadOnly = true;
            UnitPrice.Width = 135;
            // 
            // Discount
            // 
            Discount.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Discount.DataPropertyName = "Discount";
            Discount.HeaderText = "Dis%";
            Discount.MinimumWidth = 10;
            Discount.Name = "Discount";
            Discount.ReadOnly = true;
            Discount.Width = 112;
            // 
            // Total
            // 
            Total.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Total.DataPropertyName = "Total";
            Total.HeaderText = "Total";
            Total.MinimumWidth = 10;
            Total.Name = "Total";
            Total.ReadOnly = true;
            Total.Width = 110;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1724, 1052);
            Controls.Add(splitContainer1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cartDataGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView cartDataGrid;
        private Button buttonPay;
        private Label label1;
        private TextBox textBoxBarcode;
        private FlowLayoutPanel categoriesLayout;
        private FlowLayoutPanel productsLayout;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewButtonColumn Decrement;
        private DataGridViewTextBoxColumn Quantity;
        private DataGridViewButtonColumn Increment;
        private DataGridViewTextBoxColumn UnitPrice;
        private DataGridViewTextBoxColumn Discount;
        private DataGridViewTextBoxColumn Total;
    }
}