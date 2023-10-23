namespace bmcdani_c971_task.Pages;

public partial class CourseDetail : ContentPage
{
	private int selectedCourseId;
	private string selectedCourseName;
	private DateTime courseStartDate;
	private DateTime courseEndDate;
	private string courseStatus;
	private string instructorName;
	private string instructorPhone;
	private string instructorEmail;
	private bool courseNotifyOnStartDate;
	private bool courseNotifyOnEndDate;
	private int termId;

	private bool buttonClicked = false;

	public CourseDetail(int courseId,
						string courseName,
						DateTime courseStartDate,
						DateTime courseEndDate,
						string courseStatus,
						string instructorName,
						string instructorPhone,
						string instructorEmail,
						bool courseNotifyOnStartDate,
						bool courseNotifyOnEndDate)
	{
		InitializeComponent();

		this.selectedCourseId = courseId;
		this.courseStartDate = courseStartDate;
		this.courseEndDate = courseEndDate;
		this.courseStatus = courseStatus;
		this.instructorName = instructorName;
		this.instructorPhone = instructorPhone;
		this.instructorEmail = instructorEmail;
		this.courseNotifyOnStartDate = courseNotifyOnStartDate;
		this.courseNotifyOnEndDate = courseNotifyOnEndDate;

		CourseTitleLbl.Text = courseName;
		CourseStartDatePicker.Date = this.courseStartDate;
		CourseEndDatePicker.Date = this.courseEndDate;
		CourseStatusPicker.SelectedItem = this.courseStatus;
		InstructorNameEntry.Text = this.instructorName;
		InstructorPhoneEntry.Text = this.instructorPhone;
		InstructorEmailEntry.Text = this.instructorEmail;
		CourseNotifyStartCb.IsChecked = this.courseNotifyOnStartDate;
		CourseNotifyEndCb.IsChecked = this.courseNotifyOnEndDate;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		buttonClicked = false;

		Course course = await DataServices.GetCourseById(selectedCourseId);
		if (course != null)
		{
			CourseTitleLbl.Text = course.CourseName;
			CourseStartDatePicker.Date = course.CourseStartDate;
			CourseEndDatePicker.Date = course.CourseEndDate;
			CourseStatusPicker.SelectedItem = course.CourseStatus;
			InstructorNameEntry.Text = course.InstructorName;
			InstructorPhoneEntry.Text = course.InstructorPhone;
			InstructorEmailEntry.Text = course.InstructorEmail;
			CourseNotifyStartCb.IsChecked = course.CourseNotifyOnStartDate;
			CourseNotifyEndCb.IsChecked = course.CourseNotifyOnEndDate;
		}
	}

	private async void EditCourseButton_Clicked(object sender, EventArgs e)
	{
		if (buttonClicked) return;
		buttonClicked = true;

		await Navigation.PushAsync(new EditCoursePage(selectedCourseId, CourseTitleLbl.Text));
	}
}