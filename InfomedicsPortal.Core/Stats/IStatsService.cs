namespace InfomedicsPortal.Core.Stats;

public interface IStatsService
{
    Task<StatsService.Stats> GetStatsAsync();
}