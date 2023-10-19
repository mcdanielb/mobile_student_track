using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmcdani_c971_task
{
    public class SQLiteConnManager<T> : IDisposable where T : new()
    {
        private SQLiteConnection connection;

        public SQLiteConnManager(string databaseFilename)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseFilename);

            connection = new SQLiteConnection(databasePath);
            connection.CreateTable<T>();

        }

        public SQLiteConnection GetConnection()
        {
            return connection;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
