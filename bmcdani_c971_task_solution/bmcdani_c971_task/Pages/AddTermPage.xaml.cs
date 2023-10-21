namespace bmcdani_c971_task.Pages;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();

		TermStartDatePicker.DateSelected += (s, e) =>
		{
			TermEndDatePicker.MinimumDate = TermStartDatePicker.Date;
			if (TermStartDatePicker.Date > TermEndDatePicker.Date)
			{
				TermEndDatePicker.Date = TermStartDatePicker.Date;
			}
		};

		TermEndDatePicker.DateSelected += (s, e) =>
		{
			TermStartDatePicker.MaximumDate = TermEndDatePicker.Date;
			if (TermEndDatePicker.Date > TermStartDatePicker.Date)
			{
				TermStartDatePicker.Date = TermEndDatePicker.Date;
			}
		};
	}

	private async void OnAddTerm_Clicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(TermNameEntry.Text))
		{
			await DataServices.AddTerm(TermNameEntry.Text, TermStartDatePicker.Date, TermEndDatePicker.Date);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid term name.", "OK");
		}
	}
}