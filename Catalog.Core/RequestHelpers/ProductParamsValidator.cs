using FluentValidation;

namespace Catalog.Core.RequestHelpers
{
    public class ProductParamsValidator : AbstractValidator<ProductParams>
    {
        public ProductParamsValidator() 
        {
            RuleFor(p => p.MerchantName)
                   .NotEmpty().WithMessage("Merchant Name is required.")
                   .NotNull().WithMessage("Merchant Name can not be null.");  
        }
    }
}
