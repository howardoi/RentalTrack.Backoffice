using RentalTrack.Business.Dtos;

namespace RentalTrack.Business.Interfaces;

public interface IUserService
{
    Task<UserSearchResultDto> SearchAsync(string? query, string? status, string? provider, int page, int pageSize);
    Task<UserDetailDto?> GetDetailAsync(long id);
    Task SuspendAsync(long id);
}
