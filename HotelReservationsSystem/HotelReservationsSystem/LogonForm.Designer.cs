namespace HotelReservationsSystem
{
    partial class LogonForm
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
            this.userIDLabel = new System.Windows.Forms.Label();
            this.userIDTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.logonPanel = new System.Windows.Forms.Panel();
            this.logonButton = new System.Windows.Forms.Button();
            this.logonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // userIDLabel
            // 
            this.userIDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userIDLabel.Location = new System.Drawing.Point(10, 9);
            this.userIDLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.userIDLabel.Name = "userIDLabel";
            this.userIDLabel.Size = new System.Drawing.Size(85, 24);
            this.userIDLabel.TabIndex = 1;
            this.userIDLabel.Text = "User ID";
            // 
            // userIDTextBox
            // 
            this.userIDTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userIDTextBox.Location = new System.Drawing.Point(131, 7);
            this.userIDTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.userIDTextBox.Name = "userIDTextBox";
            this.userIDTextBox.Size = new System.Drawing.Size(158, 26);
            this.userIDTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(131, 38);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(158, 26);
            this.passwordTextBox.TabIndex = 1;
            // 
            // passwordLabel
            // 
            this.passwordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordLabel.Location = new System.Drawing.Point(10, 41);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(85, 24);
            this.passwordLabel.TabIndex = 2;
            this.passwordLabel.Text = "Password";
            // 
            // logonPanel
            // 
            this.logonPanel.BackColor = System.Drawing.Color.Snow;
            this.logonPanel.Controls.Add(this.userIDLabel);
            this.logonPanel.Controls.Add(this.passwordTextBox);
            this.logonPanel.Controls.Add(this.userIDTextBox);
            this.logonPanel.Controls.Add(this.passwordLabel);
            this.logonPanel.Location = new System.Drawing.Point(353, 33);
            this.logonPanel.Margin = new System.Windows.Forms.Padding(2);
            this.logonPanel.Name = "logonPanel";
            this.logonPanel.Size = new System.Drawing.Size(298, 73);
            this.logonPanel.TabIndex = 0;
            // 
            // logonButton
            // 
            this.logonButton.Location = new System.Drawing.Point(572, 110);
            this.logonButton.Margin = new System.Windows.Forms.Padding(2);
            this.logonButton.Name = "logonButton";
            this.logonButton.Size = new System.Drawing.Size(70, 25);
            this.logonButton.TabIndex = 1;
            this.logonButton.Text = "LogOn";
            this.logonButton.UseVisualStyleBackColor = true;
            this.logonButton.Click += new System.EventHandler(this.logonButton_Click);
            // 
            // LogonForm
            // 
            this.AcceptButton = this.logonButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::HotelReservationsSystem.Properties.Resources.hotel_image;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(700, 465);
            this.Controls.Add(this.logonButton);
            this.Controls.Add(this.logonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "LogonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conestoga Inn - Hotel Reservation System";
            this.logonPanel.ResumeLayout(false);
            this.logonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label userIDLabel;
        private System.Windows.Forms.TextBox userIDTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Panel logonPanel;
        private System.Windows.Forms.Button logonButton;
    }
}

