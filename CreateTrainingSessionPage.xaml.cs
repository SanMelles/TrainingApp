using TrainingApp.Models;

namespace TrainingApp;

public partial class CreateTrainingSessionPage : ContentPage
{
	private TrainingSession _session;
    public CreateTrainingSessionPage(TrainingSession session)
	{
		InitializeComponent();
        _session = session ?? new TrainingSession { Date = DateTime.Now };
        BindingContext = _session;
        if (session != null)
            LoadExercises();
    }

    private async void LoadExercises()
    {
        ExercisesList.ItemsSource = await App.Database.GetExercisesAsync(_session.Id);
    }

    private async void OnAddExercise(object sender, EventArgs e)
    {
        bool validInput = false;
        int reps = 0;
        int sets = 0;
        double weight = 0;
        string name = string.Empty; // Initialize name with an empty string

        while (!validInput)
        {
            name = await DisplayPromptAsync("Exercise", "Enter the name of the exercise");

            if (int.TryParse(await DisplayPromptAsync("Reps", "Enter reps:"), out reps) &&
                int.TryParse(await DisplayPromptAsync("Sets", "Enter sets:"), out sets) &&
                double.TryParse(await DisplayPromptAsync("Weight", "Enter weight:"), out weight))
            {
                validInput = true;
            }
            else
            {
                await DisplayAlert("Error", "Invalid input for reps, sets, or weight. Please try again.", "OK");
            }
        }

        var exercise = new TrainingSessionExercise
        {
            TrainingSessionId = _session.Id,
            ExerciseName = name, // Use the declared name variable
            Reps = reps,
            Sets = sets,
            Weight = weight
        };

        await App.Database.SaveExerciseAsync(exercise);
        LoadExercises();
    }

    private async void OnSaveSession(object sender, EventArgs e)
    {
        await App.Database.SaveTrainingSessionAsync(_session);
        await Navigation.PopAsync();
    }
}