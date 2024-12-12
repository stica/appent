CREATE TABLE [snp].[Contact]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[PhoneNumber] VARCHAR(255) NOT NULL,
	[CreatedAt] DATETIME2 (7) NOT NULL,
	[UpdatedAt] DATETIME2 (7)
)
