using RentalTrack.Utility.Enums;

namespace RentalTrack.Business.Dtos;

public class AdminPrincipalDto
{
    public long Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AdminRole Role { get; set; }
}

public class AuthenticateResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public AdminPrincipalDto? Principal { get; set; }

    public static AuthenticateResult Ok(AdminPrincipalDto p) => new() { Success = true, Principal = p };
    public static AuthenticateResult Fail(string msg)        => new() { Success = false, ErrorMessage = msg };
}
