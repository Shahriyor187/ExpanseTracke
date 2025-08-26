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
using System.Windows.Forms.DataVisualization.Charting;

namespace ExpanseTracker
{
    public partial class RecurringChartForm : Form
    {
        public RecurringChartForm(bool darkmode)
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
        }
        public void RecurLoadData(List<RecurringForm.RecurringExpense> recurexpenses)
        {
            chart1.Series.Clear();

            Series series = new Series("Recurring Expenses by Category")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Label = "#VALY"
            };
            series["PieLabelStyle"] = "Inside";

            if (chart1.Legends.Count == 0)
                chart1.Legends.Add(new Legend());

            chart1.Series.Add(series);

            var grouped = recurexpenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderBy(x => x.Category);

            foreach (var item in grouped)
            {
                int pointIndex = series.Points.AddXY(item.Category, item.Total);
                series.Points[pointIndex].LegendText = item.Category;
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}