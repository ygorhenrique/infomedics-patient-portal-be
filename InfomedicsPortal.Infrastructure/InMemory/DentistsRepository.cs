using System.Collections.Concurrent;
using InfomedicsPortal.Core.Dentists;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class DentistsRepository : BaseRepository<Dentist, Guid>, IDentistsStorage
{
    private static readonly ConcurrentDictionary<Guid, Dentist> Dentists = new(
        new[]
        {
            new Dentist { Id = Guid.NewGuid(), Name = "Bruce Wayne" },
            new Dentist { Id = Guid.NewGuid(), Name = "Selina Kyle" },
            new Dentist { Id = Guid.NewGuid(), Name = "Jack Oswald White" }
        }.ToDictionary(d => d.Id));

    public DentistsRepository() : base(it => it.Id, Dentists)
    {
    }

    public async Task<Dentist[]> GetAllDentistsAsync()
    {
        var getAllResult = await GetAllAsync();
        return getAllResult.ToArray();
    }
}