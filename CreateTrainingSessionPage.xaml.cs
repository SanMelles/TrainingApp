using TrainingApp.Models;
using TrainingApp.Data;

namespace TrainingApp;

public partial class CreateTrainingSessionPage : ContentPage
{
	private TrainingSession _session;
    public CreateTrainingSessionPage(TrainingSession session)
	{
		InitializeComponent();
        _session = session ?? new TrainingSession { Date = DateTime.Now, Exercises = new List<TrainingSessionExercise>() };
        BindingContext = _session;
        ExercisesList.ItemsSource = _session.Exercises;

        if (session != null)
        {
            SessionNameEntry.Text = session.Name;
            DatePicker.Date = session.Date;
        }
    }

    private async void LoadExercises()
    {
        ExercisesList.ItemsSource = await App.Database.GetExercisesAsync(_session.Id);
    }

    private async void OnAddExercise(object sender, EventArgs e)
    {
        string exerciseName = await DisplayPromptAsync("Exercise Name", "Enter Exercise Name");

        if (!string.IsNullOrEmpty(exerciseName))
        {
            string repsStr = await DisplayPromptAsync("Reps", "Enter Reps");
            if (int.TryParse(repsStr, out int reps))
            {
                string setsStr = await DisplayPromptAsync("Sets", "Enter Sets");
                if (int.TryParse(setsStr, out int sets))
                {
                    string weightStr = await DisplayPromptAsync("Weight", "Enter Weight");
                    if (double.TryParse(weightStr, out double weight))
                    {
                        _session.Exercises.Add(new TrainingSessionExercise
                        {
                            ExerciseName = exerciseName,
                            Reps = reps,
                            Sets = sets,
                            Weight = weight
                        });
                        ExercisesList.ItemsSource = _session.Exercises;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid weight input.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Invalid sets input.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid reps input.", "OK");
            }
        }
    }


    private async void OnSaveSession(object sender, EventArgs e)
    {
        _session.Name = SessionNameEntry.Text;
        _session.Date = DatePicker.Date;

        if (_session.Id == 0)
        {
            await App.Database.SaveTrainingSessionAsync(_session);
        }
        else
        {
            await App.Database.UpdateTrainingSessionAsync(_session);
        }

        await Navigation.PopAsync();
    }
}