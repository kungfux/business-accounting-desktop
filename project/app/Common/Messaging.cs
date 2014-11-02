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
using System.Reflection;
using System.Windows.Forms;

namespace BusinessAccounting.Common
{
    /// <summary>
    /// Provide methods for displaying messages to users
    /// </summary>
    internal class Messaging
    {
        // singleton
        private static readonly Messaging instance = new Messaging();

        private Messaging() { }

        public static Messaging Instance
        {
            get
            {
                return instance;
            }
        }

        // assembly product attribute
        public readonly string appProductName =
            ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false))
            .Product;

        /// <summary>
        /// Show standard MessageBox from System.Windows.Forms
        /// </summary>
        public DialogResult ShowMessage(string pMessageText, MessageBoxButtons pMessageBoxButtons, MessageBoxIcon pMessageBoxIcon)
        {
            return MessageBox.Show(pMessageText, appProductName, pMessageBoxButtons, pMessageBoxIcon);
        }
    }
}
