using System.Collections.Concurrent;
using InfomedicsPortal.Core.Treatments;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class TreatmentsRepository : BaseRepository<Treatment, Guid>, ITreatmentsStorage
{
    private static readonly ConcurrentDictionary<Guid, Treatment> Treatments = new(
    new[]
    {
        new Treatment { Id = Guid.NewGuid(), Name = "Teeth Whitening" },
        new Treatment { Id = Guid.NewGuid(), Name = "Dental Crown" },
        new Treatment { Id = Guid.NewGuid(), Name = "Root Canal Therapy" },
        new Treatment { Id = Guid.NewGuid(), Name = "Orthodontic Braces" }
    }.ToDictionary(t => t.Id));
    
    public TreatmentsRepository() : base(
        it => it.Id, Treatments)
    {
    }

    public async Task<Treatment[]> GetAllTreatmentsAsync()
    {
        var getAllResult = await GetAllAsync();
        return getAllResult.ToArray();
    }
}