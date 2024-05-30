CREATE TABLE [SecurityManagement].[Company]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL,  
    [Addresss] NVARCHAR(50) NOT NULL,  
    [PhoneNumber] NVARCHAR(50) NOT NULL, 
    [State] NVARCHAR(50) NULL, 
    [NumberOfTrucsAllowed] INT NOT NULL, 
    [ShortName] NVARCHAR(50) NULL, 
    [IsApproved] BIT NOT NULL,
    [IsAdmin] BIT NOT NULL,
)
