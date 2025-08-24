using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExpanseTracker.Form1;

namespace ExpanseTracker
{
    public partial class Form2 : Form
    {
        public Expense EditedExpense { get; private set; }
        public Form2(Expense expense)
        {
            InitializeComponent();
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            textBox1.Text = expense.Name;
            textBox2.Text = expense.Amount.ToString();
            dateTimePicker1.Value = expense.Date;

            EditedExpense = expense;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            EditedExpense.Name = textBox1.Text;
            EditedExpense.Amount = decimal.Parse(textBox2.Text);
            EditedExpense.Date = dateTimePicker1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}