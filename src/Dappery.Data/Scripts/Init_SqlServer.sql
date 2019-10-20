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


-- Seed data
INSERT INTO [Breweries]  (Name, CreatedAt, UpdatedAt)
VALUES 
    (
        'Fall River Brewery', 
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0), 
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0)
    );

INSERT INTO [Breweries]  (Name, CreatedAt, UpdatedAt)
VALUES 
    (
        'Sierra Nevada Brewing Company', 
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0), 
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0)
    );

INSERT INTO [Addresses] (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
VALUES 
    (
        '1030 E Cypress Ave Ste D',
        'Redding',
        'CA',
        '96002',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        1
    );

INSERT INTO [Addresses] (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
VALUES 
    (
        '1075 E 20th St',
        'Chico',
        'CA',
        '95928',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        2
    );

INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
VALUES
    (
        'Hexagenia',
        'Ipa',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        1
    );

INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
VALUES
    (
        'Widowmaker',
        'DoubleIpa',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        1
    );

INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
VALUES
    (
        'Hooked',
        'Lager',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        1
    );


INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
VALUES
    (
        'Pale Ale',
        'PaleAle',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        2
    );

INSERT INTO [Beers] (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
VALUES
    (
        'Old Chico',
        'WheatAle',
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        DATETIME2FROMPARTS(2019, 10, 11, 12, 0, 0, 0, 0),
        2
    );

-- A few stored procedures for utility
-- DROP PROCEDURE IF EXISTS [GetBeersByBreweryId];
CREATE PROCEDURE [GetBeersByBreweryId] @BreweryId INT
AS
-- Remove the 'rows effected' message
    SET NOCOUNT ON;

    SELECT
        [br].[Id],
        [b].[Id],
        [br].[Name],
        [b].[Name],
        [b].[BeerStyle]
    FROM [Breweries] AS [br]
             JOIN [Beers] AS [b] ON [br].[Id] = [b].[BreweryId]
    WHERE [br].[Id] = @BreweryId;
GO

-- DROP PROCEDURE IF EXISTS [GetAddressByBreweryId];
CREATE PROCEDURE [GetAddressByBreweryId] @BreweryId INT
AS
-- Remove the 'rows effected' message
    SET NOCOUNT ON;

    SELECT
        [br].[Id],
        [br].[Name],
        [a].[City],
        [a].[StreetAddress],
        [a].[State],
        [a].[ZipCode]
    FROM [Breweries] AS [br]
             JOIN [Addresses] AS [a] ON [br].[Id] = [a].[BreweryId]
    WHERE [br].[Id] = @BreweryId;
GO

-- Test our procedures, make sure everything looks good
EXEC [GetBeersByBreweryId] 1;
EXEC [GetBeersByBreweryId] 2;
EXEC [GetAddressByBreweryId] 1;
EXEC [GetAddressByBreweryId] 2;
