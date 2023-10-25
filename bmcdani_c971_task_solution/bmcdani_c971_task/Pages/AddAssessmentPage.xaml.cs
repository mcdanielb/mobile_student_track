namespace bmcdani_c971_task.Pages;

public partial class AddAssessmentPage : ContentPage
{
    int courseId;
    string courseName;

    private bool isAddAssessmentButtonClicked = false;

	public AddAssessmentPage(int courseId, string courseName)
	{
		InitializeComponent();

        this.courseId = courseId;
        this.courseName = courseName;

        AddAssessmentStartDatePicker.DateSelected += async (s, e) =>
        {
            if (AddAssessmentStartDatePicker.Date > AddAssessmentEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                AddAssessmentEndDatePicker.Date = AddAssessmentStartDatePicker.Date;
            }
        };

        AddAssessmentEndDatePicker.DateSelected += async (s, e) =>
        {
            if (AddAssessmentEndDatePicker.Date < AddAssessmentStartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                AddAssessmentEndDatePicker.Date = e.OldDate;
            }
        };
    }

	private async void OnAddAssessment_Clicked(object sender, EventArgs e)
	{
        if (isAddAssessmentButtonClicked) return;
        isAddAssessmentButtonClicked = true;

        if (string.IsNullOrEmpty(AddAssessmentNameEntry.Text) ||
            AddAssessmentTypePicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please enter all fields.", "Ok");
            isAddAssessmentButtonClicked = false;
            return;
        }

        await DataServices.AddAssessment(AddAssessmentNameEntry.Text,
                                         AddAssessmentStartDatePicker.Date,
                                         AddAssessmentEndDatePicker.Date,
                                         AddAssessmentTypePicker.SelectedItem.ToString(),
                                         AddAssessmentNotifyStartCb.IsChecked,
                                         AddAssessmentNotifyEndCb.IsChecked,
                                         courseId);

        string assessmentName = AddAssessmentNameEntry.Text;

        if (AddAssessmentNotifyStartCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                 201,
                                                                 assessmentName,
                                                                 "Assessment Start Notification",
                                                                 $"The assessment {assessmentName} starts today!",
                                                                 AddAssessmentStartDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(201, assessmentName);
            await NotificationData.CancelNotification(cancelId);
        }
        if (AddAssessmentNotifyEndCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                202,
                                                                assessmentName,
                                                                "Assessment End Notification",
                                                                $"The assessment {assessmentName} ends today!",
                                                                AddAssessmentEndDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(202, assessmentName);
            await NotificationData.CancelNotification(cancelId);
        }

        await Navigation.PopAsync();
    }
}