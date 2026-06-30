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
            UiTheme.ApplyForm(this);
            Text = "Панель управления";
            UiTheme.StyleHeader(lblHeader, "Панель управления");
            lblSubtitle.ForeColor = UiTheme.TextMuted;
            lblSubtitle.Text = "Администратор: " + CurrentUser.FullName;

            LoadDashboardStats();
            StyleModuleButtons();
        }

        private void LoadDashboardStats()
        {
            flowStats.Controls.Clear();
            var stats = DatabaseHelper.GetDashboardStats();

            flowStats.Controls.Add(UiTheme.CreateStatCard("Аттракционы", stats.AttractionCount.ToString(), UiTheme.Primary));
            flowStats.Controls.Add(UiTheme.CreateStatCard("Билетов продано", stats.TicketCount.ToString(), UiTheme.Accent));
            flowStats.Controls.Add(UiTheme.CreateStatCard("Выручка, руб.", stats.TotalRevenue.ToString("N0"), UiTheme.Success));
            flowStats.Controls.Add(UiTheme.CreateStatCard("Открытых заявок", stats.OpenIncidents.ToString(), UiTheme.AccentWarm));
            flowStats.Controls.Add(UiTheme.CreateStatCard("Пользователей", stats.UserCount.ToString(), UiTheme.Header));
        }

        private void StyleModuleButtons()
        {
            UiTheme.StyleMenuButton(MainButtonAtt);
            UiTheme.StyleMenuButton(MainButtonUsers);
            UiTheme.StyleMenuButton(MainButtonTickets);
            UiTheme.StyleMenuButton(MainButtonIncidents);
            UiTheme.StyleSecondaryButton(MainButtonLogout);
        }

        private void OpenModule(Form form)
        {
            using (form)
            {
                form.ShowDialog();
                LoadDashboardStats();
            }
        }

        private void MainButtonAtt_Click(object sender, EventArgs e) => OpenModule(new FormAttractions());
        private void MainButtonUsers_Click(object sender, EventArgs e) => OpenModule(new FormUsers());
        private void MainButtonTickets_Click(object sender, EventArgs e) => OpenModule(new FormTickets());
        private void MainButtonIncidents_Click(object sender, EventArgs e) => OpenModule(new FormIncidents());
        private void MainButtonLogout_Click(object sender, EventArgs e) => Close();
    }
}
