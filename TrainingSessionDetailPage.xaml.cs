using System.Collections.ObjectModel;
using TrainingApp.Data;
using TrainingApp.Models;

namespace TrainingApp;

public partial class TrainingSessionDetailPage : ContentPage
{
    private readonly WorkoutDatabase _database;
    private readonly TrainingSession _session;

    public ObservableCollection<TrainingSessionExercise> Exercises { get; set; } = new ObservableCollection<TrainingSessionExercise>();

    public TrainingSessionDetailPage(WorkoutDatabase database, TrainingSession session)
    {
        InitializeComponent();
        _database = database;
        _session = session;
        BindingContext = this;
        LoadSessionDetails();
    }

    private async void LoadSessionDetails()
    {
        Title = _session.Name;
        SessionNameLabel.Text = _session.Name;
        SessionDateLabel.Text = _session.Date.ToShortDateString();

        var exercises = await _database.GetExercisesForSessionAsync(_session.Id);
        foreach (var exercise in exercises)
        {
            Exercises.Add(exercise);
        }
    }

    private async void OnAddExerciseClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int exerciseId)
        {
            var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this exercise?", "Yes", "No");
            if (confirm)
            {
                await _database.DeleteExercisesBySessionIdAsync(exerciseId);
                var exerciseToRemove = Exercises.FirstOrDefault(exercise => exercise.Id == exerciseId);
                if (exerciseToRemove != null)
                {
                    Exercises.Remove(exerciseToRemove);
                }
            }
        }
    }

    private async void OnDeleteSessionClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this session?", "Yes", "No");
        if (confirm)
        {
            await _database.DeleteExercisesBySessionIdAsync(_session.Id);
            await DisplayAlert("Deleted", "Training session deleted successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}
