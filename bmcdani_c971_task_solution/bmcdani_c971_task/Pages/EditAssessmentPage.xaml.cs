namespace bmcdani_c971_task.Pages;

public partial class EditAssessmentPage : ContentPage
{
	private int assessmentId;
    private int courseId;
    private Assessment assessment;

	private bool isSaveAssessmentButtonClicked = false;

	public EditAssessmentPage(int assessmentId)
	{
		InitializeComponent();

		this.assessmentId = assessmentId;
        this.courseId = courseId;

        LoadAssessmentData();

        EditAssessmentStartDatePicker.DateSelected += async (s, e) =>
        {
            if (EditAssessmentStartDatePicker.Date > EditAssessmentEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                EditAssessmentEndDatePicker.Date = EditAssessmentStartDatePicker.Date;
            }
        };

        EditAssessmentEndDatePicker.DateSelected += async (s, e) =>
        {
            if (EditAssessmentEndDatePicker.Date < EditAssessmentStartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                EditAssessmentEndDatePicker.Date = e.OldDate;
            }
        };
    }

    private async void LoadAssessmentData()
    {
        assessment = await DataServices.GetAssessmentById(assessmentId);

        EditAssessmentNameEntry.Text = assessment.AssessmentName;
        EditAssessmentStartDatePicker.Date = assessment.AssessmentStartDate;
        EditAssessmentEndDatePicker.Date = assessment.AssessmentEndDate;
        EditAssessmentTypePicker.SelectedItem = assessment.AssessmentType;
        EditAssessmentNotifyStartCb.IsChecked = assessment.AssessmentNotifyStartDate;
        EditAssessmentNotifyEndCb.IsChecked = assessment.AssessmentNotifyEndDate;
        courseId = assessment.CourseId;
    }

    private async void OnSaveAssessment_Clicked(object sender, EventArgs e)
    {
        if (isSaveAssessmentButtonClicked) return;
        isSaveAssessmentButtonClicked = true;

        if (string.IsNullOrEmpty(EditAssessmentNameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter a name.", "Ok");
            isSaveAssessmentButtonClicked = false;
            return;
        }

        await DataServices.UpdateAssessment(assessmentId,
                                            EditAssessmentNameEntry.Text,
                                            EditAssessmentStartDatePicker.Date,
                                            EditAssessmentEndDatePicker.Date,
                                            EditAssessmentTypePicker.SelectedItem.ToString(),
                                            EditAssessmentNotifyStartCb.IsChecked,
                                            EditAssessmentNotifyEndCb.IsChecked,
                                            courseId);
        
        if (EditAssessmentNotifyStartCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true, 
                                                                 201,
                                                                 assessment.AssessmentName,
                                                                 "Assessment Start Notification",
                                                                 $"The assessment {assessment.AssessmentName} starts today!",
                                                                 EditAssessmentStartDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(201, assessment.AssessmentName);
            await NotificationData.CancelNotification(cancelId);
        }
        if (EditAssessmentNotifyEndCb.IsChecked)
        {
            await NotificationData.ScheduleOrCancelNotification(true,
                                                                202,
                                                                assessment.AssessmentName,
                                                                "Assessment End Notification",
                                                                $"The assessment {assessment.AssessmentName} ends today!",
                                                                EditAssessmentEndDatePicker.Date);
        }
        else
        {
            int cancelId = NotificationData.GenerateNotificationId(202, assessment.AssessmentName);
            await NotificationData.CancelNotification(cancelId);
        }

        await Navigation.PopAsync();
    }
}