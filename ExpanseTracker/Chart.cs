using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public Chart()
        {
            InitializeComponent();
        }
        public void LoadData(List<Expense> expenses)
        {
            chart1.Series.Clear();

            Series series = new Series("Expenses by Date");
            series.ChartType = SeriesChartType.SplineArea;
            chart1.Series.Add(series);

            var grouped = expenses
                .GroupBy(e => e.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(e => e.Amount) })
                .OrderBy(x => x.Date);

            foreach (var item in grouped)
            {
                series.Points.AddXY(item.Date.ToShortDateString(), item.Total);
            }

            chart1.ChartAreas[0].AxisX.Title = "Date";
            chart1.ChartAreas[0].AxisY.Title = "Total Amount ($)";
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}