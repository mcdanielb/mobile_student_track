namespace bmcdani_c971_task.Pages;

public partial class AddCoursePage : ContentPage
{
	Entry courseNameEntry;
	DatePicker startDatePicker;
	DatePicker endDatePicker;
	Picker statusPicker;
	Editor notesEditor;
	Button saveButton;

	int termId;
	string termName;

	public AddCoursePage(int termId, string termName)
	{
		this.termId = termId;
		this.termName = termName;

		courseNameEntry = new Entry { Placeholder = "Course Name" };

		startDatePicker = CustomDatePicker.CreateDatePicker();
		endDatePicker = CustomDatePicker.CreateDatePicker();

		statusPicker = new Picker { Title = "Select Status" };
		statusPicker.ItemsSource = new List<string> { "Planned", "In Progress", "Completed" };

		notesEditor = new Editor { Placeholder = "Notes" };

		saveButton = new Button { Text = "Save" };
		saveButton.Clicked += OnSaveClicked;

		Content = new StackLayout
		{
			Children = { courseNameEntry, startDatePicker, endDatePicker, statusPicker, notesEditor, saveButton }
		};
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(courseNameEntry.Text) || startDatePicker.Date >= endDatePicker.Date)
		{
			await DisplayAlert("Error", "All fields must be entered and the start date must come before the end date.", "Ok");
			return;
		}

		await DataServices.AddCourse(courseNameEntry.Text, startDatePicker.Date, endDatePicker.Date, statusPicker.SelectedItem.ToString(), notesEditor.Text, termId);

		var addedCourse = new Course
		{
			Name = courseNameEntry.Text,
			StartDate = startDatePicker.Date,
			EndDate = endDatePicker.Date,
			Status = statusPicker.SelectedItem.ToString(),
			Notes = notesEditor.Text
		};

		courseNameEntry.Unfocus();
        startDatePicker.Unfocus();
        endDatePicker.Unfocus();
        statusPicker.Unfocus();
        notesEditor.Unfocus();
		saveButton.Unfocus();

        await Navigation.PushAsync(new CourseView(termId, termName));
	}
}