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

using System.Resources;
using System.Windows.Forms;

namespace BusinessAccounting.Common
{
    /// <summary>
    /// Provides multilaguage support
    /// </summary>
    internal class LanguageSupport
    {
        // TODO: Avoid using non-language resources somehow
        // TODO: Add checking of language resource compatibility
        // TODO: Add method to return list of available language resources

        private static ResourceManager resManager;
        public string Language { get { return resManager.BaseName; } }
        private readonly string defaultLanguage = RegistrySettings.Instance.ReadSetting<string>("Language");

        public LanguageSupport()
        {
            resManager = ResourceManager.CreateFileBasedResourceManager(defaultLanguage, ".", null);
            resManager.IgnoreCase = true;
        }

        /// <summary>
        /// Return text from resource file by string id
        /// </summary>
        public string GetStringByID(string pStringID)
        {
            string result = "#UNDEFINED TEXT#";
            try
            {
                string langvalue = resManager.GetString(pStringID);
                result = langvalue != null && langvalue != "" ? langvalue : result;
            }
            catch (MissingManifestResourceException)
            {
                Messaging.Instance.ShowMessage(
                     string.Format("{0} language file was not found!", this.Language),
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
    }
}
