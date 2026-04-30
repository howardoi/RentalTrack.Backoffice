using RentalTrack.Business.Dtos;

namespace RentalTrack.Business.Interfaces;

public interface IAuthService
{
    Task<AuthenticateResult> AuthenticateAsync(string email, string password);
}
