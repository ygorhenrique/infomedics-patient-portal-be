namespace InfomedicsPortal.Core.Treatments;

public interface ITreatmentsStorage
{
    public Task<Treatment[]> GetAllTreatmentsAsync();
}