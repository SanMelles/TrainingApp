using TrainingApp.Data;
using TrainingApp.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace TrainingApp;

public partial class AddExercisePage : ContentPage
{
    private readonly WorkoutDatabase _database;
    private readonly int _sessionId;

    public AddExercisePage(WorkoutDatabase database, int sessionId)
    {
        InitializeComponent();
        _database = database;
        _sessionId = sessionId;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            // Save the session details first
            var newSession = new TrainingSession
            {
                Name = SessionNameEntry.Text,
                Date = SessionDatePicker.Date
            };

            // Save the session to the database and get its ID
            var savedSession = await _database.SaveTrainingSessionAsync(newSession);

            // Now associate the exercises with the saved session
            var exercises = new List<TrainingSessionExercise>
            {
                new TrainingSessionExercise
                {
                    ExerciseName = ExerciseNameEntry.Text,
                    Sets = int.Parse(SetsEntry.Text),
                    Reps = int.Parse(RepsEntry.Text),
                    Weight = double.Parse(WeightEntry.Text),
                    TrainingSessionId = savedSession.Id // Set the session ID for the exercise
                }
            };

            // Save the exercises to the database
            await _database.SaveExercisesAsync(exercises);

            // Navigate to the session details page
            await Navigation.PushAsync(new TrainingSessionDetailPage(_database, savedSession));
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during saving
            await DisplayAlert("Error", $"Error saving the session: {ex.Message}", "OK");
        }
    }
}
