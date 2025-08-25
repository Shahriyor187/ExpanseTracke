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
using static ExpanseTracker.Form1;

namespace ExpanseTracker
{
    public partial class Chart : Form
    {
        public Chart(bool darkmode)
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
        public void LoadData(List<Expense> expenses)
        {

            chart1.Series.Clear();

            Series series = new Series("Expenses by Category");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            series.IsValueShownAsLabel = true;
            series.Label = "#VALY";   // only show the amount 
            series["PieLabelStyle"] = "Inside";  // keep labels inside slices
            chart1.Legends[0].Enabled = true;    // show category names in legend
            chart1.Series.Add(series);

            var grouped = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderBy(x => x.Category);

            foreach (var item in grouped)
            {
                int pointIndex = series.Points.AddXY(item.Category, item.Total);
                series.Points[pointIndex].LegendText = item.Category; // show category in legend
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}