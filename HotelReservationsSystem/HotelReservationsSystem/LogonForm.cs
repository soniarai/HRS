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
        
        Welcome objectWelcomeForm;
    

        public LogonForm()
        {
            InitializeComponent();

        }

        private void logonButton_Click(object sender, EventArgs e)
        {
            try
            {
                /* Establish a DSN connection with HotelReservationSystem database using 
                 * ODBCConnection class
                 */
                OdbcConnection objectOdbcConnection = new OdbcConnection("DSN=HRS");
                objectOdbcConnection.Open();

                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "SELECT * FROM USERS WHERE userId = ? and password= ?";
                objectOdbcCommand.Parameters.Add("userid", OdbcType.NVarChar).Value = userIDTextBox.Text;
                objectOdbcCommand.Parameters.Add("pasword", OdbcType.NVarChar).Value = passwordTextBox.Text;
                OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    //This is a vlid userid/password, show the welcome page
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
    }
}