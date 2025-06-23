using InfomedicsPortal.Core.Treatments;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class TreatmentsRepository : ITreatmentsStorage
{
    public Task<Treatment[]> GetAllTreatmentsAsync()
    {
        return Task.FromResult(
            new[] {
                new Treatment() { Id = Guid.NewGuid(), Name = "Teeth Whitening" },
                new Treatment() { Id = Guid.NewGuid(), Name = "Dental Crown" },
                new Treatment() { Id = Guid.NewGuid(), Name = "Root Canal Therapy" },
                new Treatment() { Id = Guid.NewGuid(), Name = "Orthodontic Braces" },
            }
        );
    }
}