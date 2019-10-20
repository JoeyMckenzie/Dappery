-- Create our beer and brewery tables
drop table if exists Breweries cascade;
create table Breweries (  
    Id serial not null primary key,
    Name varchar(32),
    CreatedAt date,
    UpdatedAt date
);


drop table if exists Beers;
create table Beers (
    Id serial not null primary key,
    Name varchar(32),
    BeerStyle varchar(16),
    CreatedAt date,
    UpdatedAt date,
    BreweryId INT NOT NULL,
    -- A beer has a one-to-one relation, i.e. one beer belongs to one brewery
    constraint FK_Beers_Breweries_Id foreign key (BreweryId)
        references Breweries (Id) on delete cascade
);
create index IX_Beers_BreweryId on Beers (BreweryId);


drop table if exists Addresses;
create table Addresses (
    Id serial not null primary key,
    StreetAddress varchar(32),
    City varchar(32),
    State varchar(32),
    ZipCode varchar(8),
    CreatedAt date,
    UpdatedAt date,
    BreweryId int not null,
    -- An address has a one-sided 1-to-1 relation, i.e. a brewery has one address
    constraint FK_Address_Breweries_Id foreign key (BreweryId)
        references Breweries (Id) on delete cascade
);
create index IX_Address_BreweryId on Addresses (BreweryId);


-- Seed data
insert into Breweries  (Name, CreatedAt, UpdatedAt)
values
    (
        'Fall River Brewery', 
        date(now()),
        date(now())
    );

insert into Breweries  (Name, CreatedAt, UpdatedAt)
values 
    (
        'Sierra Nevada Brewing Company', 
        date(now()), 
        date(now())
    );

insert into Addresses (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
values 
    (
        '1030 E Cypress Ave Ste D',
        'Redding',
        'CA',
        96002,
        date(now()),
        date(now()),
        1
    );

insert into Addresses (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
values 
    (
        '1075 E 20th St',
        'Chico',
        'CA',
        '95928',
        date(now()),
        date(now()),
        2
    );

insert into Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
values
    (
        'Hexagenia',
        'Ipa',
        date(now()),
        date(now()),
        1
    );

insert into Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
values
    (
        'Widowmaker',
        'DoubleIpa',
        date(now()),
        date(now()),
        1
    );

insert into Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
values
    (
        'Hooked',
        'Lager',
        date(now()),
        date(now()),
        1
    );


insert into Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
values
    (
        'Pale Ale',
        'PaleAle',
        date(now()),
        date(now()),
        2
    );

insert into Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
values
    (
        'Old Chico',
        'WheatAle',
        date(now()),
        date(now()),
        2
    );
