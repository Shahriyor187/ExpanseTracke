using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExpanseTracker
{
    public partial class Form1 : Form
    {
        private List<Expense> expenses = new List<Expense>();
        private string dataFile = "expenses.json";
        bool isDarkMode = false;
        private ErrorProvider errorProvider = new ErrorProvider();
        private System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();
        private List<RecurringForm.RecurringExpense> recurrings = new List<RecurringForm.RecurringExpense>();

        public Form1()
        {
            InitializeComponent();
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            textBox3.PlaceholderText = "Enter category";
            comboBox1.SelectedIndex = 1;
            tooltip.SetToolTip(button1, "Add a new expense");
            tooltip.SetToolTip(button2, "Delete the selected expense");
            tooltip.SetToolTip(button4, "Edit the selected expense");
            tooltip.SetToolTip(button5, "View expenses in a chart");
            tooltip.SetToolTip(button6, "Toggle Dark/Light mode");
            tooltip.SetToolTip(button7, "Export expenses to Excel");
            tooltip.SetToolTip(button8, "Show weekly and monthly totals");
            tooltip.SetToolTip(button9, "Manage recurring expenses");
            tooltip.SetToolTip(numericUpDown1, "Set your monthly expense limit");

            LoadExpenses();
            UpdateTotal();
            LoadRecurringExpensesToList();
        }
        public class Expense
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Category { get; set; }
            public string Currency { get; set; }

            public Expense(string name, decimal amount, string category, string currency)
            {
                Name = name;
                Amount = amount;
                Date = DateTime.Now;
                Category = category;
                Currency = currency;
            }
            public override string ToString()
            {
                string symbol = Currency switch
                {
                    "USD" => "$",
                    "EUR" => "ˆ",
                    "UZS" => "so'm",
                    _ => Currency
                };

                return $"{Date.ToShortDateString()} - {Name} | {Amount} {symbol} | {Category}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string category = textBox3.Text.Trim();
            decimal amount;

            if (!decimal.TryParse(textBox2.Text, out amount))
            {
                errorProvider.SetError(textBox2, "Please enter a valid amount");
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider.SetError(textBox1, "Please enter a name");
                return;
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                errorProvider.SetError(textBox3, "Please enter a category");
                return;
            }
            // normalize category
            category = char.ToUpper(category[0]) + category.Substring(1).ToLower();

            //That compares without distinguishing between uppercase and lowercase letters.
            var existingCategory = expenses.FirstOrDefault(e =>
                e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (existingCategory != null)
            {
                category = existingCategory.Category;
            }
            string currency = "USD";
            if (comboBox1.SelectedIndex == 1) currency = "UZS";
            else if (comboBox1.SelectedIndex == 2) currency = "EUR";

            Expense newExpense = new Expense(name, amount, category, currency);
            expenses.Add(newExpense);
            listBox1.Items.Add(newExpense);

            SaveExpenses();
            UpdateTotal();
            CheckMonthlyLimit();
            LoadRecurringExpensesToList();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
        private void UpdateTotal()
        {
            decimal totalAll = expenses.Sum(exp => exp.Amount); // total of all expenses

            string selectedCurrency = comboBox1.SelectedItem?.ToString() ?? "USD";
            decimal totalCurrency = expenses
                .Where(exp => exp.Currency == selectedCurrency)
                .Sum(exp => exp.Amount);

            string symbol = selectedCurrency switch
            {
                "USD" => "$",
                "EUR" => "ˆ",
                "UZS" => "so'm",
                _ => selectedCurrency
            };

            label1.Text = $"Total ({selectedCurrency}): {totalCurrency} {symbol} | Overall Total: {totalAll}";
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
                CheckMonthlyLimit();
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
            chartform.Show();

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

                    if (ctrl is System.Windows.Forms.Button)
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

                    if (ctrl is System.Windows.Forms.Button)
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
            System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();
            tooltip.Show(message, this, 100, 50, 4000);
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
                ws.Cells[1, 4].Value = $"Amount";
                ws.Cells[1, 5].Value = "Currency";

                int row = 2;
                foreach (var exp in expenses)
                {
                    ws.Cells[row, 1].Value = exp.Date.ToShortDateString();
                    ws.Cells[row, 2].Value = exp.Name;
                    ws.Cells[row, 3].Value = exp.Category;
                    ws.Cells[row, 4].Value = exp.Amount;
                    ws.Cells[row, 5].Value = exp.Currency;
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (!File.Exists(dataFile))
            {
                MessageBox.Show("No expenses found.");
                return;
            }
            DateTime today = DateTime.Now;
            decimal monthly = expenses.Where(exp => exp.Date.Year == today.Year && exp.Date.Month == today.Month)
                .Sum(exp => exp.Amount);

            DateTime startofweek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endofweek = startofweek.AddDays(6);
            decimal weekly = expenses
                .Where(exp => exp.Date >= startofweek && exp.Date <= endofweek)
                .Sum(exp => exp.Amount);
            MessageBox.Show(
                $"Weekly Total: {weekly} {comboBox1.Text}\nMonthly Total: {monthly} {comboBox1.Text}",
                 "Expense Summary",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void CheckMonthlyLimit()
        {
            decimal monthlyTotal = expenses
                    .Where(exp => exp.Date.Year == DateTime.Now.Year && exp.Date.Month == DateTime.Now.Month)
                    .Sum(exp => exp.Amount);
            if (monthlyTotal > numericUpDown1.Value)
            {
                MessageBox.Show(
               $"Warning! Monthly expenses exceeded your limit of {monthlyTotal} {comboBox1.Text}.",
               "Limit Exceeded",
               MessageBoxButtons.OK,
               MessageBoxIcon.Warning);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            RecurringForm form = new RecurringForm(isDarkMode);
            form.Show();
        }
        private void LoadRecurringExpensesToList()
        {
            string json = File.ReadAllText("recurring.json");
            List<RecurringForm.RecurringExpense> recurringList = JsonSerializer.Deserialize<List<RecurringForm.RecurringExpense>>(json);
            foreach(var exp in recurringList)
            {
                listBox1.Items.Add(exp);
            }
        }
    }
}