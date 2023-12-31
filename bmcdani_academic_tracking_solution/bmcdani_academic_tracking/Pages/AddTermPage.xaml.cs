namespace bmcdani_academic_tracking.Pages;

public partial class AddTermPage : ContentPage
{
	private bool isAddTermButtonClicked = false;

	public AddTermPage()
	{
		InitializeComponent();

		AddTermStartDatePicker.DateSelected += async (s, e) =>
		{
			if (AddTermStartDatePicker.Date > AddTermEndDatePicker.Date)
			{
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                AddTermEndDatePicker.Date = AddTermStartDatePicker.Date;
			}
		};

		AddTermEndDatePicker.DateSelected += async (s, e) =>
		{
			if (AddTermEndDatePicker.Date < AddTermStartDatePicker.Date)
			{
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                AddTermEndDatePicker.Date = e.OldDate;
            }
		};
	}

	private async void OnAddTerm_Clicked(object sender, EventArgs e)
	{
		if (isAddTermButtonClicked) return;
		isAddTermButtonClicked = true;

		if (!string.IsNullOrEmpty(AddTermNameEntry.Text))
		{
			await DataServices.AddTerm(AddTermNameEntry.Text, AddTermStartDatePicker.Date, AddTermEndDatePicker.Date);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid term name.", "OK");
			isAddTermButtonClicked = false;
		}
	}
}