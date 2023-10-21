﻿using Android.Gms.Common.Apis;
using bmcdani_c971_task.Pages;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Graphics.Converters;
using SQLite;
using System.Drawing;
using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;
using Color = Microsoft.Maui.Graphics.Color;

namespace bmcdani_c971_task
{
    public partial class MainPage : ContentPage
    {
        private bool buttonClicked = false;
        private bool isTermDeleteMode = false;

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
                    Text = term.TermName,
                    FontSize = 18,
                    Margin = new Thickness(0, 5),
                    WidthRequest = 200,
                    CommandParameter = term.TermId
                };
                termButton.Clicked += TermButton_Clicked;
                termsStackLayout.Children.Add(termButton);
            }
        }

        private async void DeleteTermBtn_Clicked(object sender, EventArgs e)
        {
            isTermDeleteMode = !isTermDeleteMode;

            if (isTermDeleteMode)
            {
                DeleteTermBtn.BackgroundColor = Colors.Gray;
            }
            else
            {
                DeleteTermBtn.BackgroundColor = Colors.Crimson;
            }
        }

        private async void TermButton_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (isTermDeleteMode)
            {
                bool isConfirmed = await DisplayAlert("Delete Term", "Are you sure you want to delete this term?", "Yes", "No");
                if (isConfirmed)
                {
                    int selectedTermId = (int)clickedButton.CommandParameter;
                    await DataServices.RemoveTerm(selectedTermId);

                    await UpdateTermsAsync();
                }
            }
            else
            {
                if (buttonClicked) return;
                buttonClicked = true;

                int selectedTermId = (int)clickedButton.CommandParameter;
                Term selectedTerm = (await DataServices.GetTerms()).FirstOrDefault(t => t.TermId == selectedTermId);
                if (selectedTerm != null)
                {
                    await Navigation.PushAsync(new CourseView(selectedTermId, selectedTerm.TermName, selectedTerm.TermStartDate, selectedTerm.TermEndDate));
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            buttonClicked = false;

            UpdateTermsAsync();
        }

        private async void AddTermBtn_Clicked(object sender, EventArgs e)
        {
            if (buttonClicked) return;
            buttonClicked = true;

            await Navigation.PushAsync(new AddTermPage());
            
        }
    }
}