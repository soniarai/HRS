/** LogonForm.cs
 *      Output: Allow a user to logon to the application
 *              after validating user's credentials.
 *              
 *      Revision History
 *          Sonia, Rhema, Syeda Asifa 2012.11.04: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc; // using this namespace to use ODBC classes


namespace HotelReservationsSystem
{
    public partial class LogonForm : Form
    {
        /** Connection string to connect to HoterlReservationSystem database using
         * ODBC DSN source, defined as a constant
         */
        const string CONNECTION_STRING = "DSN=HRS";

        public LogonForm()
        {
            InitializeComponent();

        }

        /// <summary>
        ///  Establish a DSN connection with HotelReservationSystem database using 
        ///  ODBCConnection class and validate userID and Password. If a user is
        ///  authorized, it logon the user to the Welcome page of the HotelReservationSystem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logonButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Establish connection with database using OdbcConnection class
                OdbcConnection objectOdbcConnection = new OdbcConnection(CONNECTION_STRING);
                objectOdbcConnection.Open();
                //Create an OdbcCommand object using CreateCommand of the OdbcConnection object 
                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "SELECT * FROM USERS WHERE userId = ? and password= ?";
                objectOdbcCommand.Parameters.Add("userid", OdbcType.NVarChar).Value = userIDTextBox.Text;
                objectOdbcCommand.Parameters.Add("pasword", OdbcType.NVarChar).Value = passwordTextBox.Text;
                OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    //This is a valid userId/password, show the welcome page
                    Welcome objectWelcomeForm;
                    objectWelcomeForm = new Welcome();
                    objectWelcomeForm.Show();
                    this.Hide();
                    dbReader.Close();
                    objectOdbcCommand.Dispose();
                    objectOdbcConnection.Close();
                    return;
                }
                //The user id or password is not valid
                dbReader.Close();
                objectOdbcCommand.Dispose();
                objectOdbcConnection.Dispose();
                objectOdbcConnection.Close();
                MessageBox.Show("Please enter a valid user and password");
            }
            catch (Exception exp)
            {
                MessageBox.Show("An exception in the code: \n" + exp.Message.ToString());
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            userIDTextBox.Clear();
            passwordTextBox.Clear();
        }

        private void changePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePassword changePasswordDialog = new ChangePassword();
            changePasswordDialog.ShowDialog();
        }
    }
}