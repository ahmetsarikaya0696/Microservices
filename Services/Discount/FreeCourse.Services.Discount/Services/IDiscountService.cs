using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<ResponseDTO<List<Models.Discount>>> GetAll();
        Task<ResponseDTO<Models.Discount>> GetById(int id);
        Task<ResponseDTO<NoContentDTO>> Save(Models.Discount discount);
        Task<ResponseDTO<NoContentDTO>> Update(Models.Discount discount);
        Task<ResponseDTO<NoContentDTO>> Delete(int id);
        Task<ResponseDTO<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
