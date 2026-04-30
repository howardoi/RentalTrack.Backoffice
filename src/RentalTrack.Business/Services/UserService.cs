using RentalTrack.Business.Dtos;
using RentalTrack.Business.Interfaces;
using RentalTrack.Data.Dao;

namespace RentalTrack.Business.Services;

public class UserService : IUserService
{
    private readonly UserDao _userDao;

    public UserService(UserDao userDao) => _userDao = userDao;

    public async Task<UserSearchResultDto> SearchAsync(string? query, string? status, string? provider, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 200) pageSize = 25;

        var (items, total) = await _userDao.SearchAsync(query, status, provider, page, pageSize);

        return new UserSearchResultDto
        {
            Items = items.Select(u => new UserListItemDto
            {
                Id            = u.Id,
                Email         = u.Email,
                DisplayName   = u.DisplayName,
                IsActive      = u.IsActive,
                Provider      = u.Provider,
                PropertyCount = u.PropertyCount,
                CreatedAt     = u.CreatedAt,
                LastLoginAt   = u.LastLoginAt,
            }).ToList(),
            Total    = total,
            Page     = page,
            PageSize = pageSize,
        };
    }

    public async Task<UserDetailDto?> GetDetailAsync(long id)
    {
        var user = await _userDao.GetByIdAsync(id);
        if (user is null) return null;

        var provider   = await _userDao.GetPrimaryProviderAsync(id);
        var properties = await _userDao.GetPropertiesAsync(id);

        return new UserDetailDto
        {
            Id          = user.Id,
            Email       = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl   = user.AvatarUrl,
            IsActive    = user.IsActive,
            Provider    = provider,
            CreatedAt   = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            Properties  = properties.Select(p => new UserPropertyDto
            {
                Id                  = p.Id,
                Name                = p.Name,
                Address             = p.Address,
                MonthlyRentalAmount = p.MonthlyRentalAmount,
                TenantName          = p.TenantName,
            }).ToList(),
        };
    }

    public Task SuspendAsync(long id) => _userDao.SuspendAsync(id);
}
