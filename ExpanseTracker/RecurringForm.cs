using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExpanseTracker.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace ExpanseTracker
{
    public partial class RecurringForm : Form
    {
        private List<RecurringExpense> recurringExpenses;
        public string recurringFile = "recurring.json";
        bool isDarkMode = false;
        public RecurringForm(bool darkmode)
        {
            InitializeComponent();
            LoadRecurringExpenses();
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            textBox3.PlaceholderText = "Enter category";
            if (darkmode)
            {
                this.BackColor = Color.FromArgb(30, 30, 30);
                this.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
            }
        }
        public class RecurringExpense
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public string Category { get; set; }
            public string Currency { get; set; }
            public string Frequency { get; set; } // Daily, Weekly, Monthly

            public RecurringExpense(string name, decimal amount, string category, string currency, string frequency)
            {
                Name = name;
                Amount = amount;
                Category = category;
                Currency = currency;
                Frequency = frequency;
            }

            public override string ToString()
            {
                return $"{Name} | {Amount} {Currency} | {Category} | {Frequency}";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string category = textBox3.Text.Trim();
            decimal amount;

            if (!decimal.TryParse(textBox2.Text, out amount))
            {
                errorProvider1.SetError(textBox2, "Please enter a valid amount");
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                errorProvider1.SetError(textBox1, "Please enter a name");
                return;
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                errorProvider1.SetError(textBox3, "Please enter a category");
                return;
            }
            category = char.ToUpper(category[0]) + category.Substring(1).ToLower();

            var existingCategory = recurringExpenses.FirstOrDefault(e =>
                e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (existingCategory != null)
            {
                category = existingCategory.Category;
            }
            string currency = "USD";
            if (comboBox1.SelectedIndex == 1) currency = "UZS";
            else if (comboBox1.SelectedIndex == 2) currency = "EUR";
            string frequency = comboBox2.SelectedItem?.ToString() ?? "Monthly";
            RecurringExpense newRecurring = new RecurringExpense(name, amount, category, currency, frequency);
            recurringExpenses.Add(newRecurring);
            listBox1.Items.Add(newRecurring);
            // Save to file
            SaveRecurringExpenses();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
        private void SaveRecurringExpenses()
        {
            string json = JsonSerializer.Serialize(recurringExpenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(recurringFile, json);
        }
        private void LoadRecurringExpenses()
        {
            if (File.Exists(recurringFile))
            {
                string json = File.ReadAllText(recurringFile);
                recurringExpenses = JsonSerializer.Deserialize<List<RecurringExpense>>(json);
                listBox1.Items.Clear();
                foreach (var r in recurringExpenses)
                {
                    listBox1.Items.Add(r);
                }
            }
            else
            {
                recurringExpenses = new List<RecurringExpense>();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is RecurringExpense selectedExpense)
            {
                string name = textBox1.Text;
                string category = textBox3.Text.Trim();
                decimal amount;
                if (!decimal.TryParse(textBox2.Text, out amount))
                {
                    errorProvider1.SetError(textBox2, "Please enter a valid amount");
                    return;
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    errorProvider1.SetError(textBox1, "Please enter a name");
                    return;
                }
                if (string.IsNullOrWhiteSpace(category))
                {
                    errorProvider1.SetError(textBox3, "Please enter a category");
                    return;
                }
                category = char.ToUpper(category[0]) + category.Substring(1).ToLower();
                var existingCategory = recurringExpenses.FirstOrDefault(e =>
                    e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                if (existingCategory != null)
                {
                    category = existingCategory.Category;
                }
                string currency = "UZS";
                if (comboBox1.SelectedIndex == 1) currency = "USD";
                else if (comboBox1.SelectedIndex == 2) currency = "EUR";
                string frequency = comboBox2.SelectedItem?.ToString() ?? "Monthly";

                // Update the selected expense
                selectedExpense.Name = name;
                selectedExpense.Amount = amount;
                selectedExpense.Category = category;
                selectedExpense.Currency = currency;
                selectedExpense.Frequency = frequency;
                listBox1.Items[listBox1.SelectedIndex] = selectedExpense;
                SaveRecurringExpenses();
                MessageBox.Show("Recurring expense updated!");
            }
            else
            {
                MessageBox.Show("Please select a recurring expense to edit.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is RecurringExpense recurring)
            {
                recurringExpenses.Remove(recurring);
                listBox1.Items.Remove(recurring);
                SaveRecurringExpenses();
                MessageBox.Show("Recurring expense deleted!");
            }
            else
            {
                MessageBox.Show("Please select a expense to delete");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RecurringChartForm chartForm = new RecurringChartForm(isDarkMode);
            chartForm.RecurLoadData(recurringExpenses);
            chartForm.Show();
        }
    }
}