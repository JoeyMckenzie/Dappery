namespace Dappery.Core.Beers.Commands.CreateBeer
{
    using Extensions;
    using FluentValidation;

    public class CreateBeerCommandValidator : AbstractValidator<CreateBeerCommand>
    {
        public CreateBeerCommandValidator()
        {
            RuleFor(b => b.Beer)
                .NotNull()
                .WithMessage("Must supply a request object to create a beer");
            
            RuleFor(b => b.Beer.Name)
                .NotNullOrEmpty();
            
            RuleFor(b => b.Beer.Style)
                .NotNullOrEmpty();

            RuleFor(b => b.Beer.Brewery)
                .NotNull()
                .WithMessage("A beer cannot be created without a brewery");

            RuleFor(b => b.Beer.Id)
                .NotNull()
                .WithMessage("Must supply the brewery ID");
        }
    }
}