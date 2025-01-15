using SQLite;
using TrainingApp.Models;

namespace TrainingApp.Data;

public class WorkoutDatabase : SQLiteConnection
{
    private readonly SQLiteAsyncConnection _database;

    public WorkoutDatabase(string dbPath) : base(dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        InitializeDatabase().Wait();
    }

    private async Task InitializeDatabase()
    {
        await _database.CreateTableAsync<TrainingSession>();
        await _database.CreateTableAsync<TrainingSessionExercise>();
    }

    public Task<List<TrainingSession>> GetTrainingSessionsAsync()
        => _database.Table<TrainingSession>().ToListAsync();

    public Task<List<TrainingSessionExercise>> GetExercisesAsync(int sessionId)
        => _database.Table<TrainingSessionExercise>()
                   .Where(e => e.TrainingSessionId == sessionId).ToListAsync();

    public async Task<int> SaveTrainingSessionAsync(TrainingSession session)
    {
        int result = await _database.InsertOrReplaceAsync(session);

        return result;
    }

    public async Task<int> DeleteTrainingSessionAsync(TrainingSession session)
    {
        await _database.ExecuteAsync("DELETE FROM TrainingSessionExercise WHERE TrainingSessionId = ?", session.Id);
        return await _database.DeleteAsync(session);
    }

    public Task<int> SaveExerciseAsync(TrainingSessionExercise exercise)
        => _database.InsertOrReplaceAsync(exercise);

    public Task<int> UpdateExerciseAsync(TrainingSessionExercise exercise)
        => _database.UpdateAsync(exercise);

    public Task<int> DeleteExerciseAsync(TrainingSessionExercise exercise)
        => _database.DeleteAsync(exercise);

    public Task<int> UpdateTrainingSessionAsync(TrainingSession session)
        => _database.UpdateAsync(session);
}