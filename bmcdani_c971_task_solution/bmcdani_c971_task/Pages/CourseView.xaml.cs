using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
    private int selectedTermId;
    private string selectedTermName;

    private bool buttonClicked = false;
    //private bool isDeleteMode = false;

	public CourseView(int termId, string termName)
	{
		InitializeComponent();

        TermTitleLbl.Text = termName;
        selectedTermId = termId;
        selectedTermName = termName;

        UpdateCoursesAsync();
	}

	private async Task UpdateCoursesAsync()
	{
        var courses = await DataServices.GetCourses(TermId: selectedTermId);

        coursesStackLayout.Children.Clear();

        foreach (var course in courses)
        {
            Button courseButton = new Button
            {
                Text = course.CourseName,
                FontSize = 18,
                Margin = new Thickness(0, 5),
                WidthRequest = 200,
                CommandParameter = course.CourseId
            };
            courseButton.Clicked += CourseButton_Clicked;
            coursesStackLayout.Children.Add(courseButton);
        }
    }

    private async void DeleteCourseBtn_Clicked(object sender, EventArgs e)
    {

    }

    private async void CourseButton_Clicked(object sender, EventArgs e)
    {
        if (buttonClicked) return;

        buttonClicked = true;

        Button clickedButton = (Button)sender;
        int selectedCourseId = (int)clickedButton.CommandParameter;

        Course selectedCourse = (await DataServices.GetCourses()).FirstOrDefault(c => c.CourseId == selectedCourseId);
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