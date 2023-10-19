using bmcdani_c971_task.Pages;

namespace bmcdani_c971_task
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            InitializeDatabase();

            MainPage = new AppShell();
        }

        private void InitializeDatabase()
        {
            using (SQLiteConnManager<Term> connectionManager = new SQLiteConnManager<Term>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    if (conn.Table<Term>().Any())
                    {
                        return;
                    }

                    conn.Insert(new Term { Name = "Term 1" });
                    conn.Insert(new Term { Name = "Term 2" });
                    conn.Insert(new Term { Name = "Term 3" });
                }
            }

            using (SQLiteConnManager<Course> connectionManager = new SQLiteConnManager<Course>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    if (conn.Table<Course>().Any())
                    {
                        return;
                    }

                    conn.Insert(new Course { Name = "English", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                    conn.Insert(new Course { Name = "Math", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                    conn.Insert(new Course { Name = "Science", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                    conn.Insert(new Course { Name = "Biology", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                    conn.Insert(new Course { Name = "Computers", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                    conn.Insert(new Course { Name = "Drafting", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2018, 2, 1), Status = "In Progress", Notes = "This is an example.", TermId = 1 });
                }
            }
        }
    }
}