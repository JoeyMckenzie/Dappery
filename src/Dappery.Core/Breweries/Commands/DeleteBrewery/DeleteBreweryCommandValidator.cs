namespace Dappery.Core.Breweries.Commands.DeleteBrewery
{
    using FluentValidation;

    public class DeleteBreweryCommandValidator : AbstractValidator<DeleteBreweryCommand>
    {
        public DeleteBreweryCommandValidator()
        {
            RuleFor(b => b.BreweryId)
                .NotNull()
                .WithMessage("Must supply the brewery ID");
        }
    }
}