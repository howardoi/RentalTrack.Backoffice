using Dapper;
using RentalTrack.Data.Connection;
using RentalTrack.Data.Entities;

namespace RentalTrack.Data.Dao;

public class UserDao
{
    private readonly ISqlConnectionFactory _factory;

    public UserDao(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<(IEnumerable<UserListRow> Items, int Total)> SearchAsync(
        string? query, string? status, string? provider, int page, int pageSize)
    {
        using var conn = _factory.Create();
        var skip = (page - 1) * pageSize;

        const string baseSql = @"
FROM dbo.[User] u
OUTER APPLY (
    SELECT TOP 1 Provider
    FROM dbo.ExternalLogin
    WHERE UserId = u.Id
    ORDER BY Id DESC
) el
WHERE u.IsDeleted = 0
  AND (@Query    IS NULL OR u.Email LIKE @QueryLike OR u.DisplayName LIKE @QueryLike)
  AND (@Status   IS NULL OR (@Status   = 'active'    AND u.IsActive = 1) OR (@Status = 'suspended' AND u.IsActive = 0))
  AND (@Provider IS NULL OR (@Provider = 'google'    AND el.Provider = 1) OR (@Provider = 'apple' AND el.Provider = 2))";

        var sql = $@"
SELECT
    u.Id, u.Email, u.DisplayName, u.IsActive, u.CreatedAt, u.LastLoginAt,
    CASE el.Provider WHEN 1 THEN 'Google' WHEN 2 THEN 'Apple' ELSE NULL END AS Provider,
    (SELECT COUNT(*) FROM dbo.Property pr WHERE pr.UserId = u.Id AND pr.IsDeleted = 0) AS PropertyCount
{baseSql}
ORDER BY u.CreatedAt DESC
OFFSET @Skip ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT(*) {baseSql};";

        var parameters = new
        {
            Query     = query,
            QueryLike = string.IsNullOrWhiteSpace(query) ? null : "%" + query + "%",
            Status    = string.IsNullOrWhiteSpace(status) ? null : status,
            Provider  = string.IsNullOrWhiteSpace(provider) ? null : provider,
            Skip      = skip,
            PageSize  = pageSize,
        };

        using var multi = await conn.QueryMultipleAsync(sql, parameters);
        var items = await multi.ReadAsync<UserListRow>();
        var total = await multi.ReadSingleAsync<int>();
        return (items, total);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM dbo.[User] WHERE Id = @Id AND IsDeleted = 0",
            new { Id = id });
    }

    public async Task<string?> GetPrimaryProviderAsync(long userId)
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<string?>(
            @"SELECT TOP 1
                  CASE Provider WHEN 1 THEN 'Google' WHEN 2 THEN 'Apple' ELSE NULL END
              FROM dbo.ExternalLogin
              WHERE UserId = @UserId
              ORDER BY Id DESC",
            new { UserId = userId });
    }

    public async Task<IEnumerable<Property>> GetPropertiesAsync(long userId)
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<Property>(
            @"SELECT *
              FROM dbo.Property
              WHERE UserId = @UserId AND IsDeleted = 0
              ORDER BY Name",
            new { UserId = userId });
    }

    public async Task SuspendAsync(long id)
    {
        using var conn = _factory.Create();
        await conn.ExecuteAsync(
            "UPDATE dbo.[User] SET IsActive = 0, UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id",
            new { Id = id });
    }
}
