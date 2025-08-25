using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ExpanseTracker
{
    public partial class Form1 : Form
    {
        private List<Expense> expenses = new List<Expense>();
        private string dataFile = "expenses.json";
        bool isDarkMode = false;
        private ErrorProvider errorProvider = new ErrorProvider();

        public Form1()
        {
            InitializeComponent();
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            textBox3.PlaceholderText = "Enter category";
            LoadExpenses();
            UpdateTotal();
        }
        public class Expense
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Category { get; set; }

            public Expense(string name, decimal amount, string category)
            {
                Name = name;
                Amount = amount;
                Date = DateTime.Now;
                Category = category;
            }
            public override string ToString()
            {
                return $"{Date.ToShortDateString()} - {Name} - {Category} - {Amount}";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string category = textBox3.Text;
            decimal amount;
            if (!decimal.TryParse(textBox2.Text, out amount))
            {
                errorProvider.SetError(textBox2, "Please enter a valid amount");
                return;
            }
            bool hasLetter = false;
            foreach (char c in name)
            {
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                    break;
                }
            }
            if (!hasLetter)
            {
                errorProvider.SetError(textBox1, "Please enter a name");
                return;
            }

            Expense newExpense = new Expense(name, amount, category);
            expenses.Add(newExpense);
            listBox1.Items.Add(newExpense);

            SaveExpenses();
            UpdateTotal();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (var exp in expenses)
            {
                total += exp.Amount;
            }
            label1.Text = $"Total: {total}";
        }

        private void SaveExpenses()
        {
            try
            {
                string json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message);
            }
        }

        private void LoadExpenses()
        {
            if (File.Exists(dataFile))
            {
                try
                {
                    string json = File.ReadAllText(dataFile);
                    expenses = JsonSerializer.Deserialize<List<Expense>>(json);

                    listBox1.Items.Clear();
                    foreach (var exp in expenses)
                    {
                        listBox1.Items.Add(exp);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime from = dateTimePickerfrom.Value.Date;
            DateTime to = dateTimePickerto.Value.Date;
            decimal total = 0;
            listBox1.Items.Clear();

            foreach (var a in expenses)
            {
                if (a.Date >= from && a.Date <= to)
                {
                    listBox1.Items.Add(a);
                    total += a.Amount;
                }
            }
            label1.Text = $"Total (Filtered): {total}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Expense selectedExpense)
            {
                expenses.Remove(selectedExpense);
                listBox1.Items.Remove(selectedExpense);
                SaveExpenses();
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Please select an expense to delete.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is Expense selectedExpense)
            {
                using (Form2 form2 = new Form2(selectedExpense, isDarkMode, this))
                {
                    if (form2.ShowDialog() == DialogResult.OK)
                    {
                        listBox1.Items[listBox1.SelectedIndex] = form2.EditedExpense;
                        UpdateTotal();
                        SaveExpenses();
                        ShowTooltip("Expense updated successfully!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to edit.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Chart chartform = new Chart(isDarkMode);
            chartform.LoadData(expenses);
            chartform.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;

            foreach (Form frm in Application.OpenForms)
            {
                frm.BackColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
                frm.ForeColor = isDarkMode ? Color.White : Color.Black;

                ApplyTheme(frm, isDarkMode);
            }
        }
        private void ApplyTheme(Control parent, bool darkMode)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (darkMode)
                {
                    ctrl.ForeColor = Color.White;

                    if (ctrl is Button)
                        ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    else if (ctrl is ListBox)
                        ctrl.BackColor = Color.FromArgb(40, 40, 40);
                    else
                        ctrl.BackColor = Color.FromArgb(30, 30, 30);
                    button6.Text = "Switch to Light";
                }
                else
                {
                    ctrl.ForeColor = Color.Black;

                    if (ctrl is Button)
                        ctrl.BackColor = SystemColors.Control;
                    else if (ctrl is ListBox)
                        ctrl.BackColor = Color.White;
                    else
                        ctrl.BackColor = Color.White;
                    button6.Text = "Switch to Dark";
                }

                if (ctrl.HasChildren)
                    ApplyTheme(ctrl, darkMode);
            }
        }
        public void ShowTooltip(string message)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.Show(message, this, 100, 50, 2000); // 2 seconds
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!File.Exists(dataFile))
            {
                MessageBox.Show("No expenses found to export.");
                return;
            }

            string json = File.ReadAllText(dataFile);
            List<Expense> expenses = JsonSerializer.Deserialize<List<Expense>>(json);

            if (expenses == null || expenses.Count == 0)
            {
                MessageBox.Show("No expenses found to export.");
                return;
            }

            ExcelPackage.License.SetNonCommercialPersonal("Myspentmoney");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Expenses");
                ws.Cells[1, 1].Value = "Date";
                ws.Cells[1, 2].Value = "Name";
                ws.Cells[1, 3].Value = "Category";
                ws.Cells[1, 4].Value = "Amount";

                int row = 2;
                foreach (var exp in expenses)
                {
                    ws.Cells[row, 1].Value = exp.Date.ToShortDateString();
                    ws.Cells[row, 2].Value = exp.Name;
                    ws.Cells[row, 3].Value = exp.Category;
                    ws.Cells[row, 4].Value = exp.Amount;
                    row++;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Workbook|*.xlsx";
                    sfd.FileName = "Expenses.xlsx";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo file = new FileInfo(sfd.FileName);
                        package.SaveAs(file);
                        MessageBox.Show("Excel exported successfully!");
                    }
                }
            }
        }
    }
}