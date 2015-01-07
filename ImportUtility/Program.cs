using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xclass.Database;

namespace ImportUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            OleDb mdb = new OleDb();
            SQLite sqlite = new SQLite();

            if (!mdb.TestConnection("Provider=Microsoft.Jet.OleDb.4.0;Data Source=db.mdb;", true))
            {
                Console.WriteLine("Unable to open db.mdb database!");
                exit();
                return;
            }
            if (!sqlite.TestConnection("Data Source=ba.sqlite;Version=3;FailIfMissing=True;UTF8Encoding=True;foreign keys=true;", true, true))
            {
                Console.WriteLine("Unable to open ba.sqlite database!");
                exit();
                return;
            }

            DataTable mdbData = mdb.SelectTable("select dt, sumd from year_period union select dt, 0 - sumd from year_period2;");

            if (mdbData == null || mdbData.Rows.Count == 0)
            {
                Console.WriteLine("No records to import!");
                exit();
                return;
            }

            Console.WriteLine(mdbData.Rows.Count.ToString() + " records are qualified for import. Processing...");

            StreamWriter file = null;

            foreach (DataRow r in mdbData.Rows)
            {
                if (sqlite.ChangeData("insert into ba_cash_operations (datestamp, summa) values (@d, @s);",
                    new SQLiteParameter("@d", r.ItemArray[0]),
                    new SQLiteParameter("@s", r.ItemArray[1])) <= 0)
                {
                    Console.Write("E");
                    if (file == null)
                    {
                        file = new StreamWriter("import-errors.txt");
                    }
                    file.WriteLine(sqlite.LastOperationErrorMessage);
                }
                else
                {
                    Console.Write(".");
                }
            }

            if (file != null)
            {
                file.Close();
            }

            sqlite.Disconnect();

            Console.WriteLine("");

            exit();
            return;
        }

        static void exit()
        {
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
