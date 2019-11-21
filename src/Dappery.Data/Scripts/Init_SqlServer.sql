-- Initialize a fresh database
use [master];
DROP DATABASE IF EXISTS [Dappery];
CREATE DATABASE [Dappery];
USE [Dappery];


CREATE TABLE [Breweries] (
    [Id] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(32),
    [CreatedAt] DATETIME2,
    [UpdatedAt] DATETIME2,
    CONSTRAINT [PK_Breweries] PRIMARY KEY ([Id])
);


-- Create our beer and brewery tables
CREATE TABLE [Beers] (
    [Id] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(32),
    [BeerStyle] NVARCHAR(16),
    [CreatedAt] DATETIME2,
    [UpdatedAt] DATETIME2,
    [BreweryId] INT NOT NULL
    CONSTRAINT [PK_Beers] PRIMARY KEY ([Id]),
    -- A beer has a one-to-one relation, i.e. one beer belongs to one brewery
    CONSTRAINT [FK_Beers_Breweries_Id] FOREIGN KEY ([BreweryId])
        REFERENCES [Breweries] ([Id]) ON DELETE CASCADE
);
CREATE INDEX [IX_Beers_BreweryId] ON [Beers] ([BreweryId]);


CREATE TABLE [Addresses] (
    [Id] INT NOT NULL IDENTITY,
    [StreetAddress] NVARCHAR(32),
    [City] NVARCHAR(32),
    [State] NVARCHAR(32),
    [ZipCode] VARCHAR(8),
    [CreatedAt] DATETIME2,
    [UpdatedAt] DATETIME2,
    [BreweryId] INT NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY ([Id]),
    -- An address has a one-sided 1-to-1 relation, i.e. a brewery has one address
    CONSTRAINT [FK_Address_Breweries_Id] FOREIGN KEY ([BreweryId])
        REFERENCES [Breweries] ([Id]) ON DELETE CASCADE
);
CREATE INDEX [IX_Address_BreweryId] ON [Addresses] ([BreweryId]);

-- Seed our transactional data
BEGIN TRANSACTION
    
    INSERT INTO [Breweries]  (Name, CreatedAt, UpdatedAt)
    VALUES 
        (
            'Fall River Brewery', 
            SYSDATETIME(), 
            SYSDATETIME()
        );
    
    INSERT INTO [Breweries]  (Name, CreatedAt, UpdatedAt)
    VALUES 
        (
            'Sierra Nevada Brewing Company',
            SYSDATETIME(),
            SYSDATETIME()
        );
    
    INSERT INTO [Addresses] (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
    VALUES 
        (
            '1030 E Cypress Ave Ste D',
            'Redding',
            'CA',
            '96002',
            SYSDATETIME(),
            SYSDATETIME(),
            1
        );
    
    INSERT INTO [Addresses] (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
    VALUES 
        (
            '1075 E 20th St',
            'Chico',
            'CA',
            '95928',
            SYSDATETIME(),
            SYSDATETIME(),
            2
        );
    
    INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
    VALUES
        (
            'Hexagenia',
            'Ipa',
            SYSDATETIME(),
            SYSDATETIME(),
            1
        );
    
    INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
    VALUES
        (
            'Widowmaker',
            'DoubleIpa',
            SYSDATETIME(),
            SYSDATETIME(),
            1
        );
    
    INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
    VALUES
        (
            'Hooked',
            'Lager',
            SYSDATETIME(),
            SYSDATETIME(),
            1
        );
    
    
    INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
    VALUES
        (
            'Pale Ale',
            'PaleAle',
            SYSDATETIME(),
            SYSDATETIME(),
            2
        );
    
    INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
    VALUES
        (
            'Old Chico',
            'WheatAle',
            SYSDATETIME(),
            SYSDATETIME(),
            2
        );

COMMIT TRANSACTION;
