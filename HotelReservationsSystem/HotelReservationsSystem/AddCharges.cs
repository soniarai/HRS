using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HotelReservationsSystem
{
    public partial class AddCharges : Form
    {
        BookingInformation objectBookingInformation;
        public AddCharges()
        {
            InitializeComponent();
        }
        public AddCharges(BookingInformation bookingInformation)
            : this()
        {
            objectBookingInformation = bookingInformation;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            roomServiceChargesTextBox.Clear();
            restaurantChargesTextBox.Clear();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            objectBookingInformation.roomServiceCharges += Convert.ToDouble(roomServiceChargesTextBox.Text);
            objectBookingInformation.restaurantCharges += Convert.ToDouble(restaurantChargesTextBox.Text);
            this.Close();
        }
    }
}
