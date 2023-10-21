namespace bmcdani_c971_task.Pages;

public partial class EditTermPage : ContentPage
{
	private int termId;

	public EditTermPage(int termId, string termName, DateTime startDate, DateTime endDate)
	{
		InitializeComponent();

		this.termId = termId;

		EditTermNameEntry.Placeholder = termName;
		EditTermStartDatePicker.Date = startDate;
		EditTermEndDatePicker.Date = endDate;
	}

	private async void OnSaveChanges_Clicked(object sender, EventArgs e)
	{
        if (!string.IsNullOrEmpty(EditTermNameEntry.Text))
        {
			await DataServices.UpdateTerm(termId, EditTermNameEntry.Text, EditTermStartDatePicker.Date, EditTermEndDatePicker.Date);
			await Navigation.PopAsync();
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid term name.", "Ok");
		}
    }
}