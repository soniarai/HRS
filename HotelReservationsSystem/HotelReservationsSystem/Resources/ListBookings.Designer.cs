namespace HotelReservationsSystem
{
    partial class ListBookings
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
            this.bookingsListView = new System.Windows.Forms.ListView();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bookingsListView
            // 
            this.bookingsListView.FullRowSelect = true;
            this.bookingsListView.Location = new System.Drawing.Point(12, 54);
            this.bookingsListView.Name = "bookingsListView";
            this.bookingsListView.Size = new System.Drawing.Size(958, 242);
            this.bookingsListView.TabIndex = 0;
            this.bookingsListView.UseCompatibleStateImageBehavior = false;
            this.bookingsListView.View = System.Windows.Forms.View.Details;
            this.bookingsListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.bookingsListView_MouseDoubleClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(440, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 29);
            this.button2.TabIndex = 12;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ListBookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 346);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bookingsListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ListBookings";
            this.Text = "List of Bookings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ListBookings_FormClosed);
            this.Load += new System.EventHandler(this.ListBookings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView bookingsListView;
        private System.Windows.Forms.Button button2;

    }
}