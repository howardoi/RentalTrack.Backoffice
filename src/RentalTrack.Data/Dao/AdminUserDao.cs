using Dapper;
using RentalTrack.Data.Connection;
using RentalTrack.Data.Entities;

namespace RentalTrack.Data.Dao;

public class AdminUserDao
{
    private readonly ISqlConnectionFactory _factory;

    public AdminUserDao(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<AdminUser?> GetByEmailAsync(string email)
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<AdminUser>(
            "SELECT * FROM dbo.AdminUser WHERE Email = @Email",
            new { Email = email });
    }

    public async Task<AdminUser?> GetByIdAsync(long id)
    {
        using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<AdminUser>(
            "SELECT * FROM dbo.AdminUser WHERE Id = @Id",
            new { Id = id });
    }

    public async Task UpdateLoginSuccessAsync(long id)
    {
        using var conn = _factory.Create();
        await conn.ExecuteAsync(
            @"UPDATE dbo.AdminUser
              SET FailedLoginAttempts = 0,
                  LockoutEndAt        = NULL,
                  LastLoginAt         = SYSUTCDATETIME(),
                  UpdatedAt           = SYSUTCDATETIME()
              WHERE Id = @Id",
            new { Id = id });
    }

    public async Task UpdateLoginFailureAsync(long id, int failedAttempts, DateTime? lockoutEndAt)
    {
        using var conn = _factory.Create();
        await conn.ExecuteAsync(
            @"UPDATE dbo.AdminUser
              SET FailedLoginAttempts = @FailedLoginAttempts,
                  LockoutEndAt        = @LockoutEndAt,
                  UpdatedAt           = SYSUTCDATETIME()
              WHERE Id = @Id",
            new { Id = id, FailedLoginAttempts = failedAttempts, LockoutEndAt = lockoutEndAt });
    }
}
