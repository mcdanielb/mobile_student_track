using bmcdani_c971_task.ViewModels;
using System.Text;

namespace bmcdani_c971_task.Pages;

public partial class CourseDetail : ContentPage
{
	private Course currentCourse;
	private bool isButtonClicked = false;

	public CourseDetail(Course course)
	{
		InitializeComponent();

		this.currentCourse = course;
		this.BindingContext = new CourseDetailViewModel(course.CourseId);

	}

	private void PopulateUICourseData()
	{
		if (currentCourse != null)
		{
            CourseTitleLbl.Text = currentCourse.CourseName;
            CourseStartDatePicker.Date = currentCourse.CourseStartDate;
            CourseEndDatePicker.Date = currentCourse.CourseEndDate;
            CourseStatusPicker.SelectedItem = currentCourse.CourseStatus;
            InstructorNameEntry.Text = currentCourse.InstructorName;
            InstructorPhoneEntry.Text = currentCourse.InstructorPhone;
            InstructorEmailEntry.Text = currentCourse.InstructorEmail;
            CourseNotifyStartCb.IsChecked = currentCourse.CourseNotifyOnStartDate;
            CourseNotifyEndCb.IsChecked = currentCourse.CourseNotifyOnEndDate;
        }
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		isButtonClicked = false;

		currentCourse = await DataServices.GetCourseById(currentCourse.CourseId);
		PopulateUICourseData();
	}

	private async void EditCourseButton_Clicked(object sender, EventArgs e)
	{
		if (isButtonClicked) return;
		isButtonClicked = true;

		await Navigation.PushAsync(new EditCoursePage(currentCourse.CourseId,
													  CourseTitleLbl.Text,
													  CourseStartDatePicker.Date,
													  CourseEndDatePicker.Date,
													  CourseStatusPicker.SelectedItem.ToString(),
													  InstructorNameEntry.Text,
													  InstructorPhoneEntry.Text,
													  InstructorEmailEntry.Text,
													  CourseNotifyStartCb.IsChecked,
													  CourseNotifyEndCb.IsChecked,
													  currentCourse.TermId));
	}

	private void OnAddNoteButtonClicked(object sender, EventArgs e)
	{
		AddNoteEntry.Text = string.Empty;
	}

    private async void AssessmentsButton_Clicked(object sender, EventArgs e)
	{
		if (isButtonClicked) return;
		isButtonClicked = true;

		await Navigation.PushAsync(new AssessmentView(currentCourse.CourseId, currentCourse.CourseName));
	}
}