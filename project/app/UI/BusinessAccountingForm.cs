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

using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using SkinFramework;
using BusinessAccounting.Common;

namespace BusinessAccounting.UI
{
    /// <summary>
    /// App main form
    /// </summary>
    internal class BusinessAccountingForm : Form
    {
        // Attach skin manager
        private readonly SkinningManager skinManager = new SkinningManager();
        private readonly LanguageSupport language = new LanguageSupport();
        private StatusStrip statusStrip;

        public BusinessAccountingForm()
        {
            // set default form settings
            this.Text = Messaging.Instance.appProductName;
            this.Icon = Resources.favicon;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = RegistrySettings.Instance.ReadSetting<Size>("FormSize");
            this.WindowState = (FormWindowState)RegistrySettings.Instance.ReadSetting<int>("FormWindowState");
            this.Font = SystemFonts.GetFontByName(RegistrySettings.Instance.ReadSetting<string>("GlobalFontName"));
            
            // status strip
            statusStrip = new StatusStrip();
            statusStrip.Visible = false;

            ToolStripStatusLabel labelLoading = new ToolStripStatusLabel(language.GetStringByID("LoadingStatus"));
            ToolStripStatusLabel labelSpring = new ToolStripStatusLabel("");
            labelSpring.Spring = true;

            ToolStripProgressBar progressBar = new ToolStripProgressBar();
            progressBar.Width = 100;
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 50;
            progressBar.Value = progressBar.Maximum;
            
            statusStrip.Items.Add(labelLoading);
            statusStrip.Items.Add(labelSpring);
            statusStrip.Items.Add(progressBar);
            this.Controls.Add(statusStrip);
            
            // events
            this.FormClosing += new FormClosingEventHandler(BusinessAccountingForm_FormClosing);

            // add front-end browser
            FrontEndBrowser.Instance.Navigating += new WebBrowserNavigatingEventHandler(frontEndBrowser_Navigating);
            FrontEndBrowser.Instance.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(frontEndBrowser_DocumentCompleted);
            this.Controls.Add(FrontEndBrowser.Instance);

            // apply skin
            skinManager.ParentForm = this;
            skinManager.DefaultSkin = (DefaultSkin)RegistrySettings.Instance.ReadSetting<int>("Skin");
        }

        private void DisplayProgress(bool pInProgress)
        {
            statusStrip.Visible = pInProgress;
        }

        void frontEndBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            DisplayProgress(false);
        }

        void frontEndBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            DisplayProgress(true);
        }

        void BusinessAccountingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save form state and size
            if (this.WindowState != FormWindowState.Maximized)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Size));
                RegistrySettings.Instance.SaveSetting("FormSize", typeConverter.ConvertToString(this.Size));
            }
            RegistrySettings.Instance.SaveSetting("FormWindowState", (int)this.WindowState);
        }
    }
}
