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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;

namespace BusinessAccounting.Common
{
    /// <summary>
    /// Provide methods to get/set app settings
    /// </summary>
    internal class RegistrySettings
    {
        // singleton
        private static readonly RegistrySettings instance = new RegistrySettings();

        private RegistrySettings() { }

        public static RegistrySettings Instance
        {
            get
            {
                return instance;
            }
        }

        // default path in Registry\HKCU
        private const string defaultBranchPath = @"Software\BusinessAccounting";

        /// <summary>
        /// Return setting value from registry
        /// </summary>
        /// <typeparam name="T">string, int</typeparam>
        public T ReadSetting<T>(string pSettingName, T pDefaultValue)
        {
            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.OpenSubKey(defaultBranchPath);

                if (typeof(T) == typeof(bool))
                {
                    return key != null ? (T)(bool.Parse((key.GetValue(pSettingName, pDefaultValue).ToString())) as object) : pDefaultValue;
                }
                else
                {
                    return key != null ? (T)key.GetValue(pSettingName, pDefaultValue) : pDefaultValue;
                }
            }
            catch (Exception ex)
            {
                Messaging.Instance.ShowMessage(
                    ex.Message, // TODO: Replace with lang string reader
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return pDefaultValue;
            }
            finally
            {
                if (key != null)
                    key.Close();
            }
        }

        /// <summary>
        /// Return Size type setting value from registry
        /// </summary>
        public Size ReadSizeSetting(string pSettingName, Size pDefaultSize)
        {
            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.OpenSubKey(defaultBranchPath);

                    TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(Size));
                    return key != null ? (Size)typeConverter.ConvertFromString(key.GetValue(pSettingName, pDefaultSize).ToString()) : pDefaultSize;
            }
            catch (Exception ex)
            {
                Messaging.Instance.ShowMessage(
                    ex.Message, // TODO: Replace with lang string reader
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return pDefaultSize;
            }
            finally
            {
                if (key != null)
                    key.Close();
            }
        }

        /// <summary>
        /// Place new setting value to registry
        /// </summary>
        public bool SaveSetting(string pSettingName, object pValue)
        {
            RegistryKey key = null;
            try
            {
                key = Registry.CurrentUser.CreateSubKey(defaultBranchPath);
                key.SetValue(pSettingName, pValue);
                return true;
            }
            catch (Exception ex)
            {
                Messaging.Instance.ShowMessage(
                   ex.Message, // TODO: Replace with lang string reader
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (key != null)
                    key.Close();
            }
        }
    }
}