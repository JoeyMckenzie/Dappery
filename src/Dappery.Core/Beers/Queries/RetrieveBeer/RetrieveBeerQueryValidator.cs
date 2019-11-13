namespace Dappery.Core.Beers.Queries.RetrieveBeer
{
    using FluentValidation;

    public class RetrieveBeerQueryValidator : AbstractValidator<RetrieveBeerQuery>
    {
        public RetrieveBeerQueryValidator()
        {
            RuleFor(b => b.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Must supply an ID to retrieve a beer")
                .GreaterThanOrEqualTo(1)
                .WithMessage("Must be a valid beer ID");

        }
    }
}