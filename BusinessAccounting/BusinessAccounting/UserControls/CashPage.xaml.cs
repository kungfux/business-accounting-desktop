using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
                historyRecords = global.sqlite.SelectTable("select id, datetime, sum, comment from ba_cash_operations order by id desc;");
                groupHistory.Header = "Все записи";
            }
            else
            {
                historyRecords = global.sqlite.SelectTable("select id, datetime, sum, comment from ba_cash_operations order by id desc limit 20;");
                groupHistory.Header = "Последние 20 записей";
            }

            List<HistoryRecord> history = new List<HistoryRecord>();

            if (historyRecords != null)
            {
                foreach (DataRow row in historyRecords.Rows)
                {
                    history.Add(new HistoryRecord()
                    {
                        id = int.Parse(row.ItemArray[0].ToString()),
                        date = DateTime.Parse(row.ItemArray[1].ToString()),
                        sum = decimal.Parse(row.ItemArray[2].ToString()),
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

            if (global.sqlite.ChangeData("insert into ba_cash_operations (datetime, sum, comment) values (@d, @s, @c);",
                new SQLiteParameter("@d", inputDate.DisplayDate),
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
            ((MetroWindow)this.Parent.GetParentObject().GetParentObject()).ShowMessageAsync("Проблемка", text +
                Environment.NewLine + global.sqlite.LastOperationErrorMessage, MessageDialogStyle.Affirmative);
        }
    }
}
