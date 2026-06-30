using System;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Меню администратора — " + CurrentUser.FullName;
        }

        private void MainButtonAtt_Click(object sender, EventArgs e)
        {
            using (var form = new FormAttractions())
                form.ShowDialog();
        }

        private void MainButtonUsers_Click(object sender, EventArgs e)
        {
            using (var form = new FormUsers())
                form.ShowDialog();
        }

        private void MainButtonTickets_Click(object sender, EventArgs e)
        {
            using (var form = new FormTickets())
                form.ShowDialog();
        }

        private void MainButtonLogout_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
