using FreeCourse.Web.Models.Catalog.PhotoStock;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPhotoStockService
    {
        Task<PhotoStockVM> UploadPhoto(IFormFile file);
        Task<bool> DeletePhoto(string photoUrl);
    }
}
