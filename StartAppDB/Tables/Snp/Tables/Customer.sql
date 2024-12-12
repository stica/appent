CREATE TABLE [snp].[Customer]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[Status] INT NOT NULL,
	[CreatedAt] DATETIME2 (7) NOT NULL,
	[UpdatedAt] DATETIME2 (7),
	[ContactId] INT NOT NULL,
	CONSTRAINT FK_Customer_Contact FOREIGN KEY (ContactId) REFERENCES [snp].[Contact](Id) ON DELETE CASCADE
)
