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
        public Form1()
        {
            InitializeComponent();
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            LoadExpenses();
            UpdateTotal();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            decimal amount;
            if (!decimal.TryParse(textBox2.Text, out amount))
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            Expense newExpense = new Expense(name, amount);
            expenses.Add(newExpense);
            listBox1.Items.Add(newExpense);

            SaveExpenses();
            UpdateTotal();
            textBox1.Clear();
            textBox2.Clear();
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

        public class Expense
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }

            public Expense(string name, decimal amount)
            {
                Name = name;
                Amount = amount;
                Date = DateTime.Now;
            }
            public override string ToString()
            {
                return $"{Date.ToShortDateString()} - {Name} - {Amount}";
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

    }
}