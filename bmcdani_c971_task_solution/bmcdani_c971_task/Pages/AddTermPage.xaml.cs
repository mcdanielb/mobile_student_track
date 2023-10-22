namespace bmcdani_c971_task.Pages;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();

		AddTermStartDatePicker.DateSelected += async (s, e) =>
		{
			if (AddTermStartDatePicker.Date > AddTermEndDatePicker.Date)
			{
                await DisplayAlert("Error", "Start date cannot be greater than end date.", "Ok");
                AddTermStartDatePicker.Date = e.OldDate;
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
		if (!string.IsNullOrEmpty(AddTermNameEntry.Text))
		{
			await DataServices.AddTerm(AddTermNameEntry.Text, AddTermStartDatePicker.Date, AddTermEndDatePicker.Date);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid term name.", "OK");
		}
	}
}