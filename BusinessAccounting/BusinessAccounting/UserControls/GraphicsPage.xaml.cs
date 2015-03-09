using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
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

        public static RoutedCommand PrintChartCommand = new RoutedCommand();
        public static RoutedCommand SaveChartCommand = new RoutedCommand();

        private const string sqlIncomes = "select datestamp, sum(summa) from ba_cash_operations where summa > 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        private const string sqlCharges = "select datestamp, sum(summa) from ba_cash_operations where summa < 0 and datestamp >= @d1 and datestamp <= @d2 group by datestamp order by datestamp asc;";
        private const string sqlCompare = "select * from ((select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa > 0), " +
                                          "(select sum(summa) from ba_cash_operations where datestamp >= @d1 and datestamp <= @d2 and summa < 0));";

        Chart chart;

        #region Functionality methods
        private void PrintChart()
        {
            wfHost.Visibility = Visibility.Hidden;

            chart = new Chart();

            DateTime startDate = (DateTime)pickerPeriodStart.SelectedDate;
            DateTime endDate = (DateTime)pickerPeriodEnd.SelectedDate;

            bool chartReady = false;
            switch (((ComboBoxItem)comboChartType.SelectedItem).Name)
            {
                case "Incomes":
                    chartReady = BuildIncomesChart(startDate, endDate);
                    break;
                case "Charges":
                    chartReady = BuildChargesChart(startDate, endDate);
                    break;
                case "IncomesCharges":
                    chartReady = BuildIncomesChargesChart(startDate, endDate);
                    break;
                case "Compare":
                    chartReady = BuildCompareChart(startDate, endDate);
                    break;
            }

            if (chartReady)
            {
                wfHost.Child = chart;
                wfHost.Visibility = Visibility.Visible;
            }
        }

        private bool BuildIncomesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График прибыли за период " + Environment.NewLine + "с " + startDate.ToString("dd MMMM yyyy") + " по " + endDate.ToString("dd MMMM yyyy"));
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";
            chart.Series[0].LegendText = "Прибыль";

            chart.ChartAreas.Add("Прибыль");
            chart.ChartAreas[0].AxisX.Title = "Дата";
            chart.ChartAreas[0].AxisY.Title = "Сумма";

            chart.Series[0].XValueType = ChartValueType.Date;

            chart.Series[0].ChartType = SeriesChartType.Area;

            DataTable table =
                App.sqlite.SelectTable(sqlIncomes,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(),
                        Convert.ToDouble(table.Rows[a].ItemArray[1])));
                    if (comboDisplayValues.IsChecked == true)
                    {
                        chart.Series[0].Points[a].Label = string.Format("{0:C}", Convert.ToDouble(table.Rows[a].ItemArray[1]));
                    }
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private bool BuildChargesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График расходов за период " + Environment.NewLine + "с " + startDate.ToString("dd MMMM yyyy") + " по " + endDate.ToString("dd MMMM yyyy"));
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
                App.sqlite.SelectTable(sqlCharges,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (table != null && table.Rows.Count > 0)
            {
                for (int a = 0; a < table.Rows.Count; a++)
                {
                    chart.Series[0].Points.Add(new DataPoint(Convert.ToDateTime(table.Rows[a].ItemArray[0]).ToOADate(),
                        0 - Convert.ToDouble(table.Rows[a].ItemArray[1])));
                    if (comboDisplayValues.IsChecked == true)
                    {
                        chart.Series[0].Points[a].Label = string.Format("{0:C}", Convert.ToDouble(table.Rows[a].ItemArray[1]));
                    }
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private bool BuildCompareChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Titles.Add("График сравнения за период " + Environment.NewLine + "с " + startDate.ToString("dd MMMM yyyy") + " по " + endDate.ToString("dd MMMM yyyy"));
            chart.Legends.Add("");
            chart.Legends[0].Title = "Легенда";

            chart.ChartAreas.Add("");

            chart.Series[0].ChartType = SeriesChartType.Doughnut;

            DataRow row =
                App.sqlite.SelectRow(sqlCompare,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            if (row != null)
            {
                double incomeSum = row.ItemArray[0] != DBNull.Value ? Convert.ToDouble(row.ItemArray[0]) : 0;
                double chargesSum = row.ItemArray[1] != DBNull.Value ? 0 - Convert.ToDouble(row.ItemArray[1]) : 0;

                chart.Series[0].Points.Add(incomeSum);
                chart.Series[0].Points[0].LegendText = "Доходы";
                if (comboDisplayValues.IsChecked == true)
                {
                    chart.Series[0].Points[0].Label = incomeSum > 0 ? string.Format("{0:C}", incomeSum) : "";
                }

                chart.Series[0].Points.Add(chargesSum);
                chart.Series[0].Points[1].LegendText = "Расходы";
                if (comboDisplayValues.IsChecked == true)
                {
                    chart.Series[0].Points[1].Label = chargesSum > 0 ? string.Format("{0:C}", chargesSum) : "";
                }
            }
            else
            {
                ShowMessage("Нет данных для построения графика!");
                return false;
            }
            return true;
        }

        private bool BuildIncomesChargesChart(DateTime startDate, DateTime endDate)
        {
            chart.Series.Add("");
            chart.Series.Add("");
            chart.Titles.Add("График прибыли и расходов за период " + Environment.NewLine + "с " + startDate.ToString("dd MMMM yyyy") + " по " + endDate.ToString("dd MMMM yyyy"));
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
                App.sqlite.SelectTable(sqlIncomes,
                new SQLiteParameter("@d1", startDate),
                new SQLiteParameter("@d2", endDate));

            DataTable tableCharges =
                App.sqlite.SelectTable(sqlCharges,
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
                        if (comboDisplayValues.IsChecked == true)
                        {
                            chart.Series[0].Points[a].Label = string.Format("{0:C}", Convert.ToDouble(tableIncomes.Rows[a].ItemArray[1]));
                        }
                    }
                }

                if (tableCharges != null)
                {
                    for (int a = 0; a < tableCharges.Rows.Count; a++)
                    {
                        chart.Series[1].Points.Add(new DataPoint(Convert.ToDateTime(tableCharges.Rows[a].ItemArray[0]).ToOADate(),
                            0 - Convert.ToDouble(tableCharges.Rows[a].ItemArray[1])));
                        if (comboDisplayValues.IsChecked == true)
                        {
                            chart.Series[1].Points[a].Label = string.Format("{0:C}", Convert.ToDouble(tableCharges.Rows[a].ItemArray[1]));
                        }
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
               Environment.NewLine + App.sqlite.LastOperationErrorMessage, MessageDialogStyle.Affirmative);
                }
        }

        private void SaveChart()
        {
            System.Windows.Forms.SaveFileDialog sfDialog = new System.Windows.Forms.SaveFileDialog();
            sfDialog.AddExtension = true;
            sfDialog.Filter = "Png (PNG)|*.png";
            if (sfDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (sfDialog.FileName != "")
                {
                    chart.SaveImage(sfDialog.FileName, ChartImageFormat.Png);
                }
            }
        }
        #endregion

        #region Commands
        private void PrintChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                comboChartType.SelectedIndex != -1 & // type of chart is selected
                pickerPeriodStart.SelectedDate != null & // start date is selected
                pickerPeriodEnd.SelectedDate != null & // end date is selected
                pickerPeriodStart.SelectedDate <= pickerPeriodEnd.SelectedDate; // start date is earlier then end date
        }

        private void PrintChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintChart();
        }

        private void SaveChart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =
                wfHost != null &&
                wfHost.Visibility == Visibility.Visible && // if WindowsFormsHost visible than chart is built
                chart != null;
        }

        private void SaveChart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChart();
        }
        #endregion
    }
}
