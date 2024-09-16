using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Model;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(c => true).ToListAsync();

            return ResponseDTO<List<CategoryDTO>>.Success(_mapper.Map<List<CategoryDTO>>(categories), 200);
        }


        public async Task<ResponseDTO<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDTO)
        {
            var category = _mapper.Map<Category>(categoryCreateDTO);
            await _categoryCollection.InsertOneAsync(category);
            return ResponseDTO<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
        }

        public async Task<ResponseDTO<CategoryDTO>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null) return ResponseDTO<CategoryDTO>.Fail($"Category not found! (ID = {id})", 404);

            return ResponseDTO<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
        }

    }
}
