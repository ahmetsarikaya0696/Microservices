using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim alanı boş olamaz!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı boş olamaz!");

            RuleFor(x => x.Feature.Duration)
                .InclusiveBetween(1, int.MaxValue).WithMessage("Geçerli bir süre alanı giriniz!");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Fiyat alanı boş olamaz!")
                .PrecisionScale(6, 2, false).WithMessage("Hatalı para formatı!");

            RuleFor(x => x.CategoryId)
               .NotEmpty().WithMessage("Kategori alanı seçilmelidir!");
        }
    }
}
