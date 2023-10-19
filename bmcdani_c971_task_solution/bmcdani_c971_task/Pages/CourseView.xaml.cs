using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
	private List<Course> courses;
    private int selectedTermId;
    private string selectedTermName;

	public CourseView(int termId, string termName)
	{
		InitializeComponent();

        TermTitleLbl.Text = termName;
        selectedTermId = termId;
        selectedTermName = termName;

		UpdateCourses();
	}

	private void UpdateCourses()
	{
        using (var connectionManager = new SQLiteConnManager<Course>("AcademicTrackerSQLite.db3"))
        {
            using (var conn = connectionManager.GetConnection())
            {
                courses = GetCoursesForTerm(conn, selectedTermId);

                foreach (var course in courses)
                {
                    Button courseButton = new Button
                    {
                        Text = course.Name,
                        FontSize = 18,
                        Margin = new Thickness(0, 5),
                        WidthRequest = 200
                    };
                    coursesStackLayout.Children.Add(courseButton);
                }
            }
        }
        
	}

    private List<Course> GetCoursesForTerm(SQLiteConnection conn, int termId)
    {
        return conn.Table<Course>().Where(c => c.TermId == termId).ToList();
    }
}