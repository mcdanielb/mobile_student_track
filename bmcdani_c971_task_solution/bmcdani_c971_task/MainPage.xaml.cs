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

            UpdateTerms();
        }

        private void UpdateTerms()
        {
            using (var connectionManager = new SQLiteConnManager<Term>("AcademicTrackerSQLite.db3"))
            {
                using (var conn = connectionManager.GetConnection())
                {
                    terms = GetTerms(conn);

                    foreach (var term in terms)
                    {
                        Button termButton = new Button
                        {
                            Text = term.Name,
                            FontSize = 18,
                            Margin = new Thickness(0, 5),
                            WidthRequest = 200,
                            CommandParameter = term.Id
                        };
                        termButton.Clicked += TermButton_Clicked;
                        termsStackLayout.Children.Add(termButton);
                    }
                }
            }
        }

        private List<Term> GetTerms(SQLiteConnection conn)
        {
            return conn.Table<Term>().ToList();
        }

        private void TermButton_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int selectedTermId = (int)clickedButton.CommandParameter;
        }

        private void AddTermBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}