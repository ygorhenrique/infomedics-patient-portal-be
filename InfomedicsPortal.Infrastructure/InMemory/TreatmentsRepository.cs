using System.Collections.Concurrent;
using InfomedicsPortal.Core.Treatments;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class TreatmentsRepository : BaseRepository<Treatment, Guid>, ITreatmentsStorage
{
    private static readonly ConcurrentDictionary<Guid, Treatment> Treatments = new(
    new[]
    {
        new Treatment { Id = Guid.NewGuid(), Name = "Teeth Whitening", DurationMins = 60 },
        new Treatment { Id = Guid.NewGuid(), Name = "Dental Crown", DurationMins = 90 },
        new Treatment { Id = Guid.NewGuid(), Name = "Root Canal Therapy", DurationMins = 90 },
        new Treatment { Id = Guid.NewGuid(), Name = "Orthodontic Braces", DurationMins = 90 }
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