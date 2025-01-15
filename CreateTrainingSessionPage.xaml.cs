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

        while (!validInput)
        {
            string name = await DisplayPromptAsync("Exercise", "Enter the name of the exercise");

            if (string.IsNullOrEmpty(name)) // If user cancels or inputs empty name
            {
                return;
            }

            if (int.TryParse(await DisplayPromptAsync("Reps", "Enter reps:"), out int reps) &&
                int.TryParse(await DisplayPromptAsync("Sets", "Enter sets:"), out int sets) &&
                double.TryParse(await DisplayPromptAsync("Weight", "Enter weight:"), out double weight))
            {
                validInput = true;

                var exercise = new TrainingSessionExercise
                {
                    TrainingSessionId = _session.Id,
                    ExerciseName = name,
                    Reps = reps,
                    Sets = sets,
                    Weight = weight
                };

                await App.Database.SaveExerciseAsync(exercise);
                LoadExercises();
            }
            else
            {
                await DisplayAlert("Error", "Invalid input for reps, sets, or weight. Please try again.", "OK");
            }
        }
    }


    private async void OnSaveSession(object sender, EventArgs e)
    {
        await App.Database.SaveTrainingSessionAsync(_session);
        await Navigation.PopAsync();
    }
}