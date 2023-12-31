namespace bmcdani_academic_tracking.Pages;

public partial class AssessmentView : ContentPage
{
	private int courseId;
    private int assessmentId;
    private string courseName;

    private bool isButtonClicked = false;
    private bool isAssessmentDeleteMode = false;
    private bool isAssessmentEditMode = false;

    public AssessmentView(int courseId, string courseName)
	{
		InitializeComponent();

		this.courseId = courseId;
        this.courseName = courseName;
        this.assessmentId = assessmentId;


        UpdateAssessmentGrid();
	}

	private async Task UpdateAssessmentGrid()
	{
        var assessments = await DataServices.GetAssessments(CourseId: courseId);

        assessmentStackLayout.Children.Clear();

        foreach (var assessment in assessments)
        {
            Grid assessmentGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 20,

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
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
                Margin = new Thickness(50, 10)
            };

            assessmentGrid.Add(new Label { Text = "Name:", VerticalOptions = LayoutOptions.Center }, 0, 1);
            assessmentGrid.Add(new Entry { IsEnabled = false, Text = assessment.AssessmentName, MaxLength = 30, TextColor = Colors.Black, HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center }, 1, 1);

            assessmentGrid.Add(new Label { Text = "Start Date:", VerticalOptions = LayoutOptions.Center }, 0, 2);
            assessmentGrid.Add(new DatePicker { IsEnabled = false, Date = assessment.AssessmentStartDate, TextColor = Colors.Black, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }, 1, 2);

            assessmentGrid.Add(new Label { Text = "End Date:", VerticalOptions = LayoutOptions.Center }, 0, 3);
            assessmentGrid.Add(new DatePicker { IsEnabled = false, Date = assessment.AssessmentEndDate, TextColor = Colors.Black, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }, 1, 3);

            assessmentGrid.Add(new Label { Text = "Notify Start:", VerticalOptions = LayoutOptions.Center }, 0, 4);
            assessmentGrid.Add(new CheckBox { IsEnabled = false, IsChecked = assessment.AssessmentNotifyStartDate, VerticalOptions = LayoutOptions.Center }, 1, 4);

            assessmentGrid.Add(new Label { Text = "Notify End:", VerticalOptions = LayoutOptions.Center }, 0, 5);
            assessmentGrid.Add(new CheckBox { IsEnabled = false, IsChecked = assessment.AssessmentNotifyEndDate, VerticalOptions = LayoutOptions.Center }, 1, 5);

            Button assessmentTypeBtn = new Button
            {
                IsEnabled = false,
                Text = assessment.AssessmentType,
                TextColor = Colors.White,
                BackgroundColor = Colors.MidnightBlue,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                //WidthRequest = 195
            };

            assessmentGrid.Add(assessmentTypeBtn, 0, 0);
            Grid.SetColumnSpan(assessmentTypeBtn, 2);

            Button selectAssessmentBtn = new Button
            {
                Text = "Select",
                CommandParameter = assessment.AssessmentId,
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0,
                Opacity = 0.3
            };

            selectAssessmentBtn.Clicked += SelectAssessmentBtn_Clicked;

            assessmentGrid.Add(selectAssessmentBtn, 0, 0);
            Grid.SetRowSpan(selectAssessmentBtn, 6);
            Grid.SetColumnSpan(selectAssessmentBtn, 2);

            assessmentStackLayout.Children.Add(assessmentGrid);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        isButtonClicked = false;

        UpdateAssessmentGrid();
    }

    private async void AddAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (isButtonClicked) return;
        isButtonClicked = true;

        await Navigation.PushAsync(new AddAssessmentPage(courseId, courseName));
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
            EditAssessmentBtn.BackgroundColor = Colors.White;
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
            EditAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            EditAssessmentBtn.BorderWidth = 0;
            EditAssessmentBtn.TextColor = Colors.White;
            EditAssessmentBtn.Opacity = 1;
        }
    }

    private async void EditAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        isAssessmentEditMode = !isAssessmentEditMode;

        if (isAssessmentEditMode)
        {
            EditAssessmentBtn.BackgroundColor = Colors.MidnightBlue;
            EditAssessmentBtn.BorderColor = Colors.Black;
            EditAssessmentBtn.BorderWidth = 2;

            AddAssessmentBtn.IsEnabled = false;
            AddAssessmentBtn.BackgroundColor = Colors.White;
            AddAssessmentBtn.BorderColor = Colors.Black;
            AddAssessmentBtn.BorderWidth = 1;
            AddAssessmentBtn.TextColor = Colors.Black;
            AddAssessmentBtn.Opacity = .2;

            DeleteAssessmentBtn.IsEnabled = false;
            DeleteAssessmentBtn.BackgroundColor = Colors.White;
            DeleteAssessmentBtn.BorderColor = Colors.Black;
            DeleteAssessmentBtn.BorderWidth = 1;
            DeleteAssessmentBtn.TextColor = Colors.Black;
            DeleteAssessmentBtn.Opacity = .2;
        }
        else
        {
            EditAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            EditAssessmentBtn.BorderWidth = 0;

            AddAssessmentBtn.IsEnabled = true;
            AddAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            AddAssessmentBtn.BorderWidth = 0;
            AddAssessmentBtn.TextColor = Colors.White;
            AddAssessmentBtn.Opacity = 1;

            DeleteAssessmentBtn.IsEnabled = true;
            DeleteAssessmentBtn.BackgroundColor = Colors.RoyalBlue;
            DeleteAssessmentBtn.BorderWidth = 0;
            DeleteAssessmentBtn.TextColor = Colors.White;
            DeleteAssessmentBtn.Opacity = 1;
        }
    }

    private async void SelectAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        int assessmentId = (int)clickedButton.CommandParameter;

        if (isAssessmentDeleteMode)
        {
            bool isConfirmed = await DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (isConfirmed)
            {
                await DataServices.RemoveAssessment(assessmentId);
                await UpdateAssessmentGrid();
            }
        }
        if (isAssessmentEditMode)
        {
            await Navigation.PushAsync(new EditAssessmentPage(assessmentId));
        }
    }
}