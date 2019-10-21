namespace Dappery.Data
{
    using System;
    using System.Data;
    using Core.Data;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Data.Sqlite;
    using Npgsql;
    using Repositories;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;

        public UnitOfWork(string? connectionString, bool isPostgres = false)
        {
            // Based on our database implementation, we'll need a reference to the last row inserted
            string rowInsertRetrievalQuery; 
            
            // If no connection string is passed, we'll assume we're running with our SQLite database provider
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                _dbConnection = new SqliteConnection("Data Source=:memory:");
                rowInsertRetrievalQuery = "; SELECT last_insert_rowid();";
            }
            else
            {
                _dbConnection = isPostgres ? (IDbConnection) new NpgsqlConnection(connectionString) : new SqlConnection(connectionString);
                rowInsertRetrievalQuery = isPostgres ? "returning Id;" : "; SELECT CAST(SCOPE_IDENTITY() as int);" ;
            }
            
            // Open our connection, begin our transaction, and instantiate our repositories
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
            BreweryRepository = new BreweryRepository(_dbTransaction, rowInsertRetrievalQuery);
            BeerRepository = new BeerRepository(_dbTransaction, rowInsertRetrievalQuery);

            // Once our connection is open, if we're running SQLite for unit tests (or that actual application), let's seed some data
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                try
                {
                    // We'll seed a couple breweries each with an address and several beers
                    SeedDatabase(_dbConnection);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not seed the database: {e.Message}");
                }
            }
        }
        
        public IBreweryRepository BreweryRepository { get; }

        public IBeerRepository BeerRepository { get; }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not commit the transaction, reason: {e.Message}");
                _dbTransaction.Rollback();
            }
            finally
            {
                _dbTransaction.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbTransaction?.Dispose();
                _dbConnection?.Dispose();
            }
        }

        private void SeedDatabase(IDbConnection dbConnection)
        {
            const string createBreweriesSql = @"
                CREATE TABLE Breweries (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT(32),
                    CreatedAt DATE,
                    UpdatedAt DATE
                );
            ";
            
            const string createBeersSql = @"
                CREATE TABLE Beers (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT(32),
                    BeerStyle TEXT(16),
                    CreatedAt DATE,
                    UpdatedAt DATE,
                    BreweryId INT NOT NULL,
                    CONSTRAINT FK_Beers_Breweries_Id FOREIGN KEY (BreweryId)
                        REFERENCES Breweries (Id) ON DELETE CASCADE
                );
            ";
            
            const string createAddressSql = @"
                CREATE TABLE Addresses (
                    Id INTEGER PRIMARY KEY,
                    StreetAddress TEXT(32),
                    City TEXT(32),
                    State TEXT(32),
                    ZipCode TEXT(8),
                    CreatedAt DATE,
                    UpdatedAt DATE,
                    BreweryId INTEGER NOT NULL,
                    CONSTRAINT FK_Address_Breweries_Id FOREIGN KEY (BreweryId)
                        REFERENCES Breweries (Id) ON DELETE CASCADE
                );
            ";
            
            // Add our tables
            dbConnection.Execute(createBreweriesSql, _dbTransaction);
            dbConnection.Execute(createBeersSql, _dbTransaction);
            dbConnection.Execute(createAddressSql, _dbTransaction);
            
            // Seed our data
            dbConnection.Execute(@"
                INSERT INTO Breweries (Name, CreatedAt, UpdatedAt)
                VALUES 
                    (
                        'Fall River Brewery', 
                        CURRENT_DATE, 
                        CURRENT_DATE 
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Breweries (Name, CreatedAt, UpdatedAt)
                VALUES 
                    (
                        'Sierra Nevada Brewing Company', 
                        CURRENT_DATE, 
                        CURRENT_DATE 
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Addresses (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
                VALUES 
                    (
                        '1030 E Cypress Ave Ste D',
                        'Redding',
                        'CA',
                        '96002',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        1
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Addresses (StreetAddress, City, State, ZipCode, CreatedAt, UpdatedAt, BreweryId)
                VALUES 
                    (
                        '1075 E 20th St',
                        'Chico',
                        'CA',
                        '95928',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        2
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                VALUES
                    (
                        'Hexagenia',
                        'Ipa',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        1
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                VALUES
                    (
                        'Widowmaker',
                        'DoubleIpa',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        1
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                VALUES
                    (
                        'Hooked',
                        'Lager',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        1
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                VALUES
                    (
                        'Pale Ale',
                        'PaleAle',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        2
                    );",
                transaction: _dbTransaction);
            
            dbConnection.Execute(@"
                INSERT INTO Beers (Name, BeerStyle, CreatedAt, UpdatedAt, BreweryId)
                VALUES
                    (
                        'Hazy Little Thing',
                        'NewEnglandIpa',
                        CURRENT_DATE,
                        CURRENT_DATE,
                        2
                    );",
                transaction: _dbTransaction);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}