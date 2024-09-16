using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<ResponseDTO<BasketDTO>> GetBasketAsync(string userId)
        {
            var basket = await _redisService.GetDatabase().StringGetAsync(userId);

            if (string.IsNullOrEmpty(basket)) return ResponseDTO<BasketDTO>.Fail("Basket not found!", 404);

            return ResponseDTO<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(basket), 200);
        }

        public async Task<ResponseDTO<bool>> SaveOrUpdateAsync(BasketDTO basketDTO)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basketDTO.UserId, JsonSerializer.Serialize(basketDTO));

            if (!status) return ResponseDTO<bool>.Fail("Basket could not be updated or saved!", 500);

            return ResponseDTO<bool>.Success(204);
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(string userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);

            if (!status) return ResponseDTO<bool>.Fail("Basket could not be deleted!", 500);

            return ResponseDTO<bool>.Success(204);
        }
    }
}
