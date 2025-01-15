using System.Linq;
using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadSessions();
        }

        private async void LoadSessions()
        {
            try
            {
                List<TrainingSession> allSessions = await App.Database.GetTrainingSessionsAsync();

                if (allSessions != null && allSessions.Any())
                {
                    List<TrainingSession> latestSessions = allSessions
                        .OrderByDescending(s => s.Date)
                        .Take(5)
                        .ToList();

                    SessionsList.ItemsSource = latestSessions;
                }
                else
                {
                    SessionsList.ItemsSource = new List<TrainingSession>();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load sessions: {ex.Message}", "OK");
            }
        }

        private async void OnSessionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TrainingSession selectedSession)
            {
                await Navigation.PushAsync(new CreateTrainingSessionPage(selectedSession));

                SessionsList.SelectedItem = null;
            }
        }

        private async void OnCreateNewSession(object sender, EventArgs e)
        {
            TrainingSession newSession = new TrainingSession { Date = DateTime.Now };

            await Navigation.PushAsync(new CreateTrainingSessionPage(newSession));
        }
    }
}
