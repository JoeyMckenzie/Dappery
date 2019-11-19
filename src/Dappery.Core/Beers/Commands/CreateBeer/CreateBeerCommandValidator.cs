namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using Extensions;
    using FluentValidation;

    public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
    {
        public CreateBeerCommandValidator()
        {
            RuleFor(b => b.Dto)
                .NotNull()
                .WithMessage("Must supply a request object to create a beer");
            
            RuleFor(b => b.Dto.Name)
                .NotNullOrEmpty();
            
            RuleFor(b => b.Dto.Style)
                .NotNullOrEmpty();

           RuleFor(b => b.Dto.BreweryId)
               .NotNull()
               .WithMessage("Must supply the brewery ID");
        }
    }
}