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
        }
    }
}
