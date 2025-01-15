using SQLite;

namespace TrainingApp.Models
{
    public class TrainingSessionExercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TrainingSessionId { get; set; } // Foreign key
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Weight { get; set; }
    }
}
