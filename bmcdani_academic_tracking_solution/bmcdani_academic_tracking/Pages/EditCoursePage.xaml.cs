namespace bmcdani_academic_tracking.Pages;

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
            isSaveCourseButtonClicked = false;
            return;
        }
        if (!IsValidEmail(EditInstructorEmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "Ok");
            isSaveCourseButtonClicked = false;
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

        string courseName = EditCourseNameEntry.Text;

        if (EditCourseNotifyStartCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                101,
                                                                courseName,
                                                                "Course Start Notification",
                                                                $"The course {courseName} starts today!",
                                                                EditCourseStartDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(101, courseName);
            await NotificationData.CancelNotification(cancelId);
        }
        if (EditCourseNotifyEndCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                102,
                                                                courseName,
                                                                "Course End Notification",
                                                                $"The course {courseName} ends today!",
                                                                EditCourseEndDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(102, courseName);
            await NotificationData.CancelNotification(cancelId);
        }

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