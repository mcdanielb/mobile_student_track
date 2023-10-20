using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
	private List<Course> courses;
    private int selectedTermId;
    private string selectedTermName;
    private bool buttonClicked = false;

	public CourseView(int termId, string termName)
	{
		InitializeComponent();

        TermTitleLbl.Text = termName;
        selectedTermId = termId;
        selectedTermName = termName;

        UpdateCourses();
	}

	private async Task UpdateCourses()
	{
        var courses = await DataServices.GetCourses(termId: selectedTermId);

        coursesStackLayout.Children.Clear();

        foreach (var course in courses)
        {
            Button courseButton = new Button
            {
                Text = course.Name,
                FontSize = 18,
                Margin = new Thickness(0, 5),
                WidthRequest = 200,
                CommandParameter = course.Id
            };
            coursesStackLayout.Children.Add(courseButton);
        }
    }

    private async void CourseButton_Clicked(object sender, EventArgs e)
    {
        if (buttonClicked) return;

        buttonClicked = true;

        Button clickedButton = (Button)sender;
        int selectedCourseId = (int)clickedButton.CommandParameter;

        Course selectedCourse = (await DataServices.GetCourses()).FirstOrDefault(c => c.Id == selectedCourseId);
        if (selectedCourse != null)
        {
            // await Navigation.PushAsync(new CourseDetail(selectedCourseId, selectedCourse.Name));
        }
    }

    private async void AddCourseBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCoursePage(selectedTermId, selectedTermName));
    }
}