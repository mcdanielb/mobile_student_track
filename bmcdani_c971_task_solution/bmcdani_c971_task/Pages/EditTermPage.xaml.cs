namespace bmcdani_c971_task.Pages;

public partial class EditTermPage : ContentPage
{
	private int termId;

	public EditTermPage(int termId, string termName, DateTime startDate, DateTime endDate)
	{
		InitializeComponent();

		this.termId = termId;

		EditTermNameEntry.Text = termName;
		EditTermStartDatePicker.Date = startDate;
		EditTermEndDatePicker.Date = endDate;

        EditTermStartDatePicker.DateSelected += async (s, e) =>
        {
            if (EditTermStartDatePicker.Date > EditTermEndDatePicker.Date)
            {
                await DisplayAlert("Error", "Start date cannot be later than end date.", "Ok");
                EditTermEndDatePicker.Date = EditTermStartDatePicker.Date;
            }
        };

        EditTermEndDatePicker.DateSelected += async (s, e) =>
        {
            if (EditTermEndDatePicker.Date < EditTermStartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
                EditTermEndDatePicker.Date = e.OldDate;
            }
        };
    }

	private async void OnSaveTermChanges_Clicked(object sender, EventArgs e)
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