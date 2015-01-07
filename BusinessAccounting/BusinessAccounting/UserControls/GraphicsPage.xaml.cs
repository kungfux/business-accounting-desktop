using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for GraphicsPage.xaml
    /// </summary>
    public partial class GraphicsPage : UserControl
    {
        public GraphicsPage()
        {
            InitializeComponent();
        }

        Chart chart = new Chart();

        private void bBuildGraph_Click(object sender, RoutedEventArgs e)
        {
            if (comboChartType.SelectedItem == null)
            {
                ShowMessage("Нужно выбрать тип графика для построения!");
                return;
            }
            if (comboChartPeriod.SelectedItem == null)
            {
                ShowMessage("Нужно выбрать период для построения графика!");
                return;
            }

            chart = new Chart();

            DateTime startDate = DateTime.Now;
            switch (((ComboBoxItem)comboChartPeriod.SelectedItem).Name)
            {
                case "Week":
                    startDate = startDate.AddDays(-7);
                    break;
                case "Month":
                    startDate = startDate.AddMonths(-1);
                    break;
                case "ThreeMonth":
                    startDate = startDate.AddMonths(-3);
                    break;
                case "SixMonth":
                    startDate = startDate.AddMonths(-6);
                    break;
                case "Year":
                    startDate = startDate.AddYears(-1);
                    break;
                case "ThreeYears":
                    startDate = startDate.AddYears(-3);
                    break;
                case "FiveYears":
                    startDate = startDate.AddYears(-5);
                    break;
                case "AllTime":
                    startDate = DateTime.MinValue;
                    break;
            }

            switch (((ComboBoxItem)comboChartType.SelectedItem).Name)
            {
                case "Incomes":
                    MakeIncomesChart(startDate);
                    break;
                case "Charges":
                    MakeChargesChart(startDate);
                    break;
                case "Compare":
                    MakeCompareChart(startDate);
                    break;
            }

            wfHost.Child = chart;
        }

        void MakeIncomesChart(DateTime startDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График прибыли за период с " + startDate.ToShortDateString() + " по " + DateTime.Now.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Прибыль";

            chart.ChartAreas.Add("Прибыль");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Area;

            DataTable table = global.sqlite.SelectTable("select datetime, sum from ba_cash_operations where sum > 0 and datetime >= @d order by datetime asc;",
                new SQLiteParameter("@d", startDate));

            for (int a = 0; a < table.Rows.Count; a++)
            {
                chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(), Convert.ToDouble(table.Rows[a].ItemArray[1])));
            }
        }

        void MakeChargesChart(DateTime startDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График расходов за период с " + startDate.ToShortDateString() + " по " + DateTime.Now.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Расходы";

            chart.ChartAreas.Add("Расходы");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Area;
            chart.Series[0].Color = System.Drawing.Color.Maroon;

            DataTable table = global.sqlite.SelectTable("select datetime, sum from ba_cash_operations where sum < 0 and datetime >= @d order by datetime asc;",
                new SQLiteParameter("@d", startDate));

            for (int a = 0; a < table.Rows.Count; a++)
            {
                chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(), Convert.ToDouble(table.Rows[a].ItemArray[1].ToString())));
            }
        }

        void MakeCompareChart(DateTime startDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График сравнения за период с " + startDate.ToShortDateString() + " по " + DateTime.Now.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";

            chart.ChartAreas.Add("");

            chart.Series[0].ChartType = SeriesChartType.Doughnut;

            DataRow row = global.sqlite.SelectRow("select * from ((select sum(sum) from ba_cash_operations where datetime >= @d and sum > 0), (select sum(sum) from ba_cash_operations where datetime >= @d and sum < 0));",
                new SQLiteParameter("@d", startDate));

            if (row != null)
            {
                double incomeSum = row.ItemArray[0] != DBNull.Value ? Convert.ToDouble(row.ItemArray[0]) : 0;
                double chargesSum = row.ItemArray[1] != DBNull.Value ? 0 - Convert.ToDouble(row.ItemArray[1]) : 0;

                chart.Series[0].Points.Add(incomeSum);
                chart.Series[0].Points[0].LegendText = "Доходы";
                chart.Series[0].Points[0].Label = incomeSum > 0 ? string.Format("{0:C}", incomeSum) : "";

                chart.Series[0].Points.Add(chargesSum);
                chart.Series[0].Points[1].LegendText = "Расходы";
                chart.Series[0].Points[1].Label = chargesSum > 0 ? string.Format("{0:C}", chargesSum) : "";
            }
        }

        void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка", text +
               Environment.NewLine + global.sqlite.LastOperationErrorMessage, MessageDialogStyle.Affirmative);
                }
        }

        private void bSaveGraph_Click(object sender, RoutedEventArgs e)
        {
            chart.SaveImage("image.png", ChartImageFormat.Png);
        }
    }
}
