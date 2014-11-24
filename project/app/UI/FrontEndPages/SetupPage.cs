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
using System.IO;
using System.Windows.Forms;
using SkinFramework;
using BusinessAccounting.Common;

namespace BusinessAccounting.UI.FrontEndPages
{
    internal class SetupPage : FrontEndPageSkeleton
    {
        public override string PageID
        {
            get { return "menuSetup"; }
        }

        public override string PageHtmlSource
        {
            get { return "setup.html"; }
        }

        public override void Init()
        {
            FrontEndBrowser.Instance.Document.InvokeScript("externalAddLanguageOptions", new object[] { getLanguages() } );
            FrontEndBrowser.Instance.Document.InvokeScript("externalAddThemeOptions", new object[] { getThemes() });
            FrontEndBrowser.Instance.Document.InvokeScript("externalAddSkinOptions", new object[] { getSkins() });
        }

        private readonly string frontendPath = Application.StartupPath + @"\frontend";
        private readonly string frontendThemesPath = Application.StartupPath + @"\frontend\themes";

        private string getLanguages()
        {
            DirectoryInfo dFrontEnd = new DirectoryInfo(frontendPath);
            FileInfo[] fLangs = dFrontEnd.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            string result = "{";
            for (int a=0;a<fLangs.Length;a++)
            {
                result += string.Format("\"{0}\":\"{1}\"", a, fLangs[a].Name.Replace(fLangs[a].Extension, ""));
                if (a < fLangs.Length - 1)
                {
                    result += ",";
                }
            }
            result += "}";
            return result;
        }

        private string getSkins()
        {
            string result = "{";
            int skinsCount = Enum.GetNames(typeof(DefaultSkin)).Length;
            for (int a = 0; a < skinsCount; a++)
            {
                result += string.Format("\"{0}\":\"{1}\"", RegistrySettings.Instance.ReadSetting<int>("Skin") == a ? "active" : a.ToString(), (DefaultSkin)a);
                if (a < skinsCount - 1)
                {
                    result += ",";
                }
            }
            result += "}";
            return result;
        }

        private string getThemes()
        {
            DirectoryInfo dFrontEndThemes = new DirectoryInfo(frontendThemesPath);
            DirectoryInfo[] dThemes = dFrontEndThemes.GetDirectories();
            string result = "{";
            for (int a = 0; a < dThemes.Length; a++)
            {
                result += string.Format("\"{0}\":\"{1}\"", a, dThemes[a].Name);
                if (a < dThemes.Length - 1)
                {
                    result += ",";
                }
            }
            result += "}";
            return result;
        }
    }
}
