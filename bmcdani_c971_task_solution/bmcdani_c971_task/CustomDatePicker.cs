namespace bmcdani_c971_task;

public class CustomDatePicker : DatePicker
{
    public CustomDatePicker()
    {
        this.MinimumDate = new DateTime(2018, 1, 1);
        this.MaximumDate = new DateTime(2030, 12, 31);
        this.Date = new DateTime(2023, 10, 18);

        this.DateSelected += OnDateSelected;
    }

    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        if (this.Date > this.MaximumDate)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Start date cannot be greater than end date.", "Ok");
            this.Date = e.OldDate;
        }
        else if (this.Date < this.MinimumDate)
        {
            await App.Current.MainPage.DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
            this.Date = e.OldDate;
        }
    }
}