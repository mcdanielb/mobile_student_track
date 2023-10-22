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

        AddCourseStartDatePicker.DateSelected += async (s, e) =>
        {
            if (AddCourseStartDatePicker.Date > AddCourseEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be greater than end date.", "Ok");
                AddCourseStartDatePicker.Date = e.OldDate;
            }
        };

        AddCourseEndDatePicker.DateSelected += async (s, e) =>
        {
            if (AddCourseEndDatePicker.Date < AddCourseStartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                AddCourseEndDatePicker.Date = e.OldDate;
            }
        };
    }

	private async void OnAddCourse_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(AddCourseNameEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorNameEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorPhoneEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorEmailEntry.Text) ||
            AddStatusPicker.SelectedItem == null)
		{
            await DisplayAlert("Error", "Please enter all fields.", "Ok");
            return;
        }
		if (!IsValidEmail(AddCourseInstructorEmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "Ok");
            return;
        }

        await DataServices.AddCourse(AddCourseNameEntry.Text, AddCourseStartDatePicker.Date, AddCourseEndDatePicker.Date, AddStatusPicker.SelectedItem.ToString(), AddCourseInstructorNameEntry.Text, AddCourseInstructorPhoneEntry.Text, AddCourseInstructorEmailEntry.Text, termId);
        await Navigation.PopAsync();
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var mail = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}