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
            var newExercise = new TrainingSessionExercise
            {
                TrainingSessionId = _sessionId, // Ensure this links to the correct session
                ExerciseName = ExerciseNameEntry.Text,
                Sets = int.Parse(SetsEntry.Text),
                Reps = int.Parse(RepsEntry.Text),
                Weight = double.Parse(WeightEntry.Text)
            };

            await _database.SaveExerciseAsync(newExercise);
            await Navigation.PopAsync(); // Navigate back to the previous page
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during saving
            await DisplayAlert("Error", $"Error saving the session: {ex.Message}", "OK");
        }
    }
}
