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
using System.Threading;
using System.Windows.Forms;
using BusinessAccounting.Common;

namespace BusinessAccounting
{
    internal class Program
    {
        // app mutex name
        private const string appMutexName = "business-accounting";

        [STAThread]
        public static void Main()
        {
            // allow running single instance only
            using (Mutex mutex = new Mutex(false, appMutexName))
            {
                bool appAlreadyRunning = !mutex.WaitOne(0, false);
                if (appAlreadyRunning)
                {
                    // init language support
                    LanguageSupport language = new LanguageSupport();
                    // if app already running
                    // show message and exit
                    Messaging.Instance.ShowMessage(
                        language.GetStringByID("WarningOneInstance"),
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    // launch app instance
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //Application.Run(new BusinessAccountingForm()); // TODO: Call new form creation
                }
            }
        }
    }
}
