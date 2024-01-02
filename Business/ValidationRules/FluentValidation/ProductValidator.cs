using DataAccess.Helper;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Ürün adı boş geçilemez.");
            RuleFor(p => p.Name)
                .MinimumLength(2).WithMessage("Ürün adı minimum 2 karakter olmalıdır.")
                .MaximumLength(250).WithMessage("Ürün adı maksimum 250 karakter olmalıdır.");
            RuleFor(p => p.Description)
                .MaximumLength(250).WithMessage("Ürün açıklaması maksimum 250 karakter olmalıdır.");
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
