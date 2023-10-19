using bmcdani_c971_task.Pages;

namespace bmcdani_c971_task
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (SQLiteConnManager<Course> connectionManager = new SQLiteConnManager<Course>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    if (conn.Table<Course>().Any())
                    {
                        return;
                    }

                    conn.Insert(new Course { Name = "English", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                    conn.Insert(new Course { Name = "Math", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                    conn.Insert(new Course { Name = "Science", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                    conn.Insert(new Course { Name = "Biology", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                    conn.Insert(new Course { Name = "Computers", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                    conn.Insert(new Course { Name = "Drafting", Term = "Term 1", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example." });
                }
            }

            using (SQLiteConnManager<Term> connectionManager = new SQLiteConnManager<Term>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    if (conn.Table<Term>().Any())
                    {
                        return;
                    }

                    conn.Insert(new Course { Name = "Term 1" });
                    conn.Insert(new Course { Name = "Term 2" });
                    conn.Insert(new Course { Name = "Term 3" });
                }
            }
        }
    }
}