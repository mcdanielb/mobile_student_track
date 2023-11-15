namespace bmcdani_academic_tracking;

public class CustomDatePicker : DatePicker
{
    public CustomDatePicker()
    {
        this.MinimumDate = new DateTime(2018, 1, 1);
        this.MaximumDate = new DateTime(2030, 12, 31);
        this.Date = DateTime.Now;

        this.DateSelected += OnDateSelected;
    }

    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        if (this.Date > this.MaximumDate)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Start date cannot be greater than end date.", "Ok");
            this.Date = e.OldDate;
        }
        else if (this.Date < this.MinimumDate)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "End date cannot be earlier than start date.", "Ok");
            this.Date = e.OldDate;
        }
    }
}