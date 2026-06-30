using System;

using System.Data;

using System.Windows.Forms;



namespace AttractionParkApp

{

    public partial class FormVisitor : Form

    {

        private Label lblBalance;

        private Panel panelBalance;



        public FormVisitor()

        {

            InitializeComponent();

        }



        private void FormVisitor_Load(object sender, EventArgs e)

        {

            UiTheme.ApplyForm(this);

            Text = "Аттракционы парка";

            UiTheme.StyleHeader(lblWelcome, CurrentUser.FullName);



            lblBalance = new Label();

            panelBalance = UiTheme.CreateBalanceCard(lblBalance);

            Controls.Add(panelBalance);

            panelBalance.BringToFront();



            UiTheme.StyleActionButtons(btnBuyTicket, btnTopUp, btnMyTickets, btnRefresh, btnLogout);

            UiTheme.LayoutContentWithFooter(this, dataGridView1, btnBuyTicket, btnTopUp, btnMyTickets, btnRefresh, btnLogout);



            int gridTop = UiTheme.HeaderHeight + panelBalance.Height + UiTheme.Pad;

            UiTheme.LayoutGrid(this, dataGridView1, gridTop, UiTheme.FooterHeight + UiTheme.Pad);



            RefreshBalance();

            LoadAttractions();

        }



        private void RefreshBalance()

        {

            CurrentUser.RefreshBalance();

            UiTheme.SetBalanceText(lblBalance, CurrentUser.Balance);

        }



        private void LoadAttractions()

        {

            dataGridView1.DataSource = DatabaseHelper.GetAttractionsForVisitors();



            if (dataGridView1.Columns.Contains("ID_Attraction"))

                dataGridView1.Columns["ID_Attraction"].Visible = false;



            SetColumnHeader("Name", "Название");

            SetColumnHeader("Zone", "Зона");

            SetColumnHeader("CurrentStatus", "Статус");

            SetColumnHeader("TicketPrice", "Цена, руб.");



            GridHelper.FitColumns(dataGridView1);

        }



        private void SetColumnHeader(string columnName, string header)

        {

            if (dataGridView1.Columns.Contains(columnName))

                dataGridView1.Columns[columnName].HeaderText = header;

        }



        private void btnBuyTicket_Click(object sender, EventArgs e)

        {

            if (dataGridView1.CurrentRow == null)

            {

                MessageBox.Show("Выберите аттракцион.", "Покупка билета",

                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            var row = ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row;

            int attractionId = Convert.ToInt32(row["ID_Attraction"]);

            string name = row["Name"].ToString();

            decimal price = row["TicketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TicketPrice"]);



            if (CurrentUser.Balance < price)

            {

                MessageBox.Show(

                    "Недостаточно средств.\nБаланс: " + CurrentUser.Balance.ToString("N0") +

                    " руб., цена билета: " + price.ToString("N0") + " руб.\n\nПополните баланс.",

                    "Покупка билета", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            var confirm = MessageBox.Show(

                "Купить билет на \"" + name + "\" за " + price.ToString("N0") + " руб.?",

                "Покупка билета",

                MessageBoxButtons.YesNo,

                MessageBoxIcon.Question);



            if (confirm != DialogResult.Yes)

                return;



            try

            {

                DatabaseHelper.BuyTicket(CurrentUser.Id, attractionId);

                RefreshBalance();

                MessageBox.Show(

                    "Билет куплен!\nАттракцион: " + name +

                    "\nСписано: " + price.ToString("N0") + " руб." +

                    "\nОстаток: " + CurrentUser.Balance.ToString("N0") + " руб.",

                    "Покупка билета",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Information);

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message, "Покупка билета",

                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }



        private void btnTopUp_Click(object sender, EventArgs e)

        {

            using (var form = new FormTopUp())

            {

                if (form.ShowDialog(this) != DialogResult.OK)

                    return;



                try

                {

                    DatabaseHelper.TopUpBalance(CurrentUser.Id, form.SelectedAmount);

                    RefreshBalance();

                    MessageBox.Show(

                        "Баланс пополнен на " + form.SelectedAmount.ToString("N0") + " руб.\n" +

                        "Текущий баланс: " + CurrentUser.Balance.ToString("N0") + " руб.",

                        "Пополнение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)

                {

                    MessageBox.Show(ex.Message, "Пополнение", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }

        }



        private void btnMyTickets_Click(object sender, EventArgs e)

        {

            using (var form = new FormMyTickets())

                form.ShowDialog(this);

        }



        private void btnRefresh_Click(object sender, EventArgs e)

        {

            RefreshBalance();

            LoadAttractions();

        }



        private void btnLogout_Click(object sender, EventArgs e) => Close();

    }

}


