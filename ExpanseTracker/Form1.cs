using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ExpanseTracker
{
    public partial class Form1 : Form
    {
        private List<Expense> expenses = new List<Expense>();
        private string dataFile = "expenses.json";

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

            public Expense(string name, decimal amount)
            {
                Name = name;
                Amount = amount;
            }
            public override string ToString()
            {
                return $"{Name} - {Amount}";
            }
        }
    }
}