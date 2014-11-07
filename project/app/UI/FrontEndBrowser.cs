/* Business Accounting 
 * Small Business Accounting Solution
 * Copyright (C) 2014 Fuks Alexander
 * <mailto:kungfux2010@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Windows.Forms;
using BusinessAccounting.Common;
using BusinessAccounting.UI.FrontEndPages;

namespace BusinessAccounting.UI
{
    /// <summary>
    /// Based on Forms.WebBrowser front-end browser
    /// </summary>
    internal class FrontEndBrowser : WebBrowser
    {
        // singleton
        private static readonly FrontEndBrowser instance = new FrontEndBrowser();

        public static FrontEndBrowser Instance
        {
            get
            {
                return instance;
            }
        }

        private LanguageSupport language = new LanguageSupport();
        private readonly string defaultRoot = string.Format(@"{0}\frontend\", Application.StartupPath);
        private readonly string defaultParameters = 
            string.Format("?lang={0}&theme={1}&reverse={2}", 
            RegistrySettings.Instance.ReadSetting<string>("Language"),
            RegistrySettings.Instance.ReadSetting<string>("FrontEndTheme"),
            RegistrySettings.Instance.ReadSetting<bool>("FrontEndReverseBackground"));
        private readonly FrontEndPageSkeleton[] pages = 
            new FrontEndPageSkeleton[] {
            new CashPage(),
            new EmployeesPage(),
            new ChartsPage(),
            new ReportsPage(),
            new SetupPage(),
            new HelpPage()
            };

        private FrontEndBrowser()
        {
            // Dock
            this.Dock = DockStyle.Fill;
            // Navigate options
            //this.AllowNavigation = false;
            this.IsWebBrowserContextMenuEnabled = false;
            this.WebBrowserShortcutsEnabled = RegistrySettings.Instance.ReadSetting<bool>("FrontEndBrowserShortcutsEnabled");
            // events
            this.Navigated += new WebBrowserNavigatedEventHandler(FrontEndBrowser_Navigated);
            this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(FrontEndBrowser_DocumentCompleted);
            // load index page
            this.Navigate(defaultRoot + "index.html" + defaultParameters);
        }

        void FrontEndBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // if requested page is not found
            if (this.Document.Url.ToString().StartsWith("res:"))
            {
                Messaging.Instance.ShowMessage(
                    language.GetStringByID("WarningPageNotFound"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                this.GoBack();
            }
        }

        void FrontEndBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // link front-end browser with HTML menu
            if (this.Document != null)
            {
                for (int a = 0; a < pages.Length; a++)
                {
                    // add click event to menu item
                    this.Document.GetElementById(pages[a].PageID).Click += new HtmlElementEventHandler(HtmlMenu_Click);
                }
            }
            // call Init() page for active page
            for (int a = 0; a < pages.Length; a++)
            {
                if (this.Document.Url.ToString().EndsWith(pages[a].PageHtmlSource))
                {
                    pages[a].Init();
                }
            }
        }

        void HtmlMenu_Click(object sender, HtmlElementEventArgs e)
        {
            // open another source if menu item is clicked
            if (sender is HtmlElement)
            {
                HtmlElement element = sender as HtmlElement;
                for (int a = 0; a < pages.Length; a++)
                {
                    if (element.Id == pages[a].PageID)
                    {
                        this.Navigate(defaultRoot + pages[a].PageHtmlSource + defaultParameters);
                        break;
                    }
                }
            }           
        }
    }
}
