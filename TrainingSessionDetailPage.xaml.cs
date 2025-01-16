using TrainingApp.Data;
using TrainingApp.Models;

namespace TrainingApp;

public partial class TrainingSessionDetailPage : ContentPage
{
    private readonly TrainingSession _session;
    private readonly WorkoutDatabase _database;

    public TrainingSessionDetailPage(WorkoutDatabase database, TrainingSession session)
    {
        InitializeComponent();
        _database = database;
        _session = session;
        BindingContext = _session;
    }

    private async void LoadSessionDetails()
    {
        SessionNameLabel.Text = _session.Name;
        SessionDateLabel.Text = _session.Date.ToString("d MMM yyyy");

        var exercises = await App.Database.GetExercisesForSessionAsync(_session.Id);
        ExercisesListView.ItemsSource = exercises.Select(e => new
        {
            ExerciseName = e.ExerciseName,
            Detail = $"{e.Sets} sets of {e.Reps} reps @ {e.Weight} kg"
        });
    }

    private async void OnAddExerciseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddExercisePage(_database, _session.Id));
    }
}
