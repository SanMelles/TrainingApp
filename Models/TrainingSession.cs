using SQLite;


namespace TrainingApp.Models
{
    public class TrainingSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TrainingSession()
        {
            Exercises = new List<TrainingSessionExercise>();
        }

        public List<TrainingSessionExercise> Exercises { get; set; }
    }
}
