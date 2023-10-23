namespace bmcdani_c971_task.Pages;

public partial class AssessmentView : ContentPage
{
	private int courseId;
    private int currentCourseId;
    private string courseName;

    private bool isButtonClicked = false;
    private bool isAssessmentDeleteMode = false;

    public AssessmentView(int courseId, string courseName)
	{
		InitializeComponent();

		this.courseId = courseId;
        this.courseName = courseName;



        UpdateAssessmentGrid();
	}

	private async Task UpdateAssessmentGrid()
	{
        var assessments = await DataServices.GetAssessments(CourseId: currentCourseId);

        assessmentStackLayout.Children.Clear();

        foreach (var assessment in assessments)
		{
            Grid assessmentGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
                },
                Margin = new Thickness(0, 5)
            };

            assessmentGrid.Add(new Label { Text = "Name" }, 0, 0);
            assessmentGrid.Add(new Entry { Text = assessment.AssessmentName }, 1, 0);

            assessmentGrid.Add(new Label { Text = "Start Date" }, 0, 1);
            assessmentGrid.Add(new DatePicker { Date = assessment.AssessmentStartDate }, 1, 1);

            assessmentGrid.Add(new Label { Text = "End Date" }, 0, 2);
            assessmentGrid.Add(new DatePicker { Date = assessment.AssessmentEndDate }, 1, 2);

            assessmentGrid.Add(new Label { Text = "Notify Start" }, 0, 3);
            assessmentGrid.Add(new CheckBox { IsChecked = assessment.AssessmentNotifyStartDate }, 1, 3);

            assessmentGrid.Add(new Label { Text = "Notify End" }, 0, 4);
            assessmentGrid.Add(new CheckBox { IsChecked = assessment.AssessmentNotifyEndDate }, 1, 4);

            assessmentStackLayout.Children.Add(assessmentGrid);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        UpdateAssessmentGrid();
    }

    private async void AddAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (isButtonClicked) return;
        isButtonClicked = true;

        await Navigation.PushAsync(new AddAssessmentPage(currentCourseId));
    }

    private async void EditAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (isButtonClicked) return;
        isButtonClicked = true;

        await Navigation.PushAsync(new EditAssessmentPage(currentCourseId));
    }

    private async void DeleteAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        isAssessmentDeleteMode = !isAssessmentDeleteMode;

        if (isAssessmentDeleteMode)
        {
            DeleteAssessmentBtn.BackgroundColor = Colors.Crimson;
            DeleteAssessmentBtn.BorderColor = Colors.Black;
            DeleteAssessmentBtn.BorderWidth = 2;

            AddAssessmentBtn.IsEnabled = false;
            AddAssessmentBtn.BackgroundColor = Colors.White;
            AddAssessmentBtn.BorderColor = Colors.Black;
            AddAssessmentBtn.BorderWidth = 1;
            AddAssessmentBtn.TextColor = Colors.Black;
            AddAssessmentBtn.Opacity = .2;

            EditAssessmentBtn.IsEnabled = false;
            EditAssessmentBtn.Background = Colors.White;
            EditAssessmentBtn.BorderColor = Colors.Black;
            EditAssessmentBtn.BorderWidth = 1;
            EditAssessmentBtn.TextColor = Colors.Black;
            EditAssessmentBtn.Opacity = .2;
        }
        else
        {
            DeleteAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            DeleteAssessmentBtn.BorderWidth = 0;

            AddAssessmentBtn.IsEnabled = true;
            AddAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            AddAssessmentBtn.BorderWidth = 0;
            AddAssessmentBtn.TextColor = Colors.White;
            AddAssessmentBtn.Opacity = 1;

            EditAssessmentBtn.IsEnabled = true;
            EditAssessmentBtn.Background = Colors.RoyalBlue;
            EditAssessmentBtn.BorderWidth = 0;
            EditAssessmentBtn.TextColor = Colors.White;
            EditAssessmentBtn.Opacity = 1;
        }
    }

    private async void PlaceHolderDelete_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;

        if (isAssessmentDeleteMode)
        {
            bool isConfirmed = await DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (isConfirmed)
            {
                int courseId = (int)clickedButton.CommandParameter;
                await DataServices.RemoveAssessment(courseId);

                await UpdateAssessmentGrid();
            }
        }
    }
}