using TrainingApp.Models;
using TrainingApp.Data;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace TrainingApp
{
    public partial class CreateTrainingSessionPage : ContentPage
    {
        private readonly WorkoutDatabase _database = App.Database;
        private readonly ObservableCollection<TrainingSessionExercise> _exercises = new ObservableCollection<TrainingSessionExercise>();

        private int? _currentSessionId;
        public CreateTrainingSessionPage()
        {
            InitializeComponent();
            BindingContext = this;
            ExercisesListView.ItemsSource = _exercises;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                string sessionName = SessionNameEntry.Text;
                DateTime sessionDate = SessionDatePicker.Date.Date;

                // Create a new training session
                var trainingSession = new TrainingSession
                {
                    Name = sessionName,
                    Date = sessionDate
                };

                // Save the session to the database and get its ID
                var savedSession = await _database.SaveTrainingSessionAsync(trainingSession);

                // Now associate the exercises with the saved session
                foreach (var exercise in _exercises)
                {
                    exercise.TrainingSessionId = savedSession.Id;  // Set the session ID for each exercise
                }

                // Save exercises to the database
                await _database.SaveExercisesAsync(_exercises);

                // Display success message and navigate back
                await DisplayAlert("Success", "Training session saved successfully!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                await DisplayAlert("Error", $"Error saving training session: {ex.Message}", "OK");
            }
        }

        private void OnAddExerciseClicked(object sender, EventArgs e)
        {
            try
            {
                string exerciseName = ExerciseNameEntry.Text;
                if (string.IsNullOrWhiteSpace(exerciseName)) throw new Exception("Exercise name cannot be empty.");

                int sets = int.Parse(SetsEntry.Text);
                int reps = int.Parse(RepsEntry.Text);
                double weight = double.Parse(WeightEntry.Text);

                // Create a new exercise object and add it to the list
                var exercise = new TrainingSessionExercise
                {
                    ExerciseName = exerciseName,
                    Sets = sets,
                    Reps = reps,
                    Weight = weight
                };

                // Add the exercise to the ObservableCollection (automatically updates UI)
                _exercises.Add(exercise);

                // Clear input fields for next entry
                ExerciseNameEntry.Text = string.Empty;
                SetsEntry.Text = string.Empty;
                RepsEntry.Text = string.Empty;
                WeightEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // Display error message for invalid input
                DisplayAlert("Error", $"Invalid input: {ex.Message}", "OK");
            }
        }

    }
}