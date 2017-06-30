using System;
using System.Windows.Forms;

namespace Kinderbijslag
{
    public partial class Regels : Form
    {
        public Regels()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Regels_Load(object sender, EventArgs e)
        {
            textBoxRegels.Text = "Een kinderbijslagregeling ziet er als volgt uit:" + "\r\n" + "\r\n" +
                "Voor elk kind jonger dan 12 jaar bedraagt de kinderbijslag € 150,= per kwartaal." + "\r\n" +
                "Voor elk kind vanaf 12 jaar en jonger dan 18 jaar bedraagt de kinderbijslag € 235,= per kwartaal." + "\r\n" + "\r\n" +
                "Afhankelijk van het totale aantal kinderen kan het berekende bedrag nog worden verhoogd met een opslagpercentage:" + "\r\n" + "\r\n" +
                " -- Dit opslagpercentage bedraagt 2 % bij 3 of 4 kinderen." + "\r\n" +
                " -- Het opslagpercentage bedraagt 3 % bij 5 kinderen en" + "\r\n" +
                " -- Het opslagpercentage bedraagt 3.5 % bij 6 of meer kinderen.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
