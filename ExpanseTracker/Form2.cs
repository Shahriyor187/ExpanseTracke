using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ExpanseTracker.Form1;

namespace ExpanseTracker
{
    public partial class Form2 : Form
    {
        private Form1 mainform;
        public Expense EditedExpense { get; private set; }
        public Form2(Expense expense, bool darkmode, Form1 form1)
        {
            InitializeComponent();
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
            button1.UseVisualStyleBackColor = false;
            mainform = form1;
            textBox1.PlaceholderText = "Enter name";
            textBox2.PlaceholderText = "Enter amount";
            textBox3.PlaceholderText = "Enter category";
            textBox1.Text = expense.Name;
            textBox2.Text = expense.Amount.ToString();
            textBox3.Text = expense.Category;
            dateTimePicker1.Value = expense.Date;

            EditedExpense = expense;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditedExpense.Name = textBox1.Text;
            EditedExpense.Amount = decimal.Parse(textBox2.Text);
            EditedExpense.Date = dateTimePicker1.Value;
            this.DialogResult = DialogResult.OK;
            mainform.ShowTooltip("Expense updated successfully!");
            this.Close();
        }
    }
}