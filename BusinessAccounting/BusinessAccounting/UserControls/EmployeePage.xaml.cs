using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusinessAccounting.UserControls
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : UserControl
    {
        public EmployeePage()
        {
            InitializeComponent();
        }

        /*
        private Employee openedEmployee;
        private List<Employee> foundEmployees;

        private void bSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (comboSearchCriteria.SelectedIndex < 0)
            {
                ShowMessage("Перед поиском необходимо выбрать критерий поиска!");
                return;
            }

            if (inputSearchData.Text == "")
            {
                ShowMessage("Перед поиском необходимо ввести данные для поиска!" + Environment.NewLine + 
                    "Например, для поиска по телефону необходимо указать цифры номера, а для поиска по ФИО ввести имя или фамилию.");
                return;
            }

            foundEmployees = new List<Employee>();

            DataTable employees = null;
            switch (((ComboBoxItem)comboSearchCriteria.SelectedItem).Name)
            {
                case "Name":
                    employees = global.sqlite.SelectTable("select id, fullname from ba_employees_cardindex where lower(fullname) like @name;",
                        new SQLiteParameter("@name", "%" + inputSearchData.Text.ToLower() + "%"));
                    break;
                case "Phone":
                    employees = global.sqlite.SelectTable("select id, fullname from ba_employees_cardindex where lower(telephone) like @phone;",
                        new SQLiteParameter("@phone", "%" + inputSearchData.Text.ToLower() + "%"));
                    break;
            }

            if (employees != null && employees.Rows.Count > 0)
            {
                foreach (DataRow r in employees.Rows)
                {
                    foundEmployees.Add(new Employee() { 
                        id = Convert.ToInt32(r.ItemArray[0]),
                        fullname = r.ItemArray[1].ToString()
                    });
                }

                listFoundEmpl.ItemsSource = foundEmployees;
            }
            else
            {
                ShowMessage("Никто не найден!");
            }
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

        private void bSearchDisplayAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataTable employees = null;
            employees = global.sqlite.SelectTable("select id, fullname from ba_employees_cardindex;");

            foundEmployees = new List<Employee>();

            if (employees != null && employees.Rows.Count > 0)
            {
                foreach (DataRow r in employees.Rows)
                {
                    foundEmployees.Add(new Employee() { 
                    id = Convert.ToInt32(r.ItemArray[0]),
                    fullname = r.ItemArray[1].ToString()
                    });
                }
                listFoundEmpl.ItemsSource = foundEmployees;
            }
            else
            {
                ShowMessage("Никто не найден!");
            }
        }

        private void listFoundEmpl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listFoundEmpl.SelectedIndex >= 0)
            {
                openedEmployee = foundEmployees[listFoundEmpl.SelectedIndex];

                inputEmplName.Text = openedEmployee.fullname;
                inputEmplPhone.Text = openedEmployee.telephone;
                inputEmplPassport.Text = openedEmployee.document;
                inputEmplAddress.Text = openedEmployee.address;
                inputEmplNotes.Text = openedEmployee.notes;
            }
        }
         */
    }
}
