using System.ComponentModel;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        [DisplayName("Kurs İsmi")]
        public string Name { get; set; }

        [DisplayName("Kurs Açıklaması")]
        public string Description { get; set; }

        [DisplayName("Kurs Fiyatı")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public FeatureVM Feature { get; set; }

        [DisplayName("Kurs Kategorisi")]
        public string CategoryId { get; set; }

        [DisplayName("Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
