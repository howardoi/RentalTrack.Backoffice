using Dapper;
using RentalTrack.Data.Connection;

namespace RentalTrack.Data.Dao;

public class DashboardDao
{
    private readonly ISqlConnectionFactory _factory;

    public DashboardDao(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<DashboardStatsRow> GetStatsAsync()
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleAsync<DashboardStatsRow>(
            @"SELECT
                (SELECT COUNT(*) FROM dbo.[User]   WHERE IsDeleted = 0)                                          AS TotalUsers,
                (SELECT COUNT(*) FROM dbo.Property WHERE IsDeleted = 0)                                          AS ActiveProperties,
                (SELECT ISNULL(SUM(MonthlyRentalAmount), 0) FROM dbo.Property WHERE IsDeleted = 0)               AS TrackedMonthlyRent,
                (SELECT COUNT(*) FROM dbo.RentalPayment p
                  INNER JOIN dbo.Property pr ON pr.Id = p.PropertyId AND pr.IsDeleted = 0
                  WHERE p.IsPaid = 0
                    AND DATEFROMPARTS(p.[Year], p.[Month], 1) < CAST(GETDATE() AS DATE))                         AS OverdueRentRows;");
    }

    public async Task<IEnumerable<RecentSignUpRow>> GetRecentSignUpsAsync(int take)
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<RecentSignUpRow>(
            @"SELECT TOP (@Take)
                  u.Id, u.Email, u.DisplayName, u.CreatedAt,
                  CASE el.Provider WHEN 1 THEN 'Google' WHEN 2 THEN 'Apple' ELSE NULL END AS Provider
              FROM dbo.[User] u
              OUTER APPLY (
                  SELECT TOP 1 Provider
                  FROM dbo.ExternalLogin
                  WHERE UserId = u.Id
                  ORDER BY Id DESC
              ) el
              WHERE u.IsDeleted = 0
              ORDER BY u.CreatedAt DESC",
            new { Take = take });
    }
}
