using RentalTrack.Business.Dtos;
using RentalTrack.Business.Interfaces;
using RentalTrack.Data.Dao;

namespace RentalTrack.Business.Services;

public class PropertyService : IPropertyService
{
    private readonly PropertyDao _propertyDao;

    public PropertyService(PropertyDao propertyDao) => _propertyDao = propertyDao;

    public async Task<PropertySearchResultDto> SearchAsync(string? query, string? status, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 200) pageSize = 25;

        var (items, total) = await _propertyDao.SearchAsync(query, status, page, pageSize);

        return new PropertySearchResultDto
        {
            Items = items.Select(p => new PropertyListItemDto
            {
                Id                  = p.Id,
                Name                = p.Name,
                Address             = p.Address,
                MonthlyRentalAmount = p.MonthlyRentalAmount,
                TenantName          = p.TenantName,
                OwnerEmail          = p.OwnerEmail,
                OwnerName           = p.OwnerDisplayName,
                CreatedAt           = p.CreatedAt,
            }).ToList(),
            Total    = total,
            Page     = page,
            PageSize = pageSize,
        };
    }

    public async Task<PropertyDetailDto?> GetDetailAsync(long id)
    {
        var p = await _propertyDao.GetWithOwnerAsync(id);
        if (p is null) return null;

        return new PropertyDetailDto
        {
            Id                   = p.Id,
            Name                 = p.Name,
            Address              = p.Address,
            MonthlyRentalAmount  = p.MonthlyRentalAmount,
            TenantName           = p.TenantName,
            TenancyStartDate     = p.TenancyStartDate,
            TenancyEndDate       = p.TenancyEndDate,
            CukaiTaksiranDueDate = p.CukaiTaksiranDueDate,
            CreatedAt            = p.CreatedAt,
            OwnerEmail           = p.OwnerEmail,
            OwnerName            = p.OwnerDisplayName,
        };
    }
}
