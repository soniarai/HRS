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
    public partial class ListBookings : Form
    {
        Welcome objectWelcomeForm;
        OdbcConnection objectOdbcConnection;
        Booking objectBookingForm;

        public ListBookings()
        {
            InitializeComponent();
        }
        public ListBookings(Welcome welcomeForm, OdbcConnection odbcConnection, HashSet<BookingInformation> bookings)
            : this()
        {
            objectWelcomeForm = welcomeForm;
            objectOdbcConnection = odbcConnection;
            bookingsListView.Columns.Add("Guest ID");
            bookingsListView.Columns.Add("First Name");
            bookingsListView.Columns.Add("Last Name");
            bookingsListView.Columns.Add("Phone");
            bookingsListView.Columns.Add("CheckIn Date");
            bookingsListView.Columns.Add("CheckOut Date");
            bookingsListView.Columns.Add("Room Number");

            bookingsListView.Columns[0].Width = 125;
            bookingsListView.Columns[1].Width = 125;
            bookingsListView.Columns[2].Width = 125;
            bookingsListView.Columns[3].Width = 125;
            bookingsListView.Columns[4].Width = 125;
            bookingsListView.Columns[5].Width = 125;
            bookingsListView.Columns[6].Width = 125;
            
            //bookingsListBox.Items.Add(lv);
            for(int i= 0; i<bookings.Count; i++)
            {
                string[] listColumns = new string[7];
                listColumns[0] = bookings.ElementAt(i).bookedGuest.guestId;
                listColumns[1] = bookings.ElementAt(i).bookedGuest.firstName;
                listColumns[2] = bookings.ElementAt(i).bookedGuest.lastName;
                listColumns[3] = bookings.ElementAt(i).bookedGuest.phone;
                listColumns[4] = bookings.ElementAt(i).checkInDate.ToShortDateString();
                listColumns[5] = bookings.ElementAt(i).checkOutDate.ToShortDateString();
                listColumns[6] = bookings.ElementAt(i).bookedRoom.roomNumber;
                ListViewItem li = new ListViewItem(listColumns);
                bookingsListView.Items.Add(li);
            }
        }

        private void ListBookings_Load(object sender, EventArgs e)
        {

        }

        private void ListBookings_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }

        private void bookingsListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(bookingsListView.SelectedIndices.Count >= 0)
            {
                BookingInformation objectBookingInformation = new BookingInformation();
                // if only one matching result, open the booking form for that user
                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "select * from guest"
                                                + " join Booking"
                                                + " on guest.guestID = Booking.guestID"
                                                + " join Rooms on Booking.roomID = rooms.roomID"
                                                + " join Roomtype on  Roomtype.roomTypeId = rooms.roomTypeId "
                                                + " where guest.guestId = ? and rooms.roomnumber = ? ";
                //ListViewItem li = bookingsListView.
                string guestId = bookingsListView.SelectedItems[0].SubItems[0].Text;
                string roomNumber = bookingsListView.SelectedItems[0].SubItems[6].Text;
                

                objectOdbcCommand.Parameters.Add("guestId", OdbcType.NVarChar).Value = guestId;
                objectOdbcCommand.Parameters.Add("roomnumber", OdbcType.Int).Value = roomNumber;
                //objectOdbcCommand.Parameters.Add("guestLname", OdbcType.NVarChar).Value = textBox2.Text;

                OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
                dbReader.Read();
                objectBookingInformation.readBookingObject(dbReader);
                dbReader.Close();
                objectOdbcCommand.Dispose();

                objectBookingForm = new Booking(objectWelcomeForm, objectOdbcConnection, objectBookingInformation);
                objectBookingForm.Show();
                this.Close();
            }
            else{
            }
        }

    }
}
