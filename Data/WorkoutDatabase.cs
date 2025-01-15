using SQLite;
using TrainingApp.Models;

public class WorkoutDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public WorkoutDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<TrainingSession>().Wait();
        _database.CreateTableAsync<TrainingSessionExercise>().Wait();
    }

    public Task<List<TrainingSession>> GetTrainingSessionsAsync()
        => _database.Table<TrainingSession>().ToListAsync();

    public Task<List<TrainingSessionExercise>> GetExercisesAsync(int sessionId)
        => _database.Table<TrainingSessionExercise>()
                    .Where(e => e.TrainingSessionId == sessionId).ToListAsync();

    public Task<int> SaveTrainingSessionAsync(TrainingSession session)
        => _database.InsertAsync(session);

    public Task<int> SaveExerciseAsync(TrainingSessionExercise exercise)
        => _database.InsertAsync(exercise);

    public Task<int> UpdateTrainingSessionAsync(TrainingSession session)
        => _database.UpdateAsync(session);

    public Task<int> DeleteTrainingSessionAsync(TrainingSession session)
        => _database.DeleteAsync(session);

    public Task<int> DeleteExerciseAsync(TrainingSessionExercise exercise)
        => _database.DeleteAsync(exercise);
}
