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
        private readonly string DefaultRoot = string.Format(@"{0}\frontend\", Application.StartupPath);
        private readonly string DefaultParameters = string.Format("?lang={0}", RegistrySettings.Instance.ReadSetting<string>("Language"));

        private FrontEndBrowser()
        {
            // Dock
            this.Dock = DockStyle.Fill;
            // Navigate options
            //this.AllowNavigation = false;
            this.IsWebBrowserContextMenuEnabled = false;
            this.WebBrowserShortcutsEnabled = false;
            // events
            this.Navigated += new WebBrowserNavigatedEventHandler(FrontEndBrowser_Navigated);
            this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(FrontEndBrowser_DocumentCompleted);
            // load index page
            this.Navigate(DefaultRoot + "index.html" + DefaultParameters);
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
            //TODO: Link front-end browser with HTML menu
        }
    }
}
