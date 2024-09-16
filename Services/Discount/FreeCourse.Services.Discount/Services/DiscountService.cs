using Dapper;
using FreeCourse.Shared.DTOs;
using Npgsql;
using System.Data;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSQL"));
        }

        public async Task<ResponseDTO<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("select * from discount");
            return ResponseDTO<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<ResponseDTO<Models.Discount>> GetById(int id)
        {
            var discounts = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where id=@Id", new { Id = id })).SingleOrDefault();

            if (discounts == null) return ResponseDTO<Models.Discount>.Fail("Discount not found!", 404);

            return ResponseDTO<Models.Discount>.Success(discounts, 200);
        }

        public async Task<ResponseDTO<NoContentDTO>> Save(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("insert into discount (userid, rate, code) values(@UserId, @Rate, @Code)", discount);

            if (status > 0) return ResponseDTO<NoContentDTO>.Success(204);

            return ResponseDTO<NoContentDTO>.Fail("Discount not found!", 404);
        }

        public async Task<ResponseDTO<NoContentDTO>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, rate=@Rate, code=@Code where id=@Id", new { UserId = discount.UserId, Rate = discount.Rate, Code = discount.Code, Id = discount.Id });

            if (status > 0) return ResponseDTO<NoContentDTO>.Success(204);

            return ResponseDTO<NoContentDTO>.Fail("Discount not found!", 404);
        }

        public async Task<ResponseDTO<NoContentDTO>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });

            if (status > 0) return ResponseDTO<NoContentDTO>.Success(204);

            return ResponseDTO<NoContentDTO>.Fail("Discount not found!", 404);
        }

        public async Task<ResponseDTO<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where code=@Code and userid=@UserId ", new { Code = code, UserId = userId })).FirstOrDefault();

            if (discounts == null) return ResponseDTO<Models.Discount>.Fail("Discount not found!", 404);

            return ResponseDTO<Models.Discount>.Success(discounts, 200);
        }
    }
}
