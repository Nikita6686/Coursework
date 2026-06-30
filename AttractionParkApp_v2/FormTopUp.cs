using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AttractionParkApp
{
    public class FormTopUp : Form
    {
        private readonly TextBox txtAmount;
        public decimal SelectedAmount { get; private set; }

        public FormTopUp()
        {
            Text = "Пополнение баланса";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(320, 170);
            UiTheme.ApplyForm(this);

            var lbl = new Label
            {
                Text = "Сумма пополнения, руб.:",
                Location = new Point(20, 24),
                AutoSize = true
            };
            UiTheme.StyleFieldLabel(lbl);

            txtAmount = new TextBox
            {
                Location = new Point(20, 48),
                Size = new Size(280, 22)
            };
            UiTheme.StyleInput(txtAmount);

            var btnOk = new Button
            {
                Text = "Пополнить",
                Location = new Point(20, 92),
                Size = new Size(135, 38)
            };
            UiTheme.StylePrimaryButton(btnOk);
            btnOk.Click += BtnOk_Click;

            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(165, 92),
                Size = new Size(135, 38),
                DialogResult = DialogResult.Cancel
            };
            UiTheme.StyleSecondaryButton(btnCancel);

            Controls.Add(lbl);
            Controls.Add(txtAmount);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text.Trim().Replace(',', '.'),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var amount) &&
                !decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show("Введите корректную сумму.", "Пополнение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Сумма должна быть больше нуля.", "Пополнение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            SelectedAmount = amount;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
