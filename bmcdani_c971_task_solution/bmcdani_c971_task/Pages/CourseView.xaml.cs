using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
	private List<Course> courses;

	public CourseView(int selectedTermId)
	{
		InitializeComponent();

		using (var connectionManager = new SQLiteConnManager<Course>("AcademicTrackerSQLite.db3"))
		{
			using (var conn = connectionManager.GetConnection())
			{
				courses = GetCoursesForTerm(conn, selectedTermId);
			}
		}

		UpdateCourses();
	}

	private List<Course> GetCoursesForTerm(SQLiteConnection conn, int termId)
	{
		return conn.Table<Course>().Where(c => c.TermId == termId).ToList();
	}

	private void UpdateCourses()
	{
		foreach (var course in courses)
		{
			Button courseButton = new Button
			{
				Text = course.Name,
				FontSize = 18,
				Margin = new Thickness(0, 5),
				WidthRequest = 100
			};
            coursesStackLayout.Children.Add(courseButton);
        }
	}
}