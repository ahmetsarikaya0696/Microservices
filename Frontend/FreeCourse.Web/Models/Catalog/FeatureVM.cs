using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class FeatureVM
    {
        [Required, DisplayName("Kurs Süresi")]
        public int Duration { get; set; }
    }
}
