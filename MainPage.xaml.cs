using TrainingApp.Data;
using TrainingApp.Models;

namespace TrainingApp
{
    public partial class MainPage : ContentPage
    {
        private readonly WorkoutDatabase _database;
        public MainPage()
        {
            InitializeComponent();
            _database = App.Database;
            LoadTrainingSessions();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            LoadTrainingSessions();
        }

        private async void LoadTrainingSessions()
        {
            var sessions = await _database.GetTrainingSessionsAsync();

            var sortedSessions = sessions.OrderByDescending(session => session.Date).ToList();

            TrainingSessionsListView.ItemsSource = sortedSessions;
        }

        private async void OnTrainingSessionTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is TrainingSession session)
            {
                await Navigation.PushAsync(new TrainingSessionDetailPage(_database, session));
            }
        }

        private async void OnCreateNewSessionClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTrainingSessionPage());
        }
    }

}

