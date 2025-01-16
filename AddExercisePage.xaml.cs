using TrainingApp.Data;
using TrainingApp.Models;
using Microsoft.Maui.Controls;

namespace TrainingApp;

public partial class AddExercisePage : ContentPage
{
    private readonly WorkoutDatabase _database;
    private int _sessionId;

    public AddExercisePage(WorkoutDatabase database, int sessionId)
    {
        InitializeComponent();
        _database = database;
        _sessionId = sessionId;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var newSession = new TrainingSession
        {
            Name = SessionNameEntry.Text,
            Date = SessionDatePicker.Date
        };

        var exercises = new List<TrainingSessionExercise>
        {
            new TrainingSessionExercise
            {
                TrainingSessionId = _sessionId,
                ExerciseName = ExerciseNameEntry.Text,
                Sets = int.Parse(SetsEntry.Text),
                Reps = int.Parse(RepsEntry.Text),
                Weight = double.Parse(WeightEntry.Text)
            }
        };

        await _database.SaveTrainingSessionAsync(newSession, exercises);
        await Navigation.PushAsync(new TrainingSessionDetailPage(_database, newSession));
    }
}
