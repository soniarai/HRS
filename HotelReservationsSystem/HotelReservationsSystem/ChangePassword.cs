using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace HotelReservationsSystem
{
    public partial class ChangePassword : Form
    {
        const string CONNECTION_STRING = "DSN=HRS";
        /// <summary>
        /// Ctro 
        /// </summary>
        public ChangePassword()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When user clicks on OK, validate that new password
        /// and confirm password are same. Update the passord for
        /// the user if the userid and password are correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (userIDTextBox.Text.Length <= 0)
            {
                MessageBox.Show("Please enter a User ID", "Missing Userid"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (passwordTextBox.Text.Length <= 0)
            {
                MessageBox.Show("Please enter password", "Missing password"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (newPasswordTextBox.Text.Length <= 5)
            {
                MessageBox.Show("Please enter at least 5 characters for new password",
                    "New Password Too Short"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (newPasswordTextBox.Text.CompareTo(confirmPasswordTextBox.Text) != 0)
            {
                MessageBox.Show("The new password and confirm new password" +
                                " does not match", "Confirm new password"
                , MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                //Establish connection with database using OdbcConnection class
                OdbcConnection objectOdbcConnection = new OdbcConnection(CONNECTION_STRING);
                objectOdbcConnection.Open();
                //Create an OdbcCommand object using CreateCommand of the OdbcConnection object 
                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "UPDATE USERS set password = ? "
                                                + " WHERE userId = ? and password= ?;"
                                                + "SELECT password FROM USERS WHERE userid=?";
                objectOdbcCommand.Parameters.Add("newPasword", OdbcType.NVarChar).Value = newPasswordTextBox.Text;
                objectOdbcCommand.Parameters.Add("userid", OdbcType.NVarChar).Value = userIDTextBox.Text;
                objectOdbcCommand.Parameters.Add("pasword", OdbcType.NVarChar).Value = passwordTextBox.Text;
                objectOdbcCommand.Parameters.Add("userid", OdbcType.NVarChar).Value = userIDTextBox.Text;
                OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    string newPassword = dbReader["password"].ToString();
                    if (newPassword.CompareTo(confirmPasswordTextBox.Text) == 0)
                    {
                        MessageBox.Show("Password changed successfully", "Password Changed",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, password was not changed successfully", 
                                                "Password Not Changed",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                dbReader.Close();
                objectOdbcCommand.Dispose();
                objectOdbcConnection.Close();

            }

        }
    }
}
