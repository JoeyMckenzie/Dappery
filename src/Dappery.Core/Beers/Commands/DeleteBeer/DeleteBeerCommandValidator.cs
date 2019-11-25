namespace Dappery.Core.Beers.Commands.DeleteBeer
{
    using FluentValidation;

    public class DeleteBeerCommandValidator : AbstractValidator<DeleteBeerCommand>
    {
        public DeleteBeerCommandValidator()
        {
            RuleFor(b => b.BeerId)
                .NotNull()
                .WithMessage("Must supply the beer ID");
        }
    }
}