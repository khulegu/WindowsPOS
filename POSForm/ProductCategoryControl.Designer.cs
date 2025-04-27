namespace POSForm.Controls
{
    partial class ProductCategoryControl
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
            nameLabel = new Label();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(0, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(78, 32);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "label1";
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ProductCategoryControl
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(nameLabel);
            Name = "ProductCategoryControl";
            Size = new Size(250, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nameLabel;
    }
}
