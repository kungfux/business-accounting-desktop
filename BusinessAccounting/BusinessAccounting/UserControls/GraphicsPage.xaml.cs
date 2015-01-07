using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

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
            chart = new Chart();

            chart.Series.Add("asd");

            chart.Titles.Add("Заголовок");
            chart.Legends.Add("");
            chart.Series[0].LegendText = "asd";

            int[] x = { 1, 5, 8 };
            int[] y = { 2, 9, 3 };

            

            for (int a = 0; a < x.Length; a++)
            {
                chart.Series[0].Points.Add(new DataPoint(x[a], y[a]));
            }

            chart.ChartAreas.Add("Прибыль");
            chart.Series[0].ChartType = SeriesChartType.SplineArea;

            wfHost.Child = chart;
        }
    }
}
