using RentalTrack.Business.Dtos;
using RentalTrack.Business.Interfaces;
using RentalTrack.Data.Dao;

namespace RentalTrack.Business.Services;

public class DashboardService : IDashboardService
{
    private readonly DashboardDao _dao;

    public DashboardService(DashboardDao dao) => _dao = dao;

    public async Task<DashboardDto> GetDashboardAsync(int recentTake = 10)
    {
        var stats  = await _dao.GetStatsAsync();
        var recent = await _dao.GetRecentSignUpsAsync(recentTake);

        return new DashboardDto
        {
            Stats = new DashboardStatsDto
            {
                TotalUsers         = stats.TotalUsers,
                ActiveProperties   = stats.ActiveProperties,
                TrackedMonthlyRent = stats.TrackedMonthlyRent,
                OverdueRentRows    = stats.OverdueRentRows,
            },
            RecentSignUps = recent.Select(r => new RecentSignUpDto
            {
                Id          = r.Id,
                Email       = r.Email,
                DisplayName = r.DisplayName,
                CreatedAt   = r.CreatedAt,
                Provider    = r.Provider,
            }).ToList(),
        };
    }
}
