namespace FreeCourse.Web.Models.Catalog
{
    public class CourseVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public string PictureUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        public FeatureVM Feature { get; set; }

        public string CategoryId { get; set; }

        public CategoryVM Category { get; set; }
    }
}
