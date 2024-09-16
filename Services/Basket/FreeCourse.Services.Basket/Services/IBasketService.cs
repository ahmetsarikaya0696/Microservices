using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<ResponseDTO<BasketDTO>> GetBasketAsync(string userId);
        Task<ResponseDTO<bool>> SaveOrUpdateAsync(BasketDTO basketDTO);
        Task<ResponseDTO<bool>> DeleteAsync(string userId);
    }
}
