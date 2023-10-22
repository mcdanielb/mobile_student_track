namespace bmcdani_c971_task.Pages;

public partial class CourseDetail : ContentPage
{
	private int selectedCourseId;
	private string selectedCourseName;
	private DateTime courseStartDate;
	private DateTime courseEndDate;

	private bool buttonClicked = false;

	public CourseDetail(int courseId, string courseName)
	{
		InitializeComponent();
	}
}