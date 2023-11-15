namespace bmcdani_c971_task.Pages;

public partial class AddCoursePage : ContentPage
{
	int termId;
	string termName;

    private bool isAddCourseButtonClicked = false;

	public AddCoursePage(int termId, string termName)
	{
		InitializeComponent();

		this.termId = termId;
		this.termName = termName;

        AddCourseStartDatePicker.DateSelected += async (s, e) =>
        {
            if (AddCourseStartDatePicker.Date > AddCourseEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                AddCourseEndDatePicker.Date = AddCourseStartDatePicker.Date;
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
        if (isAddCourseButtonClicked) return;
        isAddCourseButtonClicked = true;

        if (string.IsNullOrEmpty(AddCourseNameEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorNameEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorPhoneEntry.Text) ||
            string.IsNullOrEmpty(AddCourseInstructorEmailEntry.Text) ||
            AddStatusPicker.SelectedItem == null)
		{
            await DisplayAlert("Error", "Please enter all fields.", "Ok");
            isAddCourseButtonClicked = false;
            return;
        }
		if (!IsValidEmail(AddCourseInstructorEmailEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "Ok");
            isAddCourseButtonClicked = false;
            return;
        }

        await DataServices.AddCourse(AddCourseNameEntry.Text,
                                     AddCourseStartDatePicker.Date,
                                     AddCourseEndDatePicker.Date,
                                     AddStatusPicker.SelectedItem.ToString(),
                                     AddCourseInstructorNameEntry.Text,
                                     AddCourseInstructorPhoneEntry.Text,
                                     AddCourseInstructorEmailEntry.Text,
                                     AddCourseNotifyStartCb.IsChecked,
                                     AddCourseNotifyEndCb.IsChecked,
                                     termId);

        string courseName = AddCourseNameEntry.Text;

        if (AddCourseNotifyStartCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                101,
                                                                courseName,
                                                                "Course Start Notification",
                                                                $"The course {courseName} starts today!",
                                                                AddCourseStartDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(101, courseName);
            await NotificationData.CancelNotification(cancelId);
        }
        if (AddCourseNotifyEndCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                102,
                                                                courseName,
                                                                "Course End Notification",
                                                                $"The course {courseName} ends today!",
                                                                AddCourseEndDatePicker.Date);
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