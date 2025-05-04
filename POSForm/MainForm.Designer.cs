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
        /// <param name="disposing">true if managed resources should be disposed; otherwise, bool disposing)
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
            MainSplitContainer = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            cartDataGrid = new DataGridView();
            ItemName = new DataGridViewTextBoxColumn();
            Decrement = new DataGridViewButtonColumn();
            Quantity = new DataGridViewTextBoxColumn();
            Increment = new DataGridViewButtonColumn();
            UnitPrice = new DataGridViewTextBoxColumn();
            Discount = new DataGridViewTextBoxColumn();
            Total = new DataGridViewTextBoxColumn();
            buttonPay = new Button();
            grandTotalLabel = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            categoriesLayout = new FlowLayoutPanel();
            textBoxBarcode = new TextBox();
            productsLayout = new FlowLayoutPanel();
            menuStrip = new MenuStrip();
            label1 = new Label();
            dateLabel = new Label();
            loggedUserLabel = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).BeginInit();
            MainSplitContainer.Panel1.SuspendLayout();
            MainSplitContainer.Panel2.SuspendLayout();
            MainSplitContainer.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cartDataGrid).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // MainSplitContainer
            // 
            tableLayoutPanel3.SetColumnSpan(MainSplitContainer, 2);
            MainSplitContainer.Dock = DockStyle.Fill;
            MainSplitContainer.Location = new Point(3, 80);
            MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            MainSplitContainer.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // MainSplitContainer.Panel2
            // 
            MainSplitContainer.Panel2.Controls.Add(tableLayoutPanel2);
            MainSplitContainer.Panel2.RightToLeft = RightToLeft.No;
            MainSplitContainer.Size = new Size(1379, 830);
            MainSplitContainer.SplitterDistance = 424;
            MainSplitContainer.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(cartDataGrid, 0, 0);
            tableLayoutPanel1.Controls.Add(buttonPay, 0, 2);
            tableLayoutPanel1.Controls.Add(grandTotalLabel, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.Size = new Size(424, 830);
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
            cartDataGrid.Size = new Size(404, 630);
            cartDataGrid.TabIndex = 0;
            cartDataGrid.CellClick += CartGridCell_DoubleClickOrClick;
            cartDataGrid.CellDoubleClick += CartGridCell_DoubleClickOrClick;
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
            // buttonPay
            // 
            buttonPay.BackColor = SystemColors.Control;
            buttonPay.Dock = DockStyle.Fill;
            buttonPay.FlatStyle = FlatStyle.System;
            buttonPay.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonPay.ForeColor = Color.White;
            buttonPay.Location = new Point(10, 740);
            buttonPay.Margin = new Padding(10);
            buttonPay.Name = "buttonPay";
            buttonPay.Size = new Size(404, 80);
            buttonPay.TabIndex = 1;
            buttonPay.Text = "Pay";
            buttonPay.UseVisualStyleBackColor = false;
            buttonPay.Click += PayButton_Click;
            // 
            // grandTotalLabel
            // 
            grandTotalLabel.AutoSize = true;
            grandTotalLabel.Dock = DockStyle.Fill;
            grandTotalLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            grandTotalLabel.Location = new Point(10, 660);
            grandTotalLabel.Margin = new Padding(10);
            grandTotalLabel.Name = "grandTotalLabel";
            grandTotalLabel.Size = new Size(404, 60);
            grandTotalLabel.TabIndex = 2;
            grandTotalLabel.Text = "labelTotalPrice";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(categoriesLayout, 0, 2);
            tableLayoutPanel2.Controls.Add(textBoxBarcode, 0, 0);
            tableLayoutPanel2.Controls.Add(productsLayout, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new Padding(10);
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(951, 830);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // categoriesLayout
            // 
            categoriesLayout.AutoSize = true;
            categoriesLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            categoriesLayout.BackColor = SystemColors.Control;
            categoriesLayout.Dock = DockStyle.Fill;
            categoriesLayout.Location = new Point(10, 820);
            categoriesLayout.Margin = new Padding(0);
            categoriesLayout.Name = "categoriesLayout";
            categoriesLayout.Size = new Size(931, 1);
            categoriesLayout.TabIndex = 2;
            // 
            // textBoxBarcode
            // 
            textBoxBarcode.Dock = DockStyle.Fill;
            textBoxBarcode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxBarcode.Location = new Point(10, 10);
            textBoxBarcode.Margin = new Padding(0, 0, 0, 15);
            textBoxBarcode.Name = "textBoxBarcode";
            textBoxBarcode.PlaceholderText = "Barcode";
            textBoxBarcode.Size = new Size(931, 50);
            textBoxBarcode.TabIndex = 0;
            textBoxBarcode.KeyDown += BarcodeTextBox_KeyPress;
            // 
            // productsLayout
            // 
            productsLayout.AutoScroll = true;
            productsLayout.BackColor = SystemColors.AppWorkspace;
            productsLayout.Dock = DockStyle.Fill;
            productsLayout.Location = new Point(10, 75);
            productsLayout.Margin = new Padding(0, 0, 0, 15);
            productsLayout.Name = "productsLayout";
            productsLayout.Padding = new Padding(15);
            productsLayout.Size = new Size(931, 730);
            productsLayout.TabIndex = 1;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1385, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Desktop;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(223, 45);
            label1.TabIndex = 2;
            label1.Text = "Supermarket";
            // 
            // dateLabel
            // 
            dateLabel.AutoSize = true;
            dateLabel.Font = new Font("Consolas", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateLabel.Location = new Point(3, 45);
            dateLabel.Name = "dateLabel";
            dateLabel.Size = new Size(149, 32);
            dateLabel.TabIndex = 3;
            dateLabel.Text = "dateLabel";
            // 
            // loggedUserLabel
            // 
            loggedUserLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            loggedUserLabel.AutoSize = true;
            loggedUserLabel.Location = new Point(1190, 0);
            loggedUserLabel.Name = "loggedUserLabel";
            loggedUserLabel.Size = new Size(192, 32);
            loggedUserLabel.TabIndex = 4;
            loggedUserLabel.Text = "loggedUserLabel";
            loggedUserLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(MainSplitContainer, 1, 1);
            tableLayoutPanel3.Controls.Add(dateLabel, 0, 1);
            tableLayoutPanel3.Controls.Add(loggedUserLabel, 1, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 24);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1385, 913);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1385, 937);
            Controls.Add(tableLayoutPanel3);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += MainForm_Load;
            MainSplitContainer.Panel1.ResumeLayout(false);
            MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).EndInit();
            MainSplitContainer.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cartDataGrid).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer MainSplitContainer;
        private DataGridView cartDataGrid;
        private Button buttonPay;
        private Label grandTotalLabel;
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
        private TableLayoutPanel tableLayoutPanel2;
        private MenuStrip menuStrip;
        private Label label1;
        private Label dateLabel;
        private Label loggedUserLabel;
        private TableLayoutPanel tableLayoutPanel3;
    }
}
