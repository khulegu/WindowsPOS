namespace POSForm
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            passwordTextBox = new TextBox();
            loginButton = new Button();
            label1 = new Label();
            usernameTextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(43, 266);
            passwordTextBox.Margin = new Padding(6, 7, 6, 7);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(387, 39);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // loginButton
            // 
            loginButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            loginButton.Location = new Point(166, 359);
            loginButton.Margin = new Padding(6, 7, 6, 7);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(264, 60);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += LoginButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(43, 42);
            label1.Name = "label1";
            label1.Size = new Size(122, 51);
            label1.TabIndex = 4;
            label1.Text = "Login";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(43, 175);
            usernameTextBox.Margin = new Padding(6, 7, 6, 7);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(387, 39);
            usernameTextBox.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 136);
            label2.Name = "label2";
            label2.Size = new Size(121, 32);
            label2.TabIndex = 7;
            label2.Text = "Username";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(43, 227);
            label3.Name = "label3";
            label3.Size = new Size(111, 32);
            label3.TabIndex = 8;
            label3.Text = "Password";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(490, 472);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(loginButton);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(label1);
            Margin = new Padding(6, 7, 6, 7);
            Name = "LoginForm";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private Label label1;
        private TextBox usernameTextBox;
        private Label label2;
        private Label label3;
    }
}
