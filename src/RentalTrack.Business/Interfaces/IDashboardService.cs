using RentalTrack.Business.Dtos;

namespace RentalTrack.Business.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(int recentTake = 10);
}
