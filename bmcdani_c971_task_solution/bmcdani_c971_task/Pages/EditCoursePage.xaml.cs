namespace bmcdani_c971_task.Pages;

public partial class EditCoursePage : ContentPage
{
	private int courseId;
    private int termId;

    private bool isSaveCourseButtonClicked = false;

	public EditCoursePage(int courseId,
						  string courseName,
						  DateTime courseStartDate,
						  DateTime courseEndDate,
						  string courseStatus,
						  string instructorName,
						  string instructorPhone,
						  string instructorEmail,
						  bool courseNotifyOnStartDate,
						  bool courseNotifyOnEndDate,
                          int termId)

	{
		InitializeComponent();

		this.courseId = courseId;
        this.termId = termId;

		EditCourseNameEntry.Text = courseName;
		EditCourseStartDatePicker.Date = courseStartDate;
		EditCourseEndDatePicker.Date = courseEndDate;
		EditStatusPicker.SelectedItem = courseStatus;
		EditInstructorNameEntry.Text = instructorName;
		EditInstructorPhoneEntry.Text = instructorPhone;
		EditInstructorEmailEntry.Text = instructorEmail;
		EditCourseNotifyStartCb.IsChecked = courseNotifyOnStartDate;
		EditCourseNotifyEndCb.IsChecked = courseNotifyOnEndDate;

        EditCourseStartDatePicker.DateSelected += async (s, e) =>
        {
            if (EditCourseStartDatePicker.Date > EditCourseEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                EditCourseEndDatePicker.Date = EditCourseStartDatePicker.Date;
            }
        };

        EditCourseEndDatePicker.DateSelected += async (s, e) =>
        {
            if (EditCourseEndDatePicker.Date < EditCourseStartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                EditCourseEndDatePicker.Date = e.OldDate;
            }
        };
    }

	private async void OnSaveCourseChanges_Clicked(object sender, EventArgs e)
	{
        if (isSaveCourseButtonClicked) return;
        isSaveCourseButtonClicked = true;

        if (string.IsNullOrEmpty(EditCourseNameEntry.Text) ||
            string.IsNullOrEmpty(EditInstructorNameEntry.Text) ||
            string.IsNullOrEmpty(EditInstructorPhoneEntry.Text) ||
            string.IsNullOrEmpty(EditInstructorEmailEntry.Text) ||
            EditStatusPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please enter all fields.", "Ok");
            return;
        }
        if (!IsValidEmail(EditInstructorEmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "Ok");
            return;
        }

        await DataServices.UpdateCourse(courseId,
                                        EditCourseNameEntry.Text,
                                        EditCourseStartDatePicker.Date,
                                        EditCourseEndDatePicker.Date,
                                        EditStatusPicker.SelectedItem.ToString(),
                                        EditInstructorNameEntry.Text,
                                        EditInstructorPhoneEntry.Text,
                                        EditInstructorEmailEntry.Text,
                                        EditCourseNotifyStartCb.IsChecked,
                                        EditCourseNotifyEndCb.IsChecked,
                                        termId);
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