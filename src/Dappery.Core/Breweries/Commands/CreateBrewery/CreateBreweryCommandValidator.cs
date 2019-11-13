namespace Dappery.Core.Breweries.Commands.CreateBrewery
{
    using Extensions;
    using FluentValidation;

    public class CreateBreweryCommandValidator : AbstractValidator<CreateBreweryCommand>
    {
        public CreateBreweryCommandValidator()
        {
            RuleFor(b => b.Dto)
                .NotNull()
                .WithMessage("A request must contain valid creation data");

            RuleFor(b => b.Dto.Name)
                .NotNullOrEmpty();

            RuleFor(b => b.Dto.Address)
                .NotNull()
                .WithMessage("Must supply the address of the brewery when creating");

            RuleFor(b => b.Dto.Address!.City)
                .NotNullOrEmpty();

            RuleFor(b => b.Dto.Address!.State)
                .HasValidStateAbbreviation();

            RuleFor(b => b.Dto.Address!.StreetAddress)
                .HasValidStreetAddress();

            RuleFor(b => b.Dto.Address!.ZipCode)
                .HasValidZipCode();
        }
    }
}