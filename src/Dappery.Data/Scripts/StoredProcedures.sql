-- A few stored procedures for utility
DROP PROCEDURE IF EXISTS [GetBeersByBreweryId];
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

DROP PROCEDURE IF EXISTS [GetAddressByBreweryId];
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