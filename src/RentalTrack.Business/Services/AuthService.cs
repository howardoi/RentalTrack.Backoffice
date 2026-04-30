using RentalTrack.Business.Dtos;
using RentalTrack.Business.Interfaces;
using RentalTrack.Data.Dao;
using RentalTrack.Utility.Security;

namespace RentalTrack.Business.Services;

public class AuthService : IAuthService
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly AdminUserDao _adminUserDao;
    private readonly IPasswordHasher _hasher;

    public AuthService(AdminUserDao adminUserDao, IPasswordHasher hasher)
    {
        _adminUserDao = adminUserDao;
        _hasher = hasher;
    }

    public async Task<AuthenticateResult> AuthenticateAsync(string email, string password)
    {
        var user = await _adminUserDao.GetByEmailAsync(email);
        if (user is null || !user.IsActive)
            return AuthenticateResult.Fail("Email or password is incorrect.");

        if (user.LockoutEndAt.HasValue && user.LockoutEndAt.Value > DateTime.UtcNow)
            return AuthenticateResult.Fail("Account is temporarily locked. Please try again later.");

        if (!_hasher.Verify(password, user.PasswordHash))
        {
            var failed = user.FailedLoginAttempts + 1;
            DateTime? lockoutEnd = failed >= MaxFailedAttempts
                ? DateTime.UtcNow.Add(LockoutDuration)
                : null;
            await _adminUserDao.UpdateLoginFailureAsync(user.Id, failed, lockoutEnd);
            return AuthenticateResult.Fail("Email or password is incorrect.");
        }

        await _adminUserDao.UpdateLoginSuccessAsync(user.Id);

        return AuthenticateResult.Ok(new AdminPrincipalDto
        {
            Id          = user.Id,
            Email       = user.Email,
            DisplayName = user.DisplayName,
            Role        = user.Role,
        });
    }
}
