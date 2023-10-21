using SQLite;

namespace bmcdani_c971_task.Pages;

public partial class CourseView : ContentPage
{
    private int selectedTermId;
    private string selectedTermName;
    private DateTime termStartDate;
    private DateTime termEndDate;

    private bool buttonClicked = false;
    private bool isCourseDeleteMode = false;

	public CourseView(int termId, string termName, DateTime termStartDate, DateTime termEndDate)
	{
		InitializeComponent();

        TermTitleLbl.Text = termName;
        selectedTermId = termId;
        selectedTermName = termName;
        this.termStartDate = termStartDate;
        this.termEndDate = termEndDate;

        TermStartDatePicker.Date = termStartDate;
        TermEndDatePicker.Date = termEndDate;

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
        isCourseDeleteMode = !isCourseDeleteMode;

        if (isCourseDeleteMode)
        {
            DeleteCourseBtn.BackgroundColor = Colors.Gray;
        }
        else
        {
            DeleteCourseBtn.BackgroundColor = Colors.Crimson;
        }
    }

    private async void CourseButton_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;

        if (isCourseDeleteMode)
        {
            bool isConfirmed = await DisplayAlert("Delete Course", "Are you sure you want to delete this course?", "Yes", "No");
            if (isConfirmed)
            {
                int selectedCourseId = (int)clickedButton.CommandParameter;
                await DataServices.RemoveCourse(selectedCourseId);

                await UpdateCoursesAsync();
            }
        }
        else
        {
            if (buttonClicked) return;
            buttonClicked = true;

            int selectedCourseId = (int)clickedButton.CommandParameter;

            Course selectedCourse = (await DataServices.GetCourses()).FirstOrDefault(c => c.CourseId == selectedCourseId);
            if (selectedCourse != null)
            {
                await Navigation.PushAsync(new CourseDetail(selectedCourseId, selectedCourse.CourseName));
            }
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        buttonClicked = false;

        Term term = await DataServices.GetTermById(selectedTermId);
        if (term != null)
        {
            TermTitleLbl.Text = term.TermName;
            TermStartDatePicker.Date = term.TermStartDate;
            TermEndDatePicker.Date = term.TermEndDate;
        }

        UpdateCoursesAsync();
    }

    private async void AddCourseBtn_Clicked(object sender, EventArgs e)
    {
        if (buttonClicked) return;
        buttonClicked = true;

        await Navigation.PushAsync(new AddCoursePage(selectedTermId, selectedTermName));
    }

    private async void EditTermBtn_Clicked(object sender, EventArgs e)
    {
        if (buttonClicked) return;
        buttonClicked = true;

        await Navigation.PushAsync(new EditTermPage(selectedTermId, TermTitleLbl.Text, TermStartDatePicker.Date, TermEndDatePicker.Date));
    }
}