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

        AddCourseStartDatePicker.DateSelected += (s, e) =>
        {
            AddCourseEndDatePicker.MinimumDate = AddCourseStartDatePicker.Date;
            if (AddCourseStartDatePicker.Date > AddCourseEndDatePicker.Date)
            {
                AddCourseEndDatePicker.Date = AddCourseStartDatePicker.Date;
            }
        };

        AddCourseEndDatePicker.DateSelected += (s, e) =>
        {
            AddCourseStartDatePicker.MaximumDate = AddCourseEndDatePicker.Date;
            if (AddCourseEndDatePicker.Date > AddCourseStartDatePicker.Date)
            {
                AddCourseStartDatePicker.Date = AddCourseEndDatePicker.Date;
            }
        };
    }

	private async void OnAddCourse_Clicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(AddCourseNameEntry.Text))
		{
            await DataServices.AddCourse(AddCourseNameEntry.Text, AddCourseStartDatePicker.Date, AddCourseEndDatePicker.Date, AddStatusPicker.SelectedItem.ToString(), termId);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid course name.", "Ok");
		}
	}
}