using TrainingApp.Models;
using TrainingApp.Data;

namespace TrainingApp
{
    public partial class CreateTrainingSessionPage : ContentPage
    {
        private readonly WorkoutDatabase _database = App.Database;
        public CreateTrainingSessionPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var trainingSession = new TrainingSession
            {
                Name = SessionNameEntry.Text,
                Date = SessionDatePicker.Date
            };

            var exercises = new List<TrainingSessionExercise>
            {
                new TrainingSessionExercise
                {
                    ExerciseName = ExerciseNameEntry.Text,
                    Sets = int.Parse(SetsEntry.Text),
                    Reps = int.Parse(RepsEntry.Text),
                    Weight = double.Parse(WeightEntry.Text)
                }
            };

            await _database.SaveTrainingSessionAsync(trainingSession, exercises);
        }
    }
}