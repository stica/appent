CREATE TABLE [SecurityManagement].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(50) NULL, 
    [IsConfirmed] BIT NOT NULL,
    [IsAdmin] BIT NOT NULL,
    [ChangePasswordOnNextLogin] BIT NULL,
    [CityId] INT NULL,
    [CountryId] INT NULL,
    [DateCreated] DATE NOT NULL,
    [IsEnabled] bit NOT NULL,
)
