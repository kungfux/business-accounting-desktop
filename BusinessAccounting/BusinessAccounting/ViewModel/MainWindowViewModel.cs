using MaterialDesignThemes.Wpf;
using System;

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
                new NavigationDrawerItem("Home", null)
            };
        }

        public NavigationDrawerItem[] NavigationDrawerItems { get; }
    }
}
