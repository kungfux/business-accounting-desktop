﻿/* Business Accounting 
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

namespace BusinessAccounting.UI.FrontEndPages
{
    internal class CashPage : FrontEndPageSkeleton
    {
        public override string PageID
        {
            get { return "menuCash"; }
        }

        public override string PageHtmlSource
        {
            get { return "cash.html"; }
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }
}