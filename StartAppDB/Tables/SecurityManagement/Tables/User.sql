CREATE TABLE [SecurityManagement].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(50) NULL, 
    [IsConfirmed] BIT NOT NULL,
    [CompanyId] INT NOT NULL,
    [IsAdmin] BIT NOT NULL,
    [ChangePasswordOnNextLogin] BIT NULL,
    CONSTRAINT FK_User_Company FOREIGN KEY (CompanyId) REFERENCES [SecurityManagement].[Company](Id)
)
