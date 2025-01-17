using TrainingApp.Data;
using TrainingApp.Models;
using System.Linq;

namespace TrainingApp;

public partial class AllSessionsPage : ContentPage
{
    private readonly WorkoutDatabase _database;

    public AllSessionsPage(WorkoutDatabase database)
    {
        InitializeComponent();
        _database = database;
        LoadAllSessions();
    }

    // Load all sessions from the database
    private async void LoadAllSessions()
    {
        var allSessions = await _database.GetAllTrainingSessionsAsync();
        AllSessionsListView.ItemsSource = allSessions.OrderByDescending(session => session.Date).ToList();
    }

    // Event handler for tapping a session in the list
    private async void OnAllSessionTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var session = e.Item as TrainingSession;
            await Navigation.PushAsync(new TrainingSessionDetailPage(_database, session));
        }
    }
}
