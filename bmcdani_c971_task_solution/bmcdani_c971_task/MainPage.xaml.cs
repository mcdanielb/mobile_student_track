using Android.Gms.Common.Apis;
using bmcdani_c971_task.Pages;
using SQLite;

namespace bmcdani_c971_task
{
    public partial class MainPage : ContentPage
    {
        private List<Term> terms;
        private bool buttonClicked = false;

        public MainPage()
        {
            InitializeComponent();

            UpdateTermsAsync();
        }

        private async Task UpdateTermsAsync()
        {
            var terms = await DataServices.GetTerms();

            termsStackLayout.Children.Clear();

            foreach (var term in terms)
            {
                Button termButton = new Button
                {
                    Text = term.Name,
                    FontSize = 18,
                    Margin = new Thickness(0, 5),
                    WidthRequest = 200,
                    CommandParameter = term.Id
                };
                termButton.Clicked += TermButton_Clicked;
                termsStackLayout.Children.Add(termButton);
            }
        }

        private async void TermButton_Clicked(object sender, EventArgs e)
        {
            if (buttonClicked) return;

            buttonClicked = true;

            Button clickedButton = (Button)sender;
            int selectedTermId = (int)clickedButton.CommandParameter;

            Term selectedTerm = (await DataServices.GetTerms()).FirstOrDefault(t => t.Id == selectedTermId);
            if (selectedTerm != null)
            {
                await Navigation.PushAsync(new CourseView(selectedTermId, selectedTerm.Name));
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            buttonClicked = false;
        }

        private async void AddTermBtn_Clicked(object sender, EventArgs e)
        {
            var name = await App.Current.MainPage.DisplayPromptAsync("Name", "Term name");
            await DataServices.AddTerm(name);

            await UpdateTermsAsync();
        }
    }
}