using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Model;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Course
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Course, CourseCreateDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();

            // Category
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();

            // Feature
            CreateMap<Feature, FeatureDTO>().ReverseMap();
        }
    }
}
