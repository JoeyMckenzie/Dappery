namespace Dappery.Core.Infrastructure
{
    using AutoMapper;
    using Domain.Dtos;
    using Domain.Dtos.Beer;
    using Domain.Dtos.Brewery;
    using Domain.Entities;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<Beer, BeerDto>()
                .ForMember(dto => dto.Brewery, options => options.MapFrom(b => b.Brewery))
                .ForMember(dto => dto.Name, options => options.MapFrom(b => b.Name))
                .ForMember(dto => dto.Style, options => options.MapFrom(b => b.BeerStyle.ToString()))
                .ForMember(dto => dto.Id, options => options.MapFrom(b => b.Id))
                .ForAllOtherMembers(options => options.Ignore());

            CreateMap<Brewery, BreweryDto>()
                .ForMember(dto => dto.Address, options => options.MapFrom(br => br.Address))
                .ForMember(dto => dto.Name, options => options.MapFrom(br => br.Name))
                .ForMember(dto => dto.Id, options => options.MapFrom(br => br.Id))
                .ForMember(dto => dto.Beers,options => options.MapFrom(br => br.Beers))
                .ForAllOtherMembers(options => options.Ignore());

            CreateMap<Address, AddressDto>()
                .ForMember(dto => dto.StreetAddress, options => options.MapFrom(a => a.StreetAddress))
                .ForMember(dto => dto.City, options => options.MapFrom(a => a.City))
                .ForMember(dto => dto.State, options => options.MapFrom(a => a.State))
                .ForMember(dto => dto.ZipCode, options => options.MapFrom(a => a.ZipCode))
                .ForAllOtherMembers(options => options.Ignore());
            
            // DTO to entity mappings
            CreateMap<BeerDto, Beer>()
                .ForMember(b => b.Brewery, options => options.MapFrom(dto => dto.Brewery))
                .ForMember(b => b.Name, options => options.MapFrom(dto => dto.Name))
                .ForMember(b => b.BeerStyle, options => options.MapFrom(dto => dto.Style))
                .ForMember(b => b.Id, options => options.MapFrom(dto => dto.Id));

            CreateMap<BreweryDto, Brewery>()
                .ForMember(br => br.Address, options => options.MapFrom(dto => dto.Address))
                .ForMember(br => br.Name, options => options.MapFrom(dto => dto.Name))
                .ForMember(br => br.Beers, options => options.MapFrom(dto => dto.Beers));

            CreateMap<AddressDto, Address>()
                .ForMember(a => a.StreetAddress, options => options.MapFrom(dto => dto.StreetAddress))
                .ForMember(a => a.City, options => options.MapFrom(dto => dto.City))
                .ForMember(a => a.State, options => options.MapFrom(dto => dto.State))
                .ForMember(a => a.ZipCode, options => options.MapFrom(dto => dto.ZipCode));
        }
    }
}