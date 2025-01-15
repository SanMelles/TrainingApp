using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingApp.Models
{
    public class TrainingSessionExercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TrainingSessionId { get; set; } // Foreign key
        [ForeignKey(nameof(TrainingSession))]
        public TrainingSession TrainingSession { get; set; }
        public string ExerciseName { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Weight { get; set; }
    }
}
