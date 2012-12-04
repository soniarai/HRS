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
    public partial class Booking : Form
    {
        Welcome objWelcomeForm;
        OdbcConnection objectOdbcConnection;
        BookingInformation objectBookingInformation;

        const string CHECKED_OUT = "CHECKED OUT";
        const string CHECKED_IN = "CHECKED IN";
        const string BOOKED = "BOOKED";

        public Booking()
        {
            InitializeComponent();
        }
        /** Overloaded Constructor: Booking(Welcome welcomeForm, 
         *                                  OdbcConnection odbcConnection, 
         *                                  BookingInformation bookingInformation)
         *  This constructor will accept object of Welcome class, an opened OdbcConnection
         *  and BookingInformation object as input. When this form is closed, it uses the 
         *  Welcome object passed on via this constructor to "show" the hidden Welcome form.
         *  
         */
        public Booking(Welcome welcomeForm, 
                        OdbcConnection odbcConnection, 
                        BookingInformation bookingInformation)
            : this()
        {
            objWelcomeForm = welcomeForm;
            objectOdbcConnection = odbcConnection;
            objectBookingInformation = bookingInformation;
        }

        /** Function updateControls()
         *      Return type - Void
         *      Input - no input
         *    This function update controls on the Booking based on the current 
         *    booking status set in the statusTextBox
         *    
         */
        private void updateControls()
        {
            string statusString = statusTextBox.Text.Trim().ToUpper();

            //Make controls Read only by default
            guestIDTextBox.ReadOnly = true;
            firstNameTextBox.ReadOnly = true;
            lastNameTextBox.ReadOnly = true;
            phoneTextBox.ReadOnly = true;
            addressTextBox.ReadOnly = true;
            emailTextBox.ReadOnly = true;
            statusTextBox.ReadOnly = true;

            switch (statusString)
            {
                case "": //new booking - need to enable controls
                    guestIDTextBox.ReadOnly = false;
                    firstNameTextBox.ReadOnly = false;
                    lastNameTextBox.ReadOnly = false;
                    phoneTextBox.ReadOnly = false;
                    addressTextBox.ReadOnly = false;
                    emailTextBox.ReadOnly = false;
                    statusTextBox.ReadOnly = false;

                    saveBookingButton.Enabled = true;
                    checkInButton.Enabled = false;
                    addChargesButton.Enabled = false;
                    checkOutButton.Enabled = false;
                    modifyButton.Enabled = false;
                    break;
                case BOOKED://Existing booking - guest not checked-in yet
                    saveBookingButton.Enabled = false;
                    checkInButton.Enabled = true;
                    addChargesButton.Enabled = false;
                    checkOutButton.Enabled = false;
                    modifyButton.Enabled = false;
                    break;
                case CHECKED_IN://Existing booking - guest checked-in
                    saveBookingButton.Enabled = false;
                    checkInButton.Enabled = false;
                    addChargesButton.Enabled = true;
                    checkOutButton.Enabled = true;
                    modifyButton.Enabled = true;
                    break;
                case CHECKED_OUT://Guest checked out
                    saveBookingButton.Enabled = false;
                    checkInButton.Enabled = false;
                    addChargesButton.Enabled = true;
                    checkOutButton.Enabled = false;
                    modifyButton.Enabled = false;
                    break;
                default:
                    break;
            }
            //Get color code information for each room
            objWelcomeForm.getRoomColorStatus();
            //update each room with corresponding color code
            objWelcomeForm.setColorStatusForRooms();
        }

        /** Function: Booking_Load
         *      Update onctrols on the form using information
         *      in the BookingInformation object. Call updateControls() 
         *      function to update the controls as per the logic defined 
         *      in updateContorl() function.
         */
        private void Booking_Load(object sender, EventArgs e)
        {
            //Update controls with information stored in BookingInformation object.
            bookingIDTextBox.Text = objectBookingInformation.bookingID;
            statusTextBox.Text = objectBookingInformation.status;
            guestIDTextBox.Text = objectBookingInformation.bookedGuest.guestId;
            firstNameTextBox.Text = objectBookingInformation.bookedGuest.firstName;
            lastNameTextBox.Text = objectBookingInformation.bookedGuest.lastName;
            phoneTextBox.Text = objectBookingInformation.bookedGuest.phone;
            addressTextBox.Text = objectBookingInformation.bookedGuest.address;
            emailTextBox.Text = objectBookingInformation.bookedGuest.email;
            roomNumberTextBox.Text = objectBookingInformation.bookedRoom.roomNumber;
            roomTypeTextBox.Text = objectBookingInformation.bookedRoom.roomType;
            roomRateTextBox.Text = objectBookingInformation.bookedRoom.roomRate.ToString();
            roomDescriptionTextBox.Text = objectBookingInformation.bookedRoom.roomDescription;
            floorTextBox.Text = objectBookingInformation.bookedRoom.roomFloor;
            checkInDateTimePicker.Value = objectBookingInformation.checkInDate;
            checkOutDateTimePicker.Value = objectBookingInformation.checkOutDate;
            specialRequestTextBox.Text = objectBookingInformation.specialRequest;
            //Set user's identity verification status
            switch(objectBookingInformation.bookedGuest.identityStatus.Trim())
            {
                case "":
                case "NOT VERIFIED":
                    notVerifiedRadioButton.Checked = true;
                    break;
                case "DRIVING LICENSE":
                    drivingLicenseRadioButton.Checked = true;
                        break;
                case "PASSPORT":
                        passportRadioButton.Checked = true;
                    break;
                case "CITIZENSHIP CARD":
                    citizenshipCardRadioButton.Checked = true;
                    break;
                default:
                    otherRadioButton.Checked = true;
                    idOthersTextBox.Text = objectBookingInformation.bookedGuest.identityStatus;
                    break;
            }
            //Update applicable charges
            roomServiceTextBox.Text = objectBookingInformation.roomServiceCharges.ToString();
            restaurantTextBox.Text = objectBookingInformation.restaurantCharges.ToString();
            totalTextBox.Text = objectBookingInformation.getTotalCharges().ToString();
            //Selected credit card type
            switch(objectBookingInformation.bookedGuest.creditCardType.Trim())
            {
                case "Visa":
                    creditCardTypeComboBox.SelectedIndex = 0;
                    break;
                case "Master":
                    creditCardTypeComboBox.SelectedIndex = 1;
                    break;
                case "Amex":
                    creditCardTypeComboBox.SelectedIndex = 2;
                    break;
            }
            //Display only last 4 digits of creditcard
            if(objectBookingInformation.bookedGuest.creditCardNumber != null)
                creditCardNoTextBox.Text = "****-****-****-" 
                                + objectBookingInformation.bookedGuest.creditCardNumber.Substring(11, 4);
            //Payment status
            if (objectBookingInformation.paymentStatus.Trim() == "Paid")
                paymentStatusComboBox.SelectedIndex = 1;
            else
                paymentStatusComboBox.SelectedIndex = 0;

            updateControls();
        }

        private void Booking_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Show the hidden Welcome form on closing Booking form
            objWelcomeForm.Show();
        }

        /** Function saveBookingButton_Click
         *      This function is called when user click on "saveBookingButton"
         *      If this is an existing booking, it updates the booking with changes. 
         *      Else it creates a new booking.
         *      After updating or creating booking, it update controls on Booking form
         *      and color code information for the Welcome form
         */
        private void saveBookingButton_Click(object sender, EventArgs e)
        {
            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            if (objectBookingInformation.bookedGuest.guestId != null 
                && objectBookingInformation.bookedGuest.guestId.Length > 0)
            {//update existing guest
              
                objectOdbcCommand.CommandText = "update guest set guestFname = ?, "
                                        + " guestLname = ?, address = ? , telephone = ?, "
                                        + " email = ?, identityStatus =?, creditCardType = ?, "
                                        + " creditCardNumber=? where guestid = ? ";

            }
            else
            {//insert new guest
               objectOdbcCommand.CommandText ="insert into guest (guestFname,"
                                       + " guestLname, address, telephone,"
                                       + " email, identityStatus,creditCardType, creditCardNumber)"
                                       + " values(?,?,?,?,?,?,?,?);"
                                       + " select scope_identity()";
            }
            //Add input parameters to the OdbcCommand object 
            objectOdbcCommand.Parameters.Add("guestFname", OdbcType.NVarChar).Value 
                = firstNameTextBox.Text.Trim().ToUpper(); //First name

            objectOdbcCommand.Parameters.Add("guestLname", OdbcType.NVarChar).Value 
                = lastNameTextBox.Text.Trim().ToUpper(); //Last name

            objectOdbcCommand.Parameters.Add("address", OdbcType.NVarChar).Value 
                = addressTextBox.Text.Trim().ToUpper(); //Address

            objectOdbcCommand.Parameters.Add("telephone", OdbcType.NVarChar).Value 
                = phoneTextBox.Text.Trim().ToUpper(); //Telephone

            objectOdbcCommand.Parameters.Add("email", OdbcType.NVarChar).Value 
                = emailTextBox.Text.Trim().ToLower(); //Email

            string identityVerification = "";
            //Get selectected option for identity verification
            if (notVerifiedRadioButton.Checked == true)
                identityVerification = notVerifiedRadioButton.Text;
            else if (drivingLicenseRadioButton.Checked == true)
                identityVerification = drivingLicenseRadioButton.Text;
            else if (passportRadioButton.Checked == true)
                identityVerification = passportRadioButton.Text;
            else if (citizenshipCardRadioButton.Checked == true)
                identityVerification = citizenshipCardRadioButton.Text;
            else if (otherRadioButton.Checked == true)
                identityVerification = idOthersTextBox.Text;
            else
                identityVerification = notVerifiedRadioButton.Text;

            objectOdbcCommand.Parameters.Add("identification", OdbcType.NVarChar).Value 
                = identityVerification.Trim().ToUpper(); //Identity verification
            objectOdbcCommand.Parameters.Add("CreditCardType", OdbcType.NVarChar).Value
                = creditCardTypeComboBox.Text; //Credit card type
            //If credit card is masked, get credit card number from BookingInformation object
            if (creditCardNoTextBox.Text.IndexOf("*") < 0)
                objectOdbcCommand.Parameters.Add("CreditCardNumber", OdbcType.NVarChar).Value =
                            creditCardNoTextBox.Text.Trim().Replace("-",""); 
            else
                objectOdbcCommand.Parameters.Add("CreditCardNumber", OdbcType.NVarChar).Value =
                           objectBookingInformation.bookedGuest.creditCardNumber;

            if(guestIDTextBox.Text.Length > 0)
                objectOdbcCommand.Parameters.Add("guestid", OdbcType.Int).Value 
                    = Convert.ToInt32(guestIDTextBox.Text);

            OdbcDataReader dbReader = objectOdbcCommand.ExecuteReader();
            dbReader.Read();
            if(dbReader.HasRows)
                guestIDTextBox.Text = dbReader[0].ToString();
            dbReader.Close();
            objectOdbcCommand.Parameters.Clear();

            if(bookingIDTextBox.Text.Length >0)
            { //Existing Booking
                objectOdbcCommand.CommandText = "update booking set payStatus=?, status=? where "
                                                 + " bookingId = ?";
                objectOdbcCommand.Parameters.Add("payStatus", OdbcType.NVarChar).Value 
                    = paymentStatusComboBox.Text;
                objectOdbcCommand.Parameters.Add("status", OdbcType.NVarChar).Value 
                    = statusTextBox.Text.Trim().ToUpper();
                objectOdbcCommand.Parameters.Add("bookingID", OdbcType.NVarChar).Value 
                    = bookingIDTextBox.Text.Trim();
            }
            else
            { //New Booking
                objectOdbcCommand.CommandText 
                    = "insert into booking(status, checkIn, checkOut,specialRequest, "
                        + "roomServCharges, restaurantCharges,totalPrice, guestID, roomID,payStatus) "
                        + " values ( 'BOOKED',?,?,?,?,?,?,?,?,?);"
                        +" select scope_identity()";
                
                objectOdbcCommand.Parameters.Add("checkIn", OdbcType.Date).Value 
                    = checkInDateTimePicker.Value;
                objectOdbcCommand.Parameters.Add("checkOut", OdbcType.Date).Value 
                    = checkOutDateTimePicker.Value;
                objectOdbcCommand.Parameters.Add("specialRequest", OdbcType.NVarChar).Value 
                    = specialRequestTextBox.Text;
                objectOdbcCommand.Parameters.Add("roomServCharges", OdbcType.Decimal).Value 
                    = Convert.ToDecimal(0+roomServiceTextBox.Text);
                objectOdbcCommand.Parameters.Add("restaurantCharges", OdbcType.Decimal).Value
                    = Convert.ToDecimal(0+restaurantTextBox.Text);
                objectOdbcCommand.Parameters.Add("totalPrice", OdbcType.Decimal).Value 
                    = Convert.ToDecimal(0+totalTextBox.Text);
                objectOdbcCommand.Parameters.Add("guestId", OdbcType.Int).Value
                    = Convert.ToInt32(guestIDTextBox.Text);
                objectOdbcCommand.Parameters.Add("roomId", OdbcType.Int).Value 
                    = objectBookingInformation.bookedRoom.roomId;
                objectOdbcCommand.Parameters.Add("paystatus", OdbcType.NVarChar).Value 
                    = paymentStatusComboBox.Text;
            }
            dbReader = objectOdbcCommand.ExecuteReader();
            dbReader.Read();
            if (dbReader.HasRows)
            {
                bookingIDTextBox.Text = dbReader[0].ToString();
                statusTextBox.Text = BOOKED;
            }
            dbReader.Close();
            objectOdbcCommand.Dispose();

            updateControls();

        }
        /** Function: checkInButton_Click
         *    Verify the customers identity and check-In
         */
        private void checkInButton_Click(object sender, EventArgs e)
        {
            
            if (checkInDateTimePicker.Value > DateTime.Today)
            {//Do not allow to check-in a guest for which the check-in date is in future
                MessageBox.Show("Sorry, you can not checkin a customer with furture check-In date",
                    "Future Check-In date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (notVerifiedRadioButton.Checked == true)
            { //Ensure that identity is verified for the guest
                MessageBox.Show("Please verify the guest using a valid ID before checking In",
                    "Verify Guest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "update booking set status = 'CHECKED IN' where"
                                                + " bookingId = ?";
                objectOdbcCommand.Parameters.Add("bookingID", OdbcType.NVarChar).Value
                    = bookingIDTextBox.Text.Trim();
                objectOdbcCommand.ExecuteNonQuery();

                statusTextBox.Text = CHECKED_IN;
                updateControls();
            }
        }
        /** Function addChargesButton_Click
         *      It is called when user click on "addChargesButton" button. It will
         *      show the AddCharges form as a modal dialog. The AddCharges form
         *      allow hotel staff to add Room Service charges or Restaurant charges for
         *      the selected booking.
         */
        private void addChargesButton_Click(object sender, EventArgs e)
        {
            //Display AddCharges form as modal dialog
            AddCharges objectAddCharges = new AddCharges(objectBookingInformation);
            objectAddCharges.ShowDialog();
            
            restaurantTextBox.Text = objectBookingInformation.restaurantCharges.ToString();
            roomServiceTextBox.Text = objectBookingInformation.roomServiceCharges.ToString();
            totalTextBox.Text = objectBookingInformation.getTotalCharges().ToString();

            OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
            objectOdbcCommand.CommandText = "update booking set roomServCharges=?, restaurantCharges=?, "
                                            + " totalPrice = ? where"
                                            + " bookingId = ?";
            objectOdbcCommand.Parameters.Add("roomServCharges", OdbcType.Decimal).Value 
                = Convert.ToDecimal(roomServiceTextBox.Text.Trim());
            objectOdbcCommand.Parameters.Add("restaurantCharges", OdbcType.Decimal).Value 
                = Convert.ToDecimal(restaurantTextBox.Text.Trim());
            objectOdbcCommand.Parameters.Add("totalPrice", OdbcType.Decimal).Value 
                = Convert.ToDecimal(totalTextBox.Text.Trim());
            objectOdbcCommand.Parameters.Add("bookingID", OdbcType.NVarChar).Value 
                = bookingIDTextBox.Text.Trim();
            objectOdbcCommand.ExecuteNonQuery();

        }

        private void modifyButton_Click(object sender, EventArgs e)
        {
            //Enable text boxes to get guest inforamtion
            guestIDTextBox.ReadOnly = false;
            firstNameTextBox.ReadOnly = false;
            lastNameTextBox.ReadOnly = false;
            phoneTextBox.ReadOnly = false;
            addressTextBox.ReadOnly = false;
            emailTextBox.ReadOnly = false;
            statusTextBox.ReadOnly = false;
            //Enable save button
            saveBookingButton.Enabled = true;
            //Disable other buttons
            checkInButton.Enabled = false;
            addChargesButton.Enabled = false;
            checkOutButton.Enabled = false;
            modifyButton.Enabled = false;
        }

        /** Function checkOutButton_Click
         *      It is called when user click on "checkOutButton" button.
         *      It allow hotel staff to check out a guest once they accept the
         *      pending payments and mark it as paid for that guest.
         */
        private void checkOutButton_Click(object sender, EventArgs e)
        {
            //Confirm the identity of the user and update the status in the database
            if (paymentStatusComboBox.Text.ToUpper() != "PAID")
            {
                MessageBox.Show("Please accept the final payment from the guest and mark it as paid",
                    "Check Out - Accept Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                OdbcCommand objectOdbcCommand = objectOdbcConnection.CreateCommand();
                objectOdbcCommand.CommandText = "update booking set status = 'CHECKED OUT' where"
                                                + " bookingId = ?";
                objectOdbcCommand.Parameters.Add("bookingID", OdbcType.NVarChar).Value 
                    = bookingIDTextBox.Text.Trim();
                objectOdbcCommand.ExecuteNonQuery();

                statusTextBox.Text = CHECKED_OUT;
                updateControls();
              }

        }
   
    }

    public struct Guest
    {
        public string guestId;
        public string firstName;
        public string lastName;
        public string phone;
        public string address;
        public string email;
        public string identityStatus;
        public string creditCardType;
        public string creditCardNumber;
    }
    public struct Room
    {
        public string roomId;
        public string  roomNumber;
        public string  roomType;
        public string  roomDescription;
        public double roomRate;
        public string roomFloor;
    }

    public class BookingInformation
    {
        //defining attributes as public
        public string  bookingID;
        public string  status;
        public Guest bookedGuest;
        public Room bookedRoom;
        public DateTime checkInDate;
        public DateTime checkOutDate;
        public string  specialRequest;
        public string  identityStatus;
        public double roomServiceCharges;
        public double restaurantCharges;
        public double totalCharges;
        public string  paymentStatus;

        public BookingInformation()
        {
            bookingID = "";
            status = "";
            bookedGuest = new Guest();
            bookedRoom = new Room();
            checkInDate = DateTime.Today;
            checkOutDate = DateTime.Today.AddDays(1);
            specialRequest = "";
            roomServiceCharges = 0;
            restaurantCharges = 0;
            totalCharges = 0;
            paymentStatus = "";
        }
        /** Function: readBookingObject
         *      This object read information for dbReader object
         *      passed on to it by a caller function. Expectation is that
         *      the dbReader should contain all required fields for the
         *      BookingInformation object
         */
        public void readBookingObject( OdbcDataReader dbReader)
        {
            try
            {

                this.bookingID = dbReader["bookingId"].ToString().Trim();
                this.status = dbReader["status"].ToString().Trim();

                bookedGuest.guestId = dbReader["guestId"].ToString().Trim();
                bookedGuest.firstName = dbReader["guestFname"].ToString().Trim();
                bookedGuest.lastName = dbReader["guestLname"].ToString().Trim();
                bookedGuest.address = dbReader["address"].ToString().Trim();
                bookedGuest.phone = dbReader["telephone"].ToString().Trim();
                bookedGuest.email = dbReader["email"].ToString().Trim();
                bookedGuest.identityStatus = dbReader["identityStatus"].ToString().Trim();
                bookedGuest.creditCardType = dbReader["creditCardType"].ToString().Trim();
                bookedGuest.creditCardNumber = dbReader["creditCardNumber"].ToString().Trim();

                bookedRoom.roomNumber = dbReader["roomNumber"].ToString().Trim();
                bookedRoom.roomId = dbReader["roomId"].ToString().Trim();
                bookedRoom.roomDescription = dbReader["roomDescription"].ToString().Trim();
                bookedRoom.roomType = dbReader["roomType"].ToString().Trim();
                bookedRoom.roomRate = Convert.ToDouble(dbReader["roomPrice"].ToString());
                bookedRoom.roomFloor = dbReader["roomFloor"].ToString().Trim();

                //objectRoom = Convert.ToDouble(dbReader["totalPrice"].ToString());
                //this.bookingID = dbReader["bookingId"].ToString().Trim();
                this.checkInDate = Convert.ToDateTime(dbReader["checkIn"].ToString().Trim());
                this.checkOutDate = Convert.ToDateTime(dbReader["checkOut"].ToString().Trim());
                this.specialRequest = dbReader["specialRequest"].ToString().Trim();
                this.roomServiceCharges = Convert.ToDouble(dbReader["roomServCharges"].ToString().Trim());
                this.restaurantCharges = Convert.ToDouble(dbReader["restaurantCharges"].ToString().Trim());
               // this.totalCharges = Convert.ToDouble(dbReader["totalPrice"].ToString().Trim());
                this.totalCharges = getTotalCharges();
                this.paymentStatus = dbReader["payStatus"].ToString().Trim();
            }
            catch
            {
                MessageBox.Show("An exception occurred while reading information for this guest",
                    "BookingInformation Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /** Functoin getTotalCharges
         *      It calculate total applicable charges till today for a guest.
         */
        public double getTotalCharges()
        {
            double totalCharges = roomServiceCharges
                                + restaurantCharges;
            if(checkOutDate < DateTime.Today)
                totalCharges += bookedRoom.roomRate * (checkOutDate - checkInDate).TotalDays;
            else
                totalCharges += bookedRoom.roomRate * (DateTime.Today - checkInDate).TotalDays;
            return totalCharges;
        }

    }

}
