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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        LoadSessionDetails();
    }

    private async void LoadSessionDetails()
    {
        Title = _session.Name;

        // Assuming SessionNameLabel and SessionDateLabel are defined in XAML
        SessionNameLabel.Text = _session.Name;
        SessionDateLabel.Text = _session.Date.ToShortDateString();

        // Load exercises for this session
        var exercises = await _database.GetExercisesForSessionAsync(_session.Id);
        Exercises.Clear();
        foreach (var exercise in exercises)
        {
            Exercises.Add(exercise);
        }
    }

    private async void OnAddExerciseClicked(object sender, EventArgs e)
    {
        // Navigate to AddExercisePage to add a new exercise to this session
        await Navigation.PushAsync(new AddExercisePage(_database, _session.Id));
    }

    private async void OnDeleteExerciseClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int exerciseId)
        {
            var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this exercise?", "Yes", "No");
            if (confirm)
            {
                // Delete exercise from the database
                await _database.DeleteExerciseByIdAsync(exerciseId);

                // Remove the exercise from the ObservableCollection
                var exerciseToRemove = Exercises.FirstOrDefault(exercise => exercise.Id == exerciseId);
                if (exerciseToRemove != null)
                {
                    Exercises.Remove(exerciseToRemove);
                }

                await DisplayAlert("Deleted", "Exercise deleted successfully.", "OK");
            }
        }
    }

    private async void OnDeleteSessionClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this session?", "Yes", "No");
        if (confirm)
        {
            // Delete all exercises for this session
            await _database.DeleteExercisesBySessionIdAsync(_session.Id);

            // Delete the session itself
            await _database.DeleteSessionByIdAsync(_session.Id);

            await DisplayAlert("Deleted", "Training session deleted successfully.", "OK");
            await Navigation.PopAsync();
        }
    }
}
