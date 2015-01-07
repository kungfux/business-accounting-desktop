using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for CashPage.xaml
    /// </summary>
    public partial class CashPage : UserControl
    {
        public CashPage()
        {
            InitializeComponent();

            inputDate.DisplayDate = DateTime.Now;
            inputDate.SelectedDate = DateTime.Now;

            LoadHistory();
        }

        private void LoadHistory(bool all = false)
        {
            DataTable historyRecords;

            if (all)
            {
                historyRecords = global.sqlite.SelectTable("select id, datestamp, summa, comment from ba_cash_operations order by id desc;");
                groupHistory.Header = "Все записи";
            }
            else
            {
                historyRecords = global.sqlite.SelectTable("select id, datestamp, summa, comment from ba_cash_operations order by id desc limit 20;");
                groupHistory.Header = "Последние 20 записей";
            }

            List<HistoryRecord> history = new List<HistoryRecord>();

            if (historyRecords != null)
            {
                foreach (DataRow row in historyRecords.Rows)
                {
                    history.Add(new HistoryRecord()
                    {
                        id = Convert.ToInt32(row.ItemArray[0].ToString()),
                        date = Convert.ToDateTime(row.ItemArray[1]).ToString("dd MMMM yyyy"),
                        sum = string.Format("{0:C}", decimal.Parse(row.ItemArray[2].ToString())),
                        comment = row.ItemArray[3].ToString()
                    });
                }
                HistoryTable.ItemsSource = history;
            }
            else
            {
                groupHistory.Header = "Нет последних записей";
                groupHistory.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DateTime date;
            decimal sum;
            string comment;

            if (inputDate.SelectedDate == null)
            {
                ShowMessage("Перед сохранением необходимо выбрать дату!");
                return;
            }
            else
            {
                date = (DateTime)inputDate.SelectedDate;
            }

            if (!decimal.TryParse(inputSum.Text, out sum))
            {
                ShowMessage("Перед сохранением необходимо указать сумму или сумма была введена неверно!");
                return;
            }
            else
            {
                comment = inputComment.Text != "" ? inputComment.Text : null;
            }

            if (global.sqlite.ChangeData("insert into ba_cash_operations (datestamp, summa, comment) values (@d, @s, @c);",
                new SQLiteParameter("@d", inputDate.SelectedDate),
                new SQLiteParameter("@s", sum),
                new SQLiteParameter("@c", comment)) > 0)
            {
                LoadHistory();
                inputSum.Text = "";
                inputComment.Text = "";
            }
            else
            {
                ShowMessage("Не удалось сохранить запись в базе данных!");
                return;
            }
        }

        private void bShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadHistory(true);
        }

        void ShowMessage(string text)
        {
            for (var visual = this as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is MetroWindow)
                {
                    ((MetroWindow)visual).ShowMessageAsync("Проблемка", text + Environment.NewLine + global.sqlite.LastOperationErrorMessage, 
                        MessageDialogStyle.Affirmative);
                }
        }

        private async void bRemoveHistoryRecord_Click(object sender, RoutedEventArgs e)
        {
            HistoryRecord record = null;

            for (var visual = sender as Visual; visual != null; visual = VisualTreeHelper.GetParent(visual) as Visual)
                if (visual is GridViewRowPresenter)
                {
                    var row = (GridViewRowPresenter)visual;
                    record = (HistoryRecord)row.DataContext;
                    break;
                }

            await AskAndDelete(string.Format("Удалить запись?{0}{0}Информация об удаляемой записи:{0} Дата: {1}{0} Сумма: {2}{0} Комментарий: {3}", 
                Environment.NewLine, record.date, record.sum.ToString(), record.comment), record);
        }

        private async Task AskAndDelete(string question, HistoryRecord record)
        {
            MetroWindow w = (MetroWindow)this.Parent.GetParentObject().GetParentObject();
            MessageDialogResult result = await w.ShowMessageAsync("Вопросик", question, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                if (global.sqlite.ChangeData("delete from ba_cash_operations where id = @id;",
                        new SQLiteParameter("@id", record.id)) <= 0)
                {
                    ShowMessage("Не удалось удалить запись из базы данных!");
                }
                else
                {
                    LoadHistory();
                }
            }
        }
    }
}
