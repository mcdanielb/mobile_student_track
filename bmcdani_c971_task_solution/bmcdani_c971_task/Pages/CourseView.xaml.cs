using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
	private List<Course> courses;

	public CourseView(string selectedTerm)
	{
		InitializeComponent();

		using (var connectionManager = new SQLiteConnManager<Course>("AcademicTrackerSQLite.db3"))
		{
			using (var conn = connectionManager.GetConnection())
			{
				courses = GetCoursesForTerm(conn, selectedTerm);
			}
		}

		DisplayCourses();
	}

	private List<Course> GetCoursesForTerm(SQLiteConnection conn, string term)
	{
		return conn.Table<Course>().Where(c => c.Term == term).ToList();
	}

	private void DisplayCourses()
	{
		foreach (var course in courses)
		{
			Button button = new Button
			{
				Text = course.Name,
				FontSize = 18,
				Margin = new Thickness(0, 5),
				WidthRequest = 100
			};
            coursesStackLayout.Children.Add(button);
        }
	}
}