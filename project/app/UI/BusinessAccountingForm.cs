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

        public BusinessAccountingForm()
        {
            // set default form settings
            this.Text = Messaging.Instance.appProductName;
            this.Icon = Resources.favicon;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = RegistrySettings.Instance.ReadSetting<Size>("FormSize");
            this.WindowState = (FormWindowState)RegistrySettings.Instance.ReadSetting<int>("FormWindowState");
            this.Font = SystemFonts.GetFontByName(RegistrySettings.Instance.ReadSetting<string>("GlobalFontName"));
            // events
            this.FormClosing += new FormClosingEventHandler(BusinessAccountingForm_FormClosing);

            // apply skin
            skinManager.ParentForm = this;
            skinManager.DefaultSkin = (DefaultSkin)RegistrySettings.Instance.ReadSetting<int>("Skin");
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
