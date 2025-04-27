namespace POSForm
{
    partial class ProductControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelPrice = new Label();
            labelName = new Label();
            pictureBox = new PictureBox();
            tableLayout = new TableLayoutPanel();
            labelBarcode = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            tableLayout.SuspendLayout();
            SuspendLayout();
            // 
            // labelPrice
            // 
            labelPrice.AutoSize = true;
            labelPrice.Location = new Point(5, 250);
            labelPrice.Margin = new Padding(5, 0, 0, 5);
            labelPrice.Name = "labelPrice";
            labelPrice.Size = new Size(116, 32);
            labelPrice.TabIndex = 0;
            labelPrice.Text = "labelPrice";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelName.Location = new Point(5, 200);
            labelName.Margin = new Padding(5, 0, 0, 5);
            labelName.Name = "labelName";
            labelName.Size = new Size(136, 32);
            labelName.TabIndex = 1;
            labelName.Text = "labelName";
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.WhiteSmoke;
            pictureBox.BackgroundImageLayout = ImageLayout.None;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(3, 3);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(192, 194);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // tableLayout
            // 
            tableLayout.AutoSize = true;
            tableLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayout.ColumnCount = 1;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayout.Controls.Add(pictureBox, 0, 0);
            tableLayout.Controls.Add(labelPrice, 0, 2);
            tableLayout.Controls.Add(labelName, 0, 1);
            tableLayout.Controls.Add(labelBarcode, 0, 3);
            tableLayout.Dock = DockStyle.Top;
            tableLayout.Location = new Point(0, 0);
            tableLayout.Margin = new Padding(0);
            tableLayout.Name = "tableLayout";
            tableLayout.RowCount = 4;
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayout.RowStyles.Add(new RowStyle());
            tableLayout.RowStyles.Add(new RowStyle());
            tableLayout.Size = new Size(198, 321);
            tableLayout.TabIndex = 3;
            // 
            // labelBarcode
            // 
            labelBarcode.AutoSize = true;
            labelBarcode.Font = new Font("Cascadia Code", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelBarcode.ForeColor = SystemColors.GrayText;
            labelBarcode.Location = new Point(5, 287);
            labelBarcode.Margin = new Padding(5, 0, 0, 5);
            labelBarcode.Name = "labelBarcode";
            labelBarcode.Size = new Size(169, 29);
            labelBarcode.TabIndex = 3;
            labelBarcode.Text = "102301230120";
            // 
            // ProductControl
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ControlLight;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(tableLayout);
            Margin = new Padding(0, 0, 10, 10);
            MinimumSize = new Size(200, 200);
            Name = "ProductControl";
            Size = new Size(198, 321);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            tableLayout.ResumeLayout(false);
            tableLayout.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPrice;
        private Label labelName;
        private PictureBox pictureBox;
        private TableLayoutPanel tableLayout;
        private Label labelBarcode;
    }
}
