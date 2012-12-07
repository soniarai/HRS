/** Welcome.cs
 *      output: This allow an authorized user to check availability, 
 *              retrieve existing booking details and go to the booking
 *              form for creating new booking or update existing bookings.
 *      Revision History
 *        Sonia, Rhema, Asifa 2012.11.04: Created
 */
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
    public partial class Welcome : Form
    {
        Booking objectBookingForm;
        ListBookings objectListBookings;
        OdbcConnection objectOdbcConnection;
        BookingInformation objectBookingInformation;

        /** Using three different HashSet to store room numbers,
         * one HashSet per color code.
         * redStatusRooms - Red status, checkout is pending immediately
         * blueStatusRooms - Blue status, guest already checked-In, room in use
         * dsBlueStatusRooms - Deep Sky Blue status, Room Booked but guest 
         *                     check-In is pending
         */
        HashSet<string> redStatusRooms = new HashSet<string>();
        HashSet<string> blueStatusRooms = new HashSet<string>();
        HashSet<string> dsBlueStatusRooms = new HashSet<string>();

        public Welcome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the booking form for the room. The code automatically 
        /// detects whether the room has an existing booking or not. It
        /// creates the BookingInformation object accordingly and pass
        /// on to the Booking form as an argument. The booking form will
        /// update the controls based on the information in the 
        /// BookingInformation object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Room_Click(object sender, EventArgs e)
        {
            //Open the booking form for this room for current day
            Button roomButton = (Button)sender;
            if (objectBookingInformation != null &&
                objectBookingInformation.bookedRoom.roomNumber != null &&
                objectBookingInformation.bookedRoom.roomNumber != roomButton.Text.Substring(0, 3))
            {
                /** BookingInformation contains info for a room which is not current room.
                 *  Invalidate this information and recreate BookingInformation object.
                 */
                objectBookingInformation = new BookingInformation();
            }
            objectBookingInformation.bookedRoom.roomNumber = roomButton.Text.Substring(0, 3);
           //Populate booking info with current booking on this room        
            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            objectOdbcCommand.CommandText = "select * from guest"
                                            + " join Booking"
                                            + " on guest.guestID = Booking.guestID"
                                            + " join Rooms on Booking.roomID = rooms.roomID"
                                            + " join Roomtype on  Roomtype.roomTypeId = rooms.roomTypeId "
                                            + " where  roomnumber = ? "
                                            + " and Booking.status != 'CHECKED OUT' "
                                            + " and ((Booking.checkIn = ? and Booking.checkOut >= ?) "
                                            + " or Booking.checkOut <= getdate())";

            objectOdbcCommand.Parameters.Add("roomnumber", OdbcType.Int).Value 
                                        = objectBookingInformation.bookedRoom.roomNumber;
            objectOdbcCommand.Parameters.Add("checkIn", OdbcType.Date).Value 
                = objectBookingInformation.checkInDate;
            objectOdbcCommand.Parameters.Add("checkOut", OdbcType.Date).Value
                = objectBookingInformation.checkOutDate;
            OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
            if (dbReader.HasRows)
            {
                // Booking exist for this, read BookingInformation object
                objectBookingInformation.readBookingObject(dbReader);
            }
            else
            {   
                // Booking doesn't exist, but update Room object in BookingInformation
                dbReader.Close();
                objectOdbcCommand.Parameters.Clear();
                objectOdbcCommand.CommandText = "Select * from Rooms "
                                + " join Roomtype on  Roomtype.roomTypeId = rooms.roomTypeId "
                                + " where  roomnumber = ? ";
                objectOdbcCommand.Parameters.Add("roomnumber", OdbcType.Int).Value
                                      = objectBookingInformation.bookedRoom.roomNumber;
                dbReader = objectOdbcCommand.ExecuteReader();
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    objectBookingInformation.bookedRoom.roomId = dbReader["roomId"].ToString();
                    objectBookingInformation.bookedRoom.roomRate = Convert.ToDouble(dbReader["roomPrice"].ToString());
                    objectBookingInformation.bookedRoom.roomType = dbReader["roomType"].ToString();
                    objectBookingInformation.bookedRoom.roomFloor = dbReader["roomFloor"].ToString();
                    objectBookingInformation.bookedRoom.roomDescription = dbReader["roomDescription"].ToString();
                }
            }


            dbReader.Close();
            objectBookingForm = new Booking(this, objectOdbcConnection, objectBookingInformation);
            objectBookingForm.Show();
            this.Hide();
        }
        /// <summary>
        /// When closing this form, close the connection opened
        /// via OdbcConnection object and exit the application 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Welcome_FormClosed(object sender, FormClosedEventArgs e)
        {
            objectOdbcConnection.Close();
            Application.Exit();
        }
        /// <summary>
        /// Establish the database connection using object of
        /// OdbcConnection class
        /// Get the color status information for all the rooms and
        /// change room colors to reflect the booking status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Welcome_Load(object sender, EventArgs e)
        {
            objectOdbcConnection = new OdbcConnection("DSN=HRS");
            objectOdbcConnection.Open();
            //set check out to next day by default
            checkOutDateTimePicker.Value = DateTime.Today.AddDays(1);
            objectBookingInformation = new BookingInformation();
            getRoomColorStatus();
            setColorStatusForRooms();
        }
        /// <summary>
        /// Using database query, check the room status and add to to 
        /// the corresponding HashSet indicating their color status.
        /// Red status - checkout is pending for this room
        /// Blue status - guest already checked-In, room in use
        /// Deep Sky Blue status -  Room Booked but guest 
        ///                         check-In is pending
        /// </summary>
        public void getRoomColorStatus()
        {
            redStatusRooms.Clear();
            blueStatusRooms.Clear();
            dsBlueStatusRooms.Clear();
   
            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            objectOdbcCommand.CommandText 
                = "select * from Booking"
                + " join Rooms on Booking.roomID = rooms.roomID"
                + " where Booking.checkOut <= GETDATE() and Booking.status != 'CHECKED OUT'";

            OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
            while (dbReader.Read())
            {
                redStatusRooms.Add(dbReader["roomNumber"].ToString());
            }

            dbReader.Close();
            objectOdbcCommand.CommandText = "select * from Booking"
                                            + " join Rooms on Booking.roomID = rooms.roomID"
                                            + " where (checkIn = getdate() or  "
                                            + " getdate() between checkIn and checkOut) "
                                            + " and status = 'CHECKED IN'";
            dbReader = objectOdbcCommand.ExecuteReader();
            while (dbReader.Read())
            {
                blueStatusRooms.Add(dbReader["roomNumber"].ToString());
            }

            dbReader.Close();
            objectOdbcCommand.CommandText = "select * from Booking"
                                            + " join Rooms on Booking.roomID = rooms.roomID"
                                            + " where (checkIn = getdate() or  "
                                            + " getdate() between checkIn and checkOut) "
                                            + " and status = 'BOOKED'";
            dbReader = objectOdbcCommand.ExecuteReader();
            while (dbReader.Read())
            {
                dsBlueStatusRooms.Add(dbReader["roomNumber"].ToString());
            }
            dbReader.Close();
        }
        /// <summary>
        /// Update all the rooms and set colors as per the 
        /// by calling setColorStatusForRoom for each room
        /// </summary>
        public void setColorStatusForRooms()
        {
            foreach (TabPage tb in tabControl1.TabPages)
                foreach (Control ctr in tb.Controls)
                    try
                    {
                        setColorStatusForRoom((Button)ctr);
                    }
                    catch
                    {
                        //Just catch the exception and let the loop continue
                    }
        }
        /// <summary>
        /// Update this rooms color based on it presence in one of
        /// the three HashSet indicating color status for rooms
        /// </summary>
        /// <param name="roomButton"></param>
        public void setColorStatusForRoom(Button roomButton)
        {
            try
            {
                if (redStatusRooms.Contains(roomButton.Text.Substring(0, 3)))
                {
                    roomButton.BackColor = Color.Tomato;
                    roomButton.ForeColor = Color.White;
                }
                else if (blueStatusRooms.Contains(roomButton.Text.Substring(0, 3)))
                {
                    roomButton.BackColor = Color.MediumSlateBlue;
                    roomButton.ForeColor = Color.White;
                }
                else if (dsBlueStatusRooms.Contains(roomButton.Text.Substring(0, 3)))
                {
                    roomButton.BackColor = Color.SkyBlue;
                    roomButton.ForeColor = Color.White;
                }
                else if (int.Parse(roomButton.Text.Substring(0, 3)) != 0)
                {
                    roomButton.BackColor = Color.MediumSeaGreen;
                    roomButton.ForeColor = Color.White;
                }
            }
            catch
            {
                roomButton.Enabled = false;
            }
        }

        /// <summary>
        /// Retrieve booking information for user.
        /// Display user's information in a list if more than one 
        /// matching results is found. Once a guest is selected from that
        /// list, it will open the booking form with the selected information.
        /// If only one matching result, directly open the booking form for 
        /// the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void retrieveBookingButton_Click(object sender, EventArgs e)
        {
            if (lastNameTextBox.Text.Length <= 0)
            {
                MessageBox.Show("Please enter last name of the guest to search for their booking",
                                   "Last Name Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lastNameTextBox.Focus();
                return;
            }
            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            objectOdbcCommand.CommandText = "select * from guest"
                                            + " join Booking"
                                            + " on guest.guestID = Booking.guestID"
                                            + " join Rooms on Booking.roomID = rooms.roomID"
                                            + " join Roomtype on  Roomtype.roomTypeId = rooms.roomTypeId "
                                            + " where ";
                                           
            if (firstNameTextBox.Text.Length > 0)
            {
                objectOdbcCommand.CommandText += " guestFName = ? and ";
                objectOdbcCommand.Parameters.Add("guestFname", OdbcType.NVarChar).Value = firstNameTextBox.Text;
            }
            objectOdbcCommand.CommandText += " guestLName = ?";
            objectOdbcCommand.Parameters.Add("guestLname", OdbcType.NVarChar).Value = lastNameTextBox.Text;

            OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
            HashSet<BookingInformation> bookings = new HashSet<BookingInformation>();

            while (dbReader.Read())
            {
                BookingInformation objectBookingInformation = new BookingInformation();
                objectBookingInformation.readBookingObject(dbReader);
               bookings.Add(objectBookingInformation);
            }

            dbReader.Close();
            objectOdbcCommand.Dispose();

            switch(bookings.Count)
            {
                case 0:
                    MessageBox.Show("No booking found for this guest");
                    /*
                    BookingInformation objBookingInfo = new BookingInformation();
                    objectBookingForm = new Booking(this, objectOdbcConnection, objBookingInfo);
                    objectBookingForm.Show();
                    this.Hide();*/
                    break;
                case 1:
                    //MessageBox.Show("one booking found");
                    objectBookingForm = new Booking(this, objectOdbcConnection, bookings.ElementAt(0));
                    objectBookingForm.Show();
                    this.Hide();
                    break;
                default:
                    //MessageBox.Show("More than one booking found");
                    objectListBookings = new ListBookings(this, objectOdbcConnection, bookings);
                    objectListBookings.Show();
                    this.Hide();
                    break;
            }
        }

        /// <summary>
        /// Check a rooms availability based on user's preference.
        /// All the rooms which doesn't fulfill user's preference
        /// will be disable. Booking can be made only on the rooms
        /// with "Green" color status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAvailabilityButton_Click(object sender, EventArgs e)
        {
            string type = roomTypeComboBox.Text;
            string roomLevel = roomLevelComboBox.Text;
            string checkInDate = checkInDateTimePicker.Text;
            string checkOutDate = checkOutDateTimePicker.Text;

            HashSet<string> hashSet = new HashSet<string>();
            objectBookingInformation = new BookingInformation();
            
            //Initialize connection for ODBC
            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            string strCommand = "select * from Rooms left join RoomType on "
                                            + "rooms.roomTypeID = RoomType.roomTypeID "
                                            + "left join Booking on "
                                            + "rooms.roomID = Booking.roomId where "
                                            + "(BookingID is null or "
                                            + " Booking.status = 'CHECKED OUT' or "
                                            + " not (checkIn between ? and ? ) "
                                            + " and not (checkOut between ? and ?))";

            objectOdbcCommand.Parameters.Add("DATE", OdbcType.Date).Value = checkInDateTimePicker.Value;
            objectOdbcCommand.Parameters.Add("DATE", OdbcType.Date).Value = checkOutDateTimePicker.Value;
            objectOdbcCommand.Parameters.Add("DATE", OdbcType.Date).Value = checkInDateTimePicker.Value;
            objectOdbcCommand.Parameters.Add("DATE", OdbcType.Date).Value = checkOutDateTimePicker.Value;

            objectBookingInformation.checkInDate = checkInDateTimePicker.Value;
            objectBookingInformation.checkOutDate = checkOutDateTimePicker.Value;

            if (type.Length > 0)
            {
                strCommand += " and roomtype = ?";
                objectOdbcCommand.Parameters.Add("roomtpe", OdbcType.NVarChar).Value = type;
                objectBookingInformation.bookedRoom.roomType = type;
            }
            if (roomLevel.Length > 0)
            {
                strCommand += " and roomFloor = ?";
                objectOdbcCommand.Parameters.Add("roomLevel", OdbcType.Int).Value = roomLevel;
                //objectBookingInformation.bookedRoom.roomLevel = roomLevel;
            }
            objectOdbcCommand.CommandText = strCommand;
            OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();

            while (dbReader.Read())
            {
                string roomNumber = dbReader["roomNumber"].ToString().Trim();
                hashSet.Add(roomNumber);

            }
            dbReader.Close();
            objectOdbcCommand.Dispose();

            //Following is logic to enable/disable 
            if (roomLevelComboBox.Text.Length > 0)
            {
                int floor = int.Parse(roomLevelComboBox.Text) - 1;
                tabControl1.SelectedIndex = floor;
            }
            //Iterate thru all the TabPages and then controls on each TabPage
            foreach (TabPage tb in tabControl1.TabPages)
                foreach (Control ctr in tb.Controls)
                {
                    try
                    {
                        if (hashSet.Contains(ctr.Text.Substring(0, 3)))
                        { 
                            ctr.Enabled = true;
                            setColorStatusForRoom((Button)ctr);
                        }
                        else
                        {
                            ctr.Enabled = false;
                            ctr.BackColor = Color.WhiteSmoke;
                        }
                    }
                    catch
                    {
                        ctr.Enabled = false;
                    }

                }
            hashSet.Clear();
        }

        /// <summary>
        /// Reset color status for all the rooms and
        /// enable all rooms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            
            roomTypeComboBox.SelectedIndex = -1;
            roomLevelComboBox.SelectedIndex = -1;
            objectBookingInformation = new BookingInformation();
            foreach (TabPage tb in tabControl1.TabPages)
                foreach (Control ctr in tb.Controls)
                    ctr.Enabled = true;
            setColorStatusForRooms();
        }

        /// <summary>
        /// Close the OdbcConnection and Exit the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectOdbcConnection.Close();
            Application.Exit();
        }

        /// <summary>
        /// Show About Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "HotelReservationSystem.chm");
        }

        private void clearNamesButton_Click(object sender, EventArgs e)
        {
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
        }
    }
}