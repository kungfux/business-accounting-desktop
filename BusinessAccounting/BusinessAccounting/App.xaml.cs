using BusinessAccounting.Model;
using System;
using System.Threading;
using System.Windows;

namespace BusinessAccounting
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SetLanguageDictionary();
            CheckIfSingleInstanceIsRunning();
            new DatabaseDefaults().InitDatabaseDefaults();
        }

        private void CheckIfSingleInstanceIsRunning()
        {
            const string mutexName = "Business Accounting v2";
            bool isCreatedNew;
            _mutex = new Mutex(true, mutexName, out isCreatedNew);
            if (!isCreatedNew)
            {
                MessageBox.Show(
                    Current.FindResource("MessageAppAlreadyRunning").ToString(),
                    Current.FindResource("MessageDefaultCaption").ToString(),
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                Current.Shutdown();
            }
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "ru-RU":
                    dict.Source = new Uri("Properties\\StringResources.ru-RU.xaml", UriKind.Relative);
                    break;
                case "en-US":
                default:
                    dict.Source = new Uri("Properties\\StringResources.en-US.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
        }
    }
}
