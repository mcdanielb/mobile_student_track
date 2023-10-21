namespace bmcdani_c971_task.Pages;

public partial class AddCoursePage : ContentPage
{
	int termId;
	string termName;

	public AddCoursePage(int termId, string termName)
	{
		InitializeComponent();

		this.termId = termId;
		this.termName = termName;

        CourseStartDatePicker.DateSelected += (s, e) =>
        {
            CourseEndDatePicker.MinimumDate = CourseStartDatePicker.Date;
            if (CourseStartDatePicker.Date > CourseEndDatePicker.Date)
            {
                CourseEndDatePicker.Date = CourseStartDatePicker.Date;
            }
        };

        CourseEndDatePicker.DateSelected += (s, e) =>
        {
            CourseStartDatePicker.MaximumDate = CourseEndDatePicker.Date;
            if (CourseEndDatePicker.Date > CourseStartDatePicker.Date)
            {
                CourseStartDatePicker.Date = CourseEndDatePicker.Date;
            }
        };
    }

	private async void OnAddCourse_Clicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(CourseNameEntry.Text))
		{
            await DataServices.AddCourse(CourseNameEntry.Text, CourseStartDatePicker.Date, CourseEndDatePicker.Date, StatusPicker.SelectedItem.ToString(), termId);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid course name.", "Ok");
		}
	}
}