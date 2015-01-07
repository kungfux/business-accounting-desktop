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
            wfHost.Visibility = Visibility.Hidden;

            if (comboChartType.SelectedItem == null)
            {
                ShowMessage("Нужно выбрать тип графика для построения!");
                return;
            }
            if (pickerPeriodStart.SelectedDate == null || pickerPeriodEnd.SelectedDate == null)
            {
                ShowMessage("Нужно выбрать период для построения графика (укажите обе даты - начальную и конечную)!");
                return;
            }
            if (pickerPeriodEnd.SelectedDate < pickerPeriodStart.SelectedDate)
            {
                ShowMessage("Выбранная конечная дата меньше начальной. Это не логично!");
                return;
            }

            chart = new Chart();

            DateTime startDate = (DateTime)pickerPeriodStart.SelectedDate;
            DateTime endDate = (DateTime)pickerPeriodEnd.SelectedDate;

            bool chartReady = false;
            switch (((ComboBoxItem)comboChartType.SelectedItem).Name)
            {
                case "Incomes":
                    chartReady = MakeIncomesChart(startDate, endDate);
                    break;
                case "Charges":
                    chartReady = MakeChargesChart(startDate, endDate);
                    break;
                case "IncomesCharges":
                    chartReady = MakeIncomesChargesChart(startDate, endDate);
                    break;
                case "Compare":
                    chartReady = MakeCompareChart(startDate, endDate);
                    break;
            }

            if (chartReady)
            {
                wfHost.Child = chart;
                wfHost.Visibility = Visibility.Visible;
            }
        }

        bool MakeIncomesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График прибыли за период с " + startDate.ToShortDateString() + " по " + endDate.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Прибыль";

            chart.ChartAreas.Add("Прибыль");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Area;

            DataTable table =
                global.sqlite.SelectTable("select datestamp, summa from ba_cash_operations where summa > 0 and datestamp >= @d1 and datestamp <= @d2 order by datestamp asc;",
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(), 
                        Convert.ToDouble(table.Rows[a].ItemArray[1])));
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        bool MakeChargesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График расходов за период с " + startDate.ToShortDateString() + " по " + endDate.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Расходы";

            chart.ChartAreas.Add("Расходы");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Area;
            chart.Series[0].Color = System.Drawing.Color.Maroon;

            DataTable table =
                global.sqlite.SelectTable("select datestamp, summa from ba_cash_operations where summa < 0 and datestamp >= @d1 and datestamp <= @d2 order by datestamp asc;",
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(), 
                        0 - Convert.ToDouble(table.Rows[a].ItemArray[1].ToString())));
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        bool MakeCompareChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График сравнения за период с " + startDate.ToShortDateString() + " по " + endDate.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";

            chart.ChartAreas.Add("");

            chart.Series[0].ChartType = SeriesChartType.Doughnut;

            DataRow row =
                global.sqlite.SelectRow("select * from ((select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0), " +
                "(select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0));",
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

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
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        bool MakeIncomesChargesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Series.Add("");
            chart.Titles.Add("График прибыли и расходов за период с " + startDate.ToShortDateString() + " по " + endDate.ToShortDateString());
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Прибыль";
            chart.Series[1].LegendText = "Расходы";

            chart.ChartAreas.Add("Прибыль");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;
            chart.Series[1].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Line;
            chart.Series[1].ChartType = SeriesChartType.Line;

            chart.Series[1].Color = System.Drawing.Color.Maroon;

            DataTable tableIncomes =
                global.sqlite.SelectTable("select datestamp, summa from ba_cash_operations where summa > 0 and datestamp >= @d1 and datestamp <= @d2 order by datestamp asc;",
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            DataTable tableCharges =
                global.sqlite.SelectTable("select datestamp, summa from ba_cash_operations where summa < 0 and datestamp >= @d1 and datestamp <= @d2 order by datestamp asc;",
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if ((tableIncomes == null || tableIncomes.Rows.Count == 0) && (tableCharges == null || tableCharges.Rows.Count == 0))
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            else
            {
                if (tableIncomes != null)
                {
                    for (int a = 0; a < tableIncomes.Rows.Count; a++)
                    {
                        chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(tableIncomes.Rows[a].ItemArray[0]).ToOADate(), 
                            Convert.ToDouble(tableIncomes.Rows[a].ItemArray[1])));
                    }
                }

                if (tableCharges != null)
                {
                    for (int a = 0; a < tableCharges.Rows.Count; a++)
                    {
                        chart.Series[1].Points.Add(new DataPoint(Convert.ToDateTime(tableCharges.Rows[a].ItemArray[0]).ToOADate(), 
                            0 - Convert.ToDouble(tableCharges.Rows[a].ItemArray[1].ToString())));
                    }
                }
            }
            return true;
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
