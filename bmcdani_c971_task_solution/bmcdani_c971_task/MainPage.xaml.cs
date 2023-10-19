using bmcdani_c971_task.Pages;
using SQLite;

namespace bmcdani_c971_task
{
    public partial class MainPage : ContentPage
    {
        private List<Term> terms;
 
        public MainPage()
        {
            InitializeComponent();

            using (var connectionManager = new SQLiteConnManager<Term>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    terms = GetTerms(conn);
                }
            }

            DisplayTerms();
        }

        private List<Term> GetTerms(SQLiteConnection conn)
        {
            return conn.Table<Term>().ToList();
        }

        private void DisplayTerms()
        {
            foreach (var term in terms)
            {
                Button button = new Button
                {
                    Text = term.Name,
                    FontSize = 18,
                    Margin = new Thickness(0, 5),
                    WidthRequest = 100
                };
                termsStackLayout.Children.Add(button);
            }
        }
    }
}