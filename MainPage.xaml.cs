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
            SessionsList.ItemsSource = await App.Database.GetTrainingSessionsAsync();
        }

        private async void OnSessionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is TrainingSession session)
            {
                await Navigation.PushAsync(new CreateTrainingSessionPage(session));
            }
        }

        private async void OnCreateNewSession(object sender, EventArgs e)
        {
            TrainingSession newSession = new TrainingSession { Date = DateTime.Now };
            await Navigation.PushAsync(new CreateTrainingSessionPage(newSession));
        }
    }

}
