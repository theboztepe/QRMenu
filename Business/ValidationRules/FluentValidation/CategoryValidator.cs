using DataAccess.Helper;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez.");
            RuleFor(c => c.Name)
                .MinimumLength(2).WithMessage("Kategori adı minimum 2 karakter olmalıdır.")
                .MaximumLength(250).WithMessage("Kategori adı maksimum 250 karakter olmalıdır.");
            RuleFor(c => c.Description)
                .MaximumLength(250).WithMessage("Kategori açıklaması maksimum 250 karakter olmalıdır.");
            RuleFor(p => p.Image)
                .Must(ImageFileTypeControl).WithMessage("Dosya türü desteklenmemektedir.")
                .Must(ImageFileSizeControl).WithMessage("Yüklenen dosyanın boyutu çok yüksek, maksimum 100kb olmalıdır.");
        }

        private bool ImageFileTypeControl(string arg)
        {
            ImageControl imageControl = new();
            return string.IsNullOrEmpty(arg) || imageControl.ImageFileTypeControl(arg);
        }

        private bool ImageFileSizeControl(string arg)
        {
            ImageControl imageControl = new();
            return string.IsNullOrEmpty(arg) || imageControl.ImageFileSizeControl(arg);
        }
    }
}
