using BusinessAccounting.Model;
using BusinessAccounting.Model.Entity;
using System;
using System.Threading;
using System.Windows;

namespace BusinessAccounting
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetLanguageDictionary();
            InitDatabase();
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("Resources\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
                case "ru-RU":
                    dict.Source = new Uri("Resources\\StringResources.ru-RU.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void InitDatabase()
        {
            var context = new DatabaseContext();

            // TODO: Remove next lines
            foreach (var EmployeePosition in context.Set<EmployeePosition>())
            {
                //MessageBox.Show(EmployeePosition.Name);
            }
        }
    }
}