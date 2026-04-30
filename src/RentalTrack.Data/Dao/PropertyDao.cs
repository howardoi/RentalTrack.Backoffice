using Dapper;
using RentalTrack.Data.Connection;

namespace RentalTrack.Data.Dao;

public class PropertyDao
{
    private readonly ISqlConnectionFactory _factory;

    public PropertyDao(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<(IEnumerable<PropertyListRow> Items, int Total)> SearchAsync(
        string? query, string? status, int page, int pageSize)
    {
        using var conn = _factory.Create();
        var skip = (page - 1) * pageSize;

        const string baseSql = @"
FROM dbo.Property p
INNER JOIN dbo.[User] u ON u.Id = p.UserId AND u.IsDeleted = 0
WHERE p.IsDeleted = 0
  AND (@Query  IS NULL OR p.Name LIKE @QueryLike OR p.Address LIKE @QueryLike OR u.Email LIKE @QueryLike)
  AND (@Status IS NULL OR (@Status = 'tenanted' AND p.TenantName IS NOT NULL) OR (@Status = 'vacant' AND p.TenantName IS NULL))";

        var sql = $@"
SELECT
    p.Id, p.Name, p.Address, p.MonthlyRentalAmount, p.TenantName, p.CreatedAt,
    u.Email AS OwnerEmail, u.DisplayName AS OwnerDisplayName
{baseSql}
ORDER BY p.CreatedAt DESC
OFFSET @Skip ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT(*) {baseSql};";

        var parameters = new
        {
            Query     = query,
            QueryLike = string.IsNullOrWhiteSpace(query) ? null : "%" + query + "%",
            Status    = string.IsNullOrWhiteSpace(status) ? null : status,
            Skip      = skip,
            PageSize  = pageSize,
        };

        using var multi = await conn.QueryMultipleAsync(sql, parameters);
        var items = await multi.ReadAsync<PropertyListRow>();
        var total = await multi.ReadSingleAsync<int>();
        return (items, total);
    }

    public async Task<PropertyWithOwner?> GetWithOwnerAsync(long id)
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<PropertyWithOwner>(
            @"SELECT
                  p.Id, p.Name, p.Address, p.MonthlyRentalAmount,
                  p.TenantName, p.TenancyStartDate, p.CukaiTaksiranDueDate, p.CreatedAt,
                  u.Email AS OwnerEmail, u.DisplayName AS OwnerDisplayName
              FROM dbo.Property p
              INNER JOIN dbo.[User] u ON u.Id = p.UserId
              WHERE p.Id = @Id AND p.IsDeleted = 0",
            new { Id = id });
    }
}
