using BusinessAccounting.Properties;
using MaterialDesignThemes.Wpf;
using System;
using System.Drawing;

namespace BusinessAccounting.ViewModel
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null)
                throw new ArgumentNullException(nameof(snackbarMessageQueue));

            NavigationDrawerItems = new[]
            {
                new NavigationDrawerItem("Home", null),
                new NavigationDrawerItem("Logbook", null),
                new NavigationDrawerItem("Property", null),
                new NavigationDrawerItem("Contacts", null),
                new NavigationDrawerItem("Documents", null),
                new NavigationDrawerItem("Charts", null),
                new NavigationDrawerItem("Reports", null)
            };

            CompanyLogo = Resources.defaultLogo;
        }

        public NavigationDrawerItem[] NavigationDrawerItems { get; }
        public Image CompanyLogo { get; }
    }
}
