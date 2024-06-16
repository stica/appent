CREATE TABLE [SecurityManagement].[LoginSession]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] INT NOT NULL,
	[Created] DATETIME NOT NULL,
	[Expires] DATETIME NOT NULL,
	[IpAddress] NVARCHAR(50) NOT NULL,
	[RefreshToken] NVARCHAR(MAX) NOT NULL,
	[AccessToken] NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK_LoginSession_User] FOREIGN KEY ([UserId]) REFERENCES [SecurityManagement].[User]([Id]),
)
