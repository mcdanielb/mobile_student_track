namespace bmcdani_c971_task.Pages;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();

		AddTermStartDatePicker.DateSelected += (s, e) =>
		{
			AddTermEndDatePicker.MinimumDate = AddTermStartDatePicker.Date;
			if (AddTermStartDatePicker.Date > AddTermEndDatePicker.Date)
			{
				AddTermEndDatePicker.Date = AddTermStartDatePicker.Date;
			}
		};

		AddTermEndDatePicker.DateSelected += (s, e) =>
		{
			AddTermStartDatePicker.MaximumDate = AddTermEndDatePicker.Date;
			if (AddTermEndDatePicker.Date > AddTermStartDatePicker.Date)
			{
				AddTermStartDatePicker.Date = AddTermEndDatePicker.Date;
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