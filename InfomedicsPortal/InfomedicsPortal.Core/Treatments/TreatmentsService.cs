namespace InfomedicsPortal.Core.Treatments;

public class TreatmentsService : ITreatmentsService
{
    private readonly ITreatmentsStorage _storage;

    public TreatmentsService(ITreatmentsStorage storage)
    {
        this._storage = storage;
    }

    public async Task<Treatment[]> GetAllTreatments()
    {
        var getAllResult = await _storage.GetAllTreatmentsAsync();
        return getAllResult;
    }
}