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
        await Navigation.PushAsync(new AddExercisePage(_database, _session.Id));
    }
}
