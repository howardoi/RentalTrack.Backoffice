using RentalTrack.Business.Dtos;

namespace RentalTrack.Business.Interfaces;

public interface IPropertyService
{
    Task<PropertySearchResultDto> SearchAsync(string? query, string? status, int page, int pageSize);
    Task<PropertyDetailDto?> GetDetailAsync(long id);
}
