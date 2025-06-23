namespace InfomedicsPortal.Core.Treatments;

public interface ITreatmentsService
{
    Task<Treatment[]> GetAllTreatmentsAsync();
}